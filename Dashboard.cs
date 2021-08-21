using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;

using System.IO.Compression;

namespace LPR
{
    
    public partial class Dashboard : Form
    {
        //Declare variables scoped to entire Dashboard.
        private BindingSource bs_Plates = new BindingSource();
        private BindingSource bs_OtherHits = new BindingSource();
        private BindingSource bs_PlateSummary = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private SqlCommand sql_Command = new SqlCommand();
        private SqlConnection sql_Connection = new SqlConnection();
        private Image img_Full;
        private Image img_License;
        private Image img_Car;

        private DateTime tck_Last = DateTime.Now;
        private DateTime tck_Current;

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Settings_Populate();

            //Set Date Ranges
            dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            dtp_Start.Value =  DateTime.Now.AddDays(-1);
            dtp_End.Value = DateTime.Now;

            //Set formatting of Main GridView
            {
                var withBlock = dgv_Plates;
                withBlock.RowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
                withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.PowderBlue;
                withBlock.Font = new Font("Courier New", 10, FontStyle.Regular);
            }

            //Set formatting of Plate GridView
            {
                var withBlock = dgv_OtherHits;
                withBlock.RowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
                withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.PowderBlue;
            }

            //Set formatting of Summary GridView
            {
                var withBlock = dgv_PlateSummary;
                withBlock.RowsDefaultCellStyle.BackColor = Color.LightSteelBlue;
                withBlock.AlternatingRowsDefaultCellStyle.BackColor = Color.PowderBlue;
                withBlock.Font = new Font("Courier New", 10, FontStyle.Regular);
            }

            //Populate GridView
            dgv_OtherHits.DataSource = bs_OtherHits;
            dgv_Plates.DataSource = bs_Plates;
            dgv_PlateSummary.DataSource = bs_PlateSummary;
            Load_Plates();

            dgv_Plates.Focus();

            try
            {
                Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());
            }
            catch (Exception e2)
            {
                write_event(e2.Message.ToString(), EventLogEntryType.Warning);
            }

            Load_DBStats();

            //Set a bunch of fields to hidden
            dgv_Plates.Columns["Picture"].Visible = false;
            dgv_Plates.Columns["plate_x1"].Visible = false;
            dgv_Plates.Columns["plate_x2"].Visible = false;
            dgv_Plates.Columns["plate_x3"].Visible = false;
            dgv_Plates.Columns["plate_x4"].Visible = false;
            dgv_Plates.Columns["plate_y1"].Visible = false;
            dgv_Plates.Columns["plate_y2"].Visible = false;
            dgv_Plates.Columns["plate_y3"].Visible = false;
            dgv_Plates.Columns["plate_y4"].Visible = false;
            dgv_Plates.Columns["vehicle_region_height"].Visible = false;
            dgv_Plates.Columns["vehicle_region_width"].Visible = false;
            dgv_Plates.Columns["vehicle_region_x"].Visible = false;
            dgv_Plates.Columns["vehicle_region_y"].Visible = false;
            dgv_Plates.Columns["pk"].Visible = false;
            dgv_Plates.Columns["Alert_Address"].Visible = false;
            dgv_Plates.RowHeadersVisible = false;

            if (Constants.HideALPRMM == "True")
            {
                dgv_Plates.Columns["Color"].Visible = false;
                dgv_Plates.Columns["Make"].Visible = false;
                dgv_Plates.Columns["Model"].Visible = false;
                dgv_Plates.Columns["Body"].Visible = false;
            }

            // If you want 24hr Format in the Grids
            if (Constants.format24hr == "True")
            {
                dgv_Plates.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
                //dgv_OtherHits.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
            }
            


            cb_CameraList.Items.Clear();
            //Added 2020.10.16 Add Distinct Camera List
            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Select Distinct Camera from LPR_PlateHits", db_connection))
                {
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            cb_CameraList.Items.Add(db_reader["Camera"].ToString());
                        }
                    }
                    db_connection.Close();
                }
            }
            StartTimer();
        }
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            if (tc_Dashboard.SelectedTab == tp_Main)
            {
                Load_Plates();
                dgv_Plates.Focus();
                try
                {
                    Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());
                }
                catch { }      
            }
            else if(tc_Dashboard.SelectedTab == tp_Summary)
            {
                dgv_PlateSummary.Focus();

                Load_PlateSummary();
                Load_PlateSummaryChart();
                Load_PlateSummaryChart2();
            }
            Load_DBStats();
        }
        private void Tc_Dashboard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tc_Dashboard.SelectedTab == tp_Summary)
            {
                Load_PlateSummary();
                Load_PlateSummaryChart();
                Load_PlateSummaryChart2();

                groupBox2.Parent = tc_Dashboard.SelectedTab;
                gb_SpecificPlate.Parent = tc_Dashboard.SelectedTab;
                btn_Refresh.Parent = tc_Dashboard.SelectedTab;
                pb_Car.Parent = tc_Dashboard.SelectedTab;
            }
            else if (tc_Dashboard.SelectedTab == tp_Main)
            {
                groupBox2.Parent = tc_Dashboard.SelectedTab;
                gb_SpecificPlate.Parent = tc_Dashboard.SelectedTab;
                btn_Refresh.Parent = tc_Dashboard.SelectedTab;
                pb_Car.Parent = tc_Dashboard.SelectedTab;
            }
        }
        private void Dtp_Start_Enter(object sender, EventArgs e)
        {
            dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        }

        private void StartTimer() // 5 Minute timer, does nothing unless specific settings are set.
        {
                timer_Download.Interval = 5 * 60 * 1000; // 5 Minutes
                timer_Download.Enabled = true;
        }
        private void Timer_Download_Tick(object sender, EventArgs e)
        {
            timer_Download.Stop();
            tck_Current = DateTime.Now;
            if (tck_Current.Day > tck_Last.Day || tck_Current.Month > tck_Last.Month || tck_Current.Year > tck_Last.Year) // Just after midnight
            {
                Perform_Daily_Tasks();
            }
            tck_Last = tck_Current;
            timer_Download.Start();
        }

        private void Perform_Daily_Tasks()
        {
            if (chk_DailyCheck.Checked == true)
            {
                // Load uuids to look up
                DataTable dt_SQLData = new DataTable();
                dataAdapter = new SqlDataAdapter("Select Top 125 * From (Select Distinct PH.best_plate, PH.region From LPR_PlateHits as PH Where PH.best_plate not in (Select Plate From LPR_AutoCheck) Union Select Distinct PH.best_plate, PH.region From LPR_PlateHits as PH Where Cast(PH.epoch_time_start as date) >= DateAdd(month, -1, GetDate()) AND PH.best_plate not in (Select Plate from LPR_AutoCheck Where Date_Imported is not NULL AND (Status = 'Manual' OR Date_Imported >= DateAdd(month, -1, GetDate())))) as T1", Constants.str_SqlCon);
                dataAdapter.Fill(dt_SQLData);

                foreach (DataRow sqlRow in dt_SQLData.Rows)
                {

                    //// Spin up a new thread for generating alerts since these will have a slight delay and shouldn't impact additional downloads.
                    //new System.Threading.Thread(() =>
                    //{
                    //    System.Threading.Thread.CurrentThread.IsBackground = true;
                    //    LicensePlateData_Lookup(sqlRow["best_plate"].ToString(), sqlRow["region"].ToString());
                    //}).Start();

                    LicensePlateData_Lookup(sqlRow["best_plate"].ToString(), sqlRow["region"].ToString());
                    Application.DoEvents();
                }
            }

            if (chk_DailyLocalImport.Checked == true)
            {
                Save_Local_ALPR_List();
            }

            if (chk_DailyReport.Checked == true)
            {
                dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

                dtp_Start.Value = tck_Last.Date;
                dtp_End.Value = tck_Last.Date;

                Load_Plates();

                EmailDailyReport_CollectInfo("Auto Report");
            }

            if (chk_SQL_Backup_Daily.Checked == true)
            {
                string BackupName = DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".bak";

                using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
                {
                    using (sql_Command = new SqlCommand("BACKUP DATABASE [LPR] TO  DISK = N'" + Constants.SQL_Backup_Location + BackupName + "' WITH NOFORMAT, INIT,  NAME = N'LPR-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", db_connection))
                    {
                        db_connection.Open();
                        sql_Command.ExecuteNonQuery();
                        db_connection.Close();
                    }
                }

                using (ZipArchive zip = ZipFile.Open(Constants.SQL_Backup_Location + BackupName + ".zip", ZipArchiveMode.Create))
                {
                    zip.CreateEntryFromFile(Constants.SQL_Backup_Location + BackupName, "data/path/" + BackupName);
                }
                File.Delete(Constants.SQL_Backup_Location + BackupName);
            }
        }
        private void Btn_SimulateDaily_Click(object sender, EventArgs e)
        {
            Perform_Daily_Tasks();
        }


        #region Settings Tab
        private void Btn_SettingsSave_Click(object sender, EventArgs e)
        {
            Settings_Save();
        }
        private void Settings_Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            config.AppSettings.Settings["SqlCon"].Value = txt_SqlCon.Text;
            config.AppSettings.Settings["programName"].Value = txt_programName.Text;
            config.AppSettings.Settings["PlateStatusOptions"].Value = txt_PlateStatusOptions.Text;

            //Added 2020.10.15 - Adding e-mail capability to Viewer
            config.AppSettings.Settings["emailDefaultTo"].Value = txt_emailDefaultTo.Text;
            config.AppSettings.Settings["emailSignIn"].Value = txt_emailSignIn.Text;
            config.AppSettings.Settings["emailPassword"].Value = txt_emailPassword.Text;
            config.AppSettings.Settings["emailServer"].Value = txt_emailServer.Text;
            config.AppSettings.Settings["emailPort"].Value = txt_emailPort.Text;
            config.AppSettings.Settings["emailUseSSL"].Value = chk_emailUseSSL.Checked.ToString();
            config.AppSettings.Settings["WebServer"].Value = txt_WebServer.Text;

            config.AppSettings.Settings["LogoLocation"].Value = txt_LogoLocation.Text;
            config.AppSettings.Settings["HideALPRMM"].Value = chk_HideALPRMM.Checked.ToString();
            config.AppSettings.Settings["format24hr"].Value = chk_24hr.Checked.ToString();
            config.AppSettings.Settings["DefaultState"].Value = txt_DefaultState.Text;
            config.AppSettings.Settings["LPD_API"].Value = txt_LPD_API.Text;

            config.AppSettings.Settings["SQL_Backup_Location"].Value = txt_SQL_Backup_Location.Text;
            config.AppSettings.Settings["SQL_Backup_Daily"].Value = chk_SQL_Backup_Daily.Checked.ToString();

            config.AppSettings.Settings["DailyCheck"].Value = chk_DailyCheck.Checked.ToString();
            config.AppSettings.Settings["DailyReport"].Value = chk_DailyReport.Checked.ToString();
            config.AppSettings.Settings["DailyLocalImport"].Value = chk_DailyLocalImport.Checked.ToString();

            config.AppSettings.Settings["Image_Backup_Location"].Value = txt_Image_Backup_Location.Text;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Settings_Populate();
            if (Constants.format24hr == "True")
            {
                dgv_Plates.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
                dgv_OtherHits.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
            }
            else
            {
                dgv_Plates.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
                dgv_OtherHits.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
            }
        }
        private void Settings_Populate()
        {
            //Read in "Constants" from Configuration File (Not true constants...)
            Constants.str_SqlCon = ConfigurationManager.AppSettings["SqlCon"];
            txt_SqlCon.Text = Constants.str_SqlCon;

            Constants.str_programName = ConfigurationManager.AppSettings["programName"];
            txt_programName.Text = Constants.str_programName;
            this.Text = Constants.str_programName;

            Constants.PlateStatusOptions = ConfigurationManager.AppSettings["PlateStatusOptions"].Split(';');
            txt_PlateStatusOptions.Text = ConfigurationManager.AppSettings["PlateStatusOptions"];
            cb_PlateStatus.Items.Clear();
            cb_PlateStatus.Items.AddRange(Constants.PlateStatusOptions);

            //Added 2020.10.15 - Used for filtering main view by status
            cb_PlateStatusSearch.Items.Clear();
            cb_PlateStatusSearch.Items.AddRange(Constants.PlateStatusOptions);

            //Added 2020.10.15 - Adding e-mail capability to Viewer
            Constants.emailDefaultTo = ConfigurationManager.AppSettings["emailDefaultTo"];
            txt_emailDefaultTo.Text = Constants.emailDefaultTo;

            Constants.emailSignIn = ConfigurationManager.AppSettings["emailSignIn"];
            txt_emailSignIn.Text = Constants.emailSignIn;

            Constants.emailPassword = ConfigurationManager.AppSettings["emailPassword"];
            txt_emailPassword.Text = Constants.emailPassword;

            Constants.emailServer = ConfigurationManager.AppSettings["emailServer"];
            txt_emailServer.Text = Constants.emailServer;

            Constants.emailPort = ConfigurationManager.AppSettings["emailPort"];
            txt_emailPort.Text = Constants.emailPort;

            Constants.emailUseSSL = ConfigurationManager.AppSettings["emailUseSSL"];
            if (Constants.emailUseSSL == "True")
            {
                chk_emailUseSSL.Checked = true;
            }
            else
            {
                chk_emailUseSSL.Checked = false;
            }

            Constants.LogoLocation = ConfigurationManager.AppSettings["LogoLocation"];
            txt_LogoLocation.Text = Constants.LogoLocation;

            Constants.DefaultState = ConfigurationManager.AppSettings["DefaultState"];
            txt_DefaultState.Text = Constants.DefaultState;

            Constants.LPD_API = ConfigurationManager.AppSettings["LPD_API"];
            txt_LPD_API.Text = Constants.LPD_API;

            Constants.SQL_Backup_Location = ConfigurationManager.AppSettings["SQL_Backup_Location"];
            txt_SQL_Backup_Location.Text = Constants.SQL_Backup_Location;

            Constants.Image_Backup_Location = ConfigurationManager.AppSettings["Image_Backup_Location"];
            txt_Image_Backup_Location.Text = Constants.Image_Backup_Location;

            Constants.str_WebServer = ConfigurationManager.AppSettings["WebServer"];
            txt_WebServer.Text = Constants.str_WebServer;

            Constants.HideALPRMM = ConfigurationManager.AppSettings["HideALPRMM"];
            if (Constants.HideALPRMM == "True")
            {
                chk_HideALPRMM.Checked = true;
            }
            else
            {
                chk_HideALPRMM.Checked = false;
            }

            Constants.format24hr = ConfigurationManager.AppSettings["format24hr"];
            if (Constants.format24hr == "True")
            {
                chk_24hr.Checked = true;
            }
            else
            {
                chk_24hr.Checked = false;
            }

            Constants.DailyCheck = ConfigurationManager.AppSettings["DailyCheck"];
            if (Constants.DailyCheck == "True")
            {
                chk_DailyCheck.Checked = true;
            }
            else
            {
                chk_DailyCheck.Checked = false;
            }

            Constants.DailyReport = ConfigurationManager.AppSettings["DailyReport"];
            if (Constants.DailyReport == "True")
            {
                chk_DailyReport.Checked = true;
            }
            else
            {
                chk_DailyReport.Checked = false;
            }

            Constants.DailyLocalImport = ConfigurationManager.AppSettings["DailyLocalImport"];
            if (Constants.DailyLocalImport == "True")
            {
                chk_DailyLocalImport.Checked = true;
            }
            else
            {
                chk_DailyLocalImport.Checked = false;
            }

            Constants.SQL_Backup_Daily = ConfigurationManager.AppSettings["SQL_Backup_Daily"];
            if (Constants.SQL_Backup_Daily == "True")
            {
                chk_SQL_Backup_Daily.Checked = true;
            }
            else
            {
                chk_SQL_Backup_Daily.Checked = false;
            }
        }
        private void Btn_emailTestSend_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new System.Net.Mail.MailAddress(txt_emailSignIn.Text);
                mail.To.Add(new MailAddress(txt_emailDefaultTo.Text));
                mail.IsBodyHtml = true;
                mail.Subject = "This is a test email";
                string st = "Success!";
                mail.Body = st;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = Convert.ToInt32(txt_emailPort.Text);
                    smtp.EnableSsl = chk_emailUseSSL.Checked;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(txt_emailSignIn.Text, txt_emailPassword.Text);
                    smtp.Host = txt_emailServer.Text;
                    smtp.Send(mail);
                }
                MessageBox.Show("Successful E-mail Test!", "Test Success");
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message.ToString(), "Test Failed");
            }
        }
        private void Btn_LoadHistoricForensic_Click(object sender, EventArgs e)
        {
            string PlatesToDo = "0";
            int PlateOn = 0;

            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Select Count(*) From (Select Distinct PH.best_plate, PH.region From LPR_PlateHits as PH Where PH.best_plate not in (Select Plate From LPR_AutoCheck)) as T1", db_connection))
                {
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            PlatesToDo = db_reader[0].ToString();
                        }
                    }
                    db_connection.Close();
                }
            }

            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Select Distinct PH.best_plate, PH.region From LPR_PlateHits as PH Where PH.best_plate not in (Select Plate From LPR_AutoCheck)", db_connection))
                {
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            PlateOn += 1;
                            LicensePlateData_Lookup(db_reader["best_plate"].ToString(), db_reader["region"].ToString());
                            btn_LoadHistoricForensic.Text = "Loading " + PlateOn + "/" + PlatesToDo;
                            Application.DoEvents();
                        }
                    }
                    db_connection.Close();
                    btn_LoadHistoricForensic.Text = "Load";
                }
            }
        }

        private void Save_Local_ALPR_List()
        {
            string PlatesToDo = "0";
            int PlateOn = 0;

            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Select Count(*) From LPR_PlateHits Where best_uuid not in (Select UUID From LPR_LocalInfo)", db_connection))
                {
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            PlatesToDo = db_reader[0].ToString();
                        }
                    }
                    db_connection.Close();
                }
            }

            // Load uuids to look up
            DataTable dt_SQLData = new DataTable();
            dataAdapter = new SqlDataAdapter("Select best_uuid From LPR_PlateHits Where best_uuid not in (Select UUID From LPR_LocalInfo)", Constants.str_SqlCon);
            dataAdapter.Fill(dt_SQLData);

            foreach (DataRow sqlRow in dt_SQLData.Rows)
            {
                PlateOn += 1;
                Save_Local_ALPR_Data(sqlRow["best_uuid"].ToString());
                btn_Load_Local_ALPR.Text = "Loading " + PlateOn + "/" + PlatesToDo;
                Application.DoEvents();
            }

            btn_Load_Local_ALPR.Text = "Load Missing";
        }
        private void Save_Local_ALPR_Data(string UUID)
        {
            try
            {
                //Gets Image and saves the full image
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + UUID + ".jpg");
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                img_Full = Image.FromStream(ms);

                // Save the Image to a Network Location
                img_Full.Save(Constants.Image_Backup_Location + UUID + ".jpg");
            }
            catch (Exception e)
            {
                write_event(e.Message.ToString(), EventLogEntryType.Error);
            }


            // Get the Metadata information...
            //Try to get JSON Response
            string ALPR_json = "";

            try
            {
                ALPR_json = (new WebClient()).DownloadString(Constants.str_WebServer + "/meta/" + UUID);

                // They don't have it wrapped in brackets...
                string ALPR_json_2 = "[" + ALPR_json + "]";

                var ALPR_Data_List = JsonSerializer.Deserialize<List<ALPR_Root>>(ALPR_json_2);

                ALPR_Root ALPR_Data = new ALPR_Root();
                ALPR_Data = ALPR_Data_List.First();

                string myColor = "";
                string myMake = "";
                string myModel = "";

                try
                {
                    myColor = ALPR_Data.vehicle.color[0].name.EmptyIfNull();
                }
                catch
                {
                    myColor = "";
                }

                try
                {
                    myMake = ALPR_Data.vehicle.make[0].name.EmptyIfNull();
                }
                catch
                {
                    myMake = "";
                }

                try
                {
                    myModel = ALPR_Data.vehicle.make_model[0].name.EmptyIfNull();
                }
                catch
                {
                    myModel = "";
                }

                using (SqlConnection sql_Connection_LocalInfo = new SqlConnection(Constants.str_SqlCon))
                    {
                        //Add new entry
                        using (SqlCommand sql_Command_LocalInfo = new SqlCommand("Insert Into LPR_LocalInfo (UUID, best_color, best_make, best_model) VALUES (@UUID, @best_color, @best_make, @best_model)", sql_Connection_LocalInfo))
                        {
                            sql_Command_LocalInfo.Parameters.AddWithValue("@UUID", UUID);
                            sql_Command_LocalInfo.Parameters.AddWithValue("@best_color", myColor);
                            sql_Command_LocalInfo.Parameters.AddWithValue("@best_make", myMake);
                            sql_Command_LocalInfo.Parameters.AddWithValue("@best_model", myModel);
                            sql_Connection_LocalInfo.Open();
                            try
                            {
                                sql_Command_LocalInfo.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                write_event(e.Message.ToString(), EventLogEntryType.Error);
                            }
                            sql_Connection_LocalInfo.Close();
                        }
                    }
                }
            catch
            {
                //  The record isn't still available locally, enter in a blank entry so it doesn't try again
                using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                {
                    //Add new entry
                    using (sql_Command = new SqlCommand("Insert Into LPR_LocalInfo (UUID, best_color, best_make, best_model) VALUES (@UUID, @best_color, @best_make, @best_model)", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@UUID", UUID);
                        sql_Command.Parameters.AddWithValue("@best_color", "");
                        sql_Command.Parameters.AddWithValue("@best_make", "");
                        sql_Command.Parameters.AddWithValue("@best_model", "");
                        sql_Connection.Open();
                        try
                        {
                            sql_Command.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            write_event(e.Message.ToString(), EventLogEntryType.Error);
                        }
                        sql_Connection.Close();
                    }
                }
            }
        }
        private void btn_Load_Local_ALPR_Click(object sender, EventArgs e)
        {
            Save_Local_ALPR_List();
        }

        #endregion


        #region Main Dashboard Tab
        private void Load_Plates()
        {
            dgv_Plates.ClearSelection();
            dgv_Plates.CurrentCell = null;

            string str_PlateSearch = txt_PlateSearch.Text;
            if (str_PlateSearch == "")
            {
                str_PlateSearch = "%";
            }
            
            //Added 2020.10.15
            string str_PlateStatusSearch = cb_PlateStatusSearch.Text;
            if (str_PlateStatusSearch == "")
            {
                str_PlateStatusSearch = "%";
            }

            string str_CameraList = cb_CameraList.Text;
            if (str_CameraList == "")
            {
                str_CameraList = "%";
            }

            string str_Desc = txt_Desc_Search.Text;
            if (str_Desc == "")
            {
                str_Desc = "%";
            }

            string str_Make = txt_Search_Make.Text;
            if (str_Make == "")
            {
                str_Make = "%";
            }

            string str_Model = txt_Search_Model.Text;
            if (str_Model == "")
            {
                str_Model = "%";
            }

            string str_Color = txt_Search_Color.Text;
            if (str_Color == "")
            {
                str_Color = "%";
            }

            string str_VIN = txt_Search_VIN.Text;
            if (str_VIN == "")
            {
                str_VIN = "%";
            }

            dataAdapter = new SqlDataAdapter("Exec sp_LPR_AllPlates @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH, @Status, @Camera, @Desc, @Make, @Model, @Color, @Vin", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Plate", str_PlateSearch);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@HideNeighbors", chk_HideNeighbors.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@IdentifyDupes", chk_IdentifyDups.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@TopPH", 9999999);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Status", str_PlateStatusSearch);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Camera", str_CameraList);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Desc", str_Desc);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Make", str_Make);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Model", str_Model);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Color", str_Color);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@VIN", str_VIN);
            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            dgv_Plates.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_Plates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            bs_Plates.DataSource = table;
            dgv_Plates.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            dgv_Plates.Columns["Distinct Days"].Width = 75;
            dgv_Plates.Columns["Hits Day"].Width = 42;
            dgv_Plates.Columns["Hits Week"].Width = 42;
            dgv_Plates.Columns["D"].Width = 20;
            dgv_Plates.Columns["Yr"].Width = 45;
        }
        private void Load_DBStats()
        {
            string str_PlateSearch = txt_PlateSearch.Text;
            if (str_PlateSearch == "")
            {
                str_PlateSearch = "%";
            }

            //Added 2020.10.15
            string str_PlateStatusSearch = cb_PlateStatusSearch.Text;
            if (str_PlateStatusSearch == "")
            {
                str_PlateStatusSearch = "%";
            }

            string str_CameraList = cb_CameraList.Text;
            if (str_CameraList == "")
            {
                str_CameraList = "%";
            }

            string str_Desc = txt_Desc_Search.Text;
            if (str_Desc == "")
            {
                str_Desc = "%";
            }

            string str_Make = txt_Search_Make.Text;
            if (str_Make == "")
            {
                str_Make = "%";
            }

            string str_Model = txt_Search_Model.Text;
            if (str_Model == "")
            {
                str_Model = "%";
            }

            string str_Color = txt_Search_Color.Text;
            if (str_Color == "")
            {
                str_Color = "%";
            }

            string str_VIN = txt_Search_VIN.Text;
            if (str_VIN == "")
            {
                str_VIN = "%";
            }

            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Exec sp_LPR_GetDBStats @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH, @Status, @Camera, @Desc, @Make, @Model, @Color, @VIN", db_connection))
                {
                    db_command.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
                    db_command.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
                    db_command.Parameters.AddWithValue("@Plate", str_PlateSearch);
                    db_command.Parameters.AddWithValue("@HideNeighbors", chk_HideNeighbors.Checked);
                    db_command.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");
                    db_command.Parameters.AddWithValue("@IdentifyDupes", chk_IdentifyDups.Checked);
                    db_command.Parameters.AddWithValue("@TopPH", 9999999);
                    db_command.Parameters.AddWithValue("@Status", str_PlateStatusSearch);
                    db_command.Parameters.AddWithValue("@Camera", str_CameraList);
                    db_command.Parameters.AddWithValue("@Desc", str_Desc);
                    db_command.Parameters.AddWithValue("@Make", str_Make);
                    db_command.Parameters.AddWithValue("@Model", str_Model);
                    db_command.Parameters.AddWithValue("@Color", str_Color);
                    db_command.Parameters.AddWithValue("@VIN", str_VIN);
                    db_connection.Open();

                    using (SqlDataReader db_reader = db_command.ExecuteReader())
                    {
                        while (db_reader.Read())
                        {
                            lbl_Total.Text = String.Format("{0:#,##0}", Convert.ToInt32(db_reader["Displayed_Total"].ToString())) + " / " + String.Format("{0:#,##0}", Convert.ToInt32(db_reader["All_Total"].ToString()));
                            lbl_Unique.Text = String.Format("{0:#,##0}", Convert.ToInt32(db_reader["Displayed_Distinct"].ToString())) + " / " + String.Format("{0:#,##0}", Convert.ToInt32(db_reader["All_Distinct"].ToString()));
                        }
                    }
                }
            }
        }
        private void KnownPlateUpdate()
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Delete From LPR_KnownPlates Where Plate = @Plate", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Plate", lbl_CurrentPlate.Text);
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }

                using (sql_Command = new SqlCommand("Insert Into LPR_KnownPlates (Plate, Description, Status, Alert_Address, Pushover, Priority) VALUES (@Plate, @Description, @Status, @AlertAddress, @Pushover, @Priority)", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Plate", lbl_CurrentPlate.Text);
                    sql_Command.Parameters.AddWithValue("@Description", txt_PlateDescription.Text);
                    sql_Command.Parameters.AddWithValue("@Status", cb_PlateStatus.Text);
                    sql_Command.Parameters.AddWithValue("@AlertAddress", txt_PlateAlert.Text);
                    sql_Command.Parameters.AddWithValue("@Pushover", chk_Pushover.Checked);
                    sql_Command.Parameters.AddWithValue("@Priority", chk_Priority.Checked);
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }
        }
        private void Set_Plate_Details(string str_PlateSearch)
        {
            //Clear values
            lbl_CurrentPlate.Text = str_PlateSearch;
            cb_PlateStatus.SelectedIndex = -1;
            txt_PlateDescription.Text = "";
            txt_PlateAlert.Text = "";
            
            //Populate single plate grid
            //dataAdapter = new SqlDataAdapter("Exec sp_LPR_AllPlates @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH", Constants.str_SqlCon);
            dataAdapter = new SqlDataAdapter("Exec sp_LPR_PlateHistory @Plate, @CurrentOffset, @TopPH", Constants.str_SqlCon);
            //dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", Convert.ToDateTime("6/1/2019"));
            //dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", DateTime.Now.AddDays(1));
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Plate", str_PlateSearch);
            //dataAdapter.SelectCommand.Parameters.AddWithValue("@HideNeighbors", 0);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");
            //dataAdapter.SelectCommand.Parameters.AddWithValue("@IdentifyDupes", 0);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@TopPH", txt_TopPH.Text);

            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            // Disabling resizing and hiding headers while loading to improve speed, enable again at end.
            dgv_OtherHits.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_OtherHits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            bs_OtherHits.DataSource = table;
            //dgv_OtherHits.Columns["Hits Day"].Visible = false;
            //dgv_OtherHits.Columns["Hits Week"].Visible = false;
            dgv_OtherHits.Columns["Description"].Visible = false;
            dgv_OtherHits.Columns["Status"].Visible = false;
            dgv_OtherHits.Columns["Plate"].Visible = false;
            dgv_OtherHits.Columns["Picture"].Visible = false;
            dgv_OtherHits.Columns["plate_x1"].Visible = false;
            dgv_OtherHits.Columns["plate_x2"].Visible = false;
            dgv_OtherHits.Columns["plate_x3"].Visible = false;
            dgv_OtherHits.Columns["plate_x4"].Visible = false;
            dgv_OtherHits.Columns["plate_y1"].Visible = false;
            dgv_OtherHits.Columns["plate_y2"].Visible = false;
            dgv_OtherHits.Columns["plate_y3"].Visible = false;
            dgv_OtherHits.Columns["plate_y4"].Visible = false;
            dgv_OtherHits.Columns["vehicle_region_height"].Visible = false;
            dgv_OtherHits.Columns["vehicle_region_width"].Visible = false;
            dgv_OtherHits.Columns["vehicle_region_x"].Visible = false;
            dgv_OtherHits.Columns["vehicle_region_y"].Visible = false;
            dgv_OtherHits.Columns["pk"].Visible = false;
            //dgv_OtherHits.Columns["Distinct Days"].Visible = false;
            dgv_OtherHits.Columns["Alert_Address"].Visible = false;
            dgv_OtherHits.Columns["Color"].Visible = false;
            dgv_OtherHits.Columns["Make"].Visible = false;
            dgv_OtherHits.Columns["Model"].Visible = false;
            dgv_OtherHits.Columns["Body"].Visible = false;
            dgv_OtherHits.Columns["Region"].Visible = false;
            dgv_OtherHits.Columns["Car Make"].Visible = false;
            dgv_OtherHits.Columns["Car Model"].Visible = false;
            dgv_OtherHits.Columns["Yr"].Visible = false;
            dgv_OtherHits.Columns["VIN"].Visible = false;
            dgv_OtherHits.Columns["Car Color"].Visible = false;
            dgv_OtherHits.Columns["ALPR Color"].Visible = false;
            dgv_OtherHits.Columns["ALPR Model"].Visible = false;
            dgv_OtherHits.Columns["Pushover"].Visible = false;
            dgv_OtherHits.Columns["Priority"].Visible = false;
            dgv_OtherHits.RowHeadersVisible = false;

            // If you want 24hr Format in the Grids
            if (Constants.format24hr == "True")
            {
                dgv_OtherHits.Columns["Local Time"].DefaultCellStyle.Format = "MM/dd/yyyy HH:mm:ss";
            }
            dgv_OtherHits.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);                                           

            if (tc_Dashboard.SelectedTab == tp_Main)
            {
                //Set single plate grid to same record as main grid
                String searchValue = dgv_Plates.SelectedRows[0].Cells["Picture"].Value.ToString();
                int rowIndex = -1;
                foreach (DataGridViewRow row in dgv_OtherHits.Rows)
                {
                    if (row.Cells["Picture"].Value.ToString().Equals(searchValue))
                    {
                        rowIndex = row.Index;
                        break;
                    }
                }
                if (rowIndex > -1)
                {
                    dgv_OtherHits.Rows[rowIndex].Selected = true;
                }
                
            }

            //Populate plate details into editable fields
            cb_PlateStatus.SelectedItem = dgv_OtherHits.SelectedRows[0].Cells["Status"].Value.ToString();
            txt_PlateDescription.Text = dgv_OtherHits.SelectedRows[0].Cells["Description"].Value.ToString();
            txt_PlateAlert.Text = dgv_OtherHits.SelectedRows[0].Cells["Alert_Address"].Value.ToString();
            txt_VIN.Text = dgv_OtherHits.SelectedRows[0].Cells["VIN"].Value.ToString();
            txt_Year.Text = dgv_OtherHits.SelectedRows[0].Cells["Yr"].Value.ToString();
            txt_Make.Text = dgv_OtherHits.SelectedRows[0].Cells["Car Make"].Value.ToString();
            txt_Model.Text = dgv_OtherHits.SelectedRows[0].Cells["Car Model"].Value.ToString();
            txt_Color.Text = dgv_OtherHits.SelectedRows[0].Cells["Car Color"].Value.ToString();

            lbl_ALPR_Color.Text = dgv_OtherHits.SelectedRows[0].Cells["ALPR Color"].Value.ToString();
            lbl_ALPR_MM.Text = dgv_OtherHits.SelectedRows[0].Cells["ALPR Model"].Value.ToString();

            if (dgv_OtherHits.SelectedRows[0].Cells["Pushover"].Value.ToString() == "True")
                chk_Pushover.Checked = true;
            else
                chk_Pushover.Checked = false;

            if (dgv_OtherHits.SelectedRows[0].Cells["Priority"].Value.ToString() == "True")
                chk_Priority.Checked = true;
            else
                chk_Priority.Checked = false;
        }
        private void Set_Plate_Image_Data()
        {

            if (Constants.Image_Backup_Location != "")
            {
                try
                {
                    img_Full = Image.FromFile(Constants.Image_Backup_Location + dgv_OtherHits.SelectedRows[0].Cells["Picture"].Value.ToString() + ".jpg");
                }
                catch
                {
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + dgv_OtherHits.SelectedRows[0].Cells["Picture"].Value.ToString() + ".jpg");
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                    img_Full = Image.FromStream(ms);

                    Save_Local_ALPR_Data(dgv_OtherHits.SelectedRows[0].Cells["Picture"].Value.ToString());
                }
            }
            else
            {
                WebClient wc = new WebClient();
                byte[] bytes = wc.DownloadData(Constants.str_WebServer + "/img/" + dgv_OtherHits.SelectedRows[0].Cells["Picture"].Value.ToString() + ".jpg");
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
                img_Full = Image.FromStream(ms);
            }         

            //Get values needed to crop License Plate
            int plate_x1 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_x1"].Value);
            int plate_x2 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_x2"].Value);
            int plate_x3 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_x3"].Value);
            int plate_x4 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_x4"].Value);
            int plate_y1 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_y1"].Value);
            int plate_y2 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_y2"].Value);
            int plate_y3 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_y3"].Value);
            int plate_y4 = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["plate_y4"].Value);
            int xMin = new int[] { plate_x1, plate_x2, plate_x3, plate_x4 }.Min();
            int yMin = new int[] { plate_y1, plate_y2, plate_y3, plate_y4 }.Min();
            int xMax = new int[] { plate_x1, plate_x2, plate_x3, plate_x4 }.Max();
            int yMax = new int[] { plate_y1, plate_y2, plate_y3, plate_y4 }.Max();


            //Get values needed to crop to Vehicle
            int vehicle_x = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["vehicle_region_x"].Value);
            int vehicle_y = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["vehicle_region_y"].Value);
            int vehicle_height = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["vehicle_region_height"].Value);
            int vehicle_width = Convert.ToInt32(dgv_OtherHits.SelectedRows[0].Cells["vehicle_region_width"].Value);

            //Try to crop images and save as seperate files
            try
            {
                img_License = CropImage(img_Full, xMin, yMin, xMax - xMin, yMax - yMin);
                img_Car = CropImage(img_Full, vehicle_x, vehicle_y, vehicle_width, vehicle_height);
            }
            catch { }
        }
        private void Set_Plate_Image_Active()
        {
            pb_Plate.Image = img_License;
            if (rb_CarImage_Full.Checked == true)
            {
                pb_Car.Image = img_Full;
            }
            else
            {
                pb_Car.Image = img_Car;
            }
        }
        private void FixPlate()
        {
            using (FixPlate myFixPlate = new FixPlate())
            {
                myFixPlate.SetPlate(lbl_CurrentPlate.Text, dgv_OtherHits.SelectedRows[0].Cells["pk"].Value.ToString());
                myFixPlate.ShowDialog();
            }
            Load_Plates();
        }
        private void Btn_DateRange_Today_Click(object sender, EventArgs e)
        {
            dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);

            dtp_Start.Value = DateTime.Now;
            dtp_End.Value = DateTime.Now;
        }
        private void Btn_DayBack_Click(object sender, EventArgs e)
        {
            if (dtp_Start.Value.Date > dtp_Start.MinDate)
            {
                dtp_Start.Value = dtp_Start.Value.AddDays(-1);
                dtp_End.Value = dtp_End.Value.AddDays(-1);
            }
            
        }
        private void Btn_DayForward_Click(object sender, EventArgs e)
        {
            if (dtp_Start.Value.Date.AddDays(1) < DateTime.Now)
            {
                dtp_Start.Value = dtp_Start.Value.AddDays(1);
                dtp_End.Value = dtp_End.Value.AddDays(1);
            }        
        }
        private void Btn_DateRange_All_Click(object sender, EventArgs e)
        {
            dtp_Start.Value = Convert.ToDateTime("6/1/2019");
            dtp_End.Value = DateTime.Now;
        }
        private void Btn_KnownPlateUpdate_Click(object sender, EventArgs e)
        {
            KnownPlateUpdate();
            Load_Plates();
            MessageBox.Show("Record Updated");
        }
        private void Btn_FixPlateEntry_Click(object sender, EventArgs e)
        {
            FixPlate();
        }
        private void Rb_CarImage_Full_CheckedChanged(object sender, EventArgs e)
        {
            Set_Plate_Image_Active();
        }
        private void Dgv_Plates_SelectionChanged(object sender, EventArgs e)
        {
            if ((dgv_Plates.SelectedRows.Count > 0) && (dgv_Plates.Focused))
            {
                Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());
            }
        }
        private void Dgv_OtherHits_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_OtherHits.SelectedRows.Count > 0)
            {
                try
                {
                    Set_Plate_Image_Data();
                    Set_Plate_Image_Active();
                }
                catch { }

                lbl_ALPR_Color.Text = dgv_OtherHits.SelectedRows[0].Cells["ALPR Color"].Value.ToString();
                lbl_ALPR_MM.Text = dgv_OtherHits.SelectedRows[0].Cells["ALPR Model"].Value.ToString();
            }   
        }
        private void Btn_HidePlateEntry_Click(object sender, EventArgs e)
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Insert Into LPR_PlateHits_ToHide (pk, reason, date_added) values (@pk, @reason, GetDate())", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@reason", "Duplicate");
                    sql_Command.Parameters.AddWithValue("@pk", dgv_OtherHits.SelectedRows[0].Cells["pk"].Value.ToString());
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }

            if (tc_Dashboard.SelectedTab == tp_Summary)
            {
                try
                {
                    Load_PlateSummary();
                    Load_PlateSummaryChart();
                    Load_PlateSummaryChart2();
                    dgv_OtherHits.Focus();
                }
                catch { }
            }
            else if (tc_Dashboard.SelectedTab == tp_Main)
            {
                try
                {
                    Set_Plate_Details(lbl_CurrentPlate.Text);
                    Load_Plates();                    
                    dgv_Plates.Focus();
                }
                catch { }          
            }           
        }
        private void Btn_StatusClear_Click(object sender, EventArgs e)
        {
            cb_PlateStatusSearch.Text = "";
        }
        private void Btn_CameraClear_Click(object sender, EventArgs e)
        {
            cb_CameraList.Text = "";
        }
        private void Btn_SearchClear_Click(object sender, EventArgs e)
        {
            txt_PlateSearch.Text = "";
        }
        private void Btn_Desc_Clear_Click(object sender, EventArgs e)
        {
            txt_Desc_Search.Text = "";
        }
        private void Btn_Clear_Color_Click(object sender, EventArgs e)
        {
            txt_Search_Color.Text = "";
        }
        private void Btn_Clear_Make_Click(object sender, EventArgs e)
        {
            txt_Search_Make.Text = "";
        }
        private void Btn_Clear_Model_Click(object sender, EventArgs e)
        {
            txt_Search_Model.Text = "";
        }
        private void Btn_Clear_VIN_Click(object sender, EventArgs e)
        {
            txt_Search_VIN.Text = "";
        }

        private void Btn_UpdateOtherHits_Click(object sender, EventArgs e)//Added 2020.10.15 to refresh "Other Hits"
        {
            Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());
        }

        private void Btn_PrintReport_Click(object sender, EventArgs e)
        {
            string ReportBy = Prompt.ShowDialog("Report Created By", "Who created this report?");
            string ReportCount = Prompt.ShowDialog("How Many", "How many entries would you like to see?");
            int ReportCountInt;

            if (int.TryParse(ReportCount, out ReportCountInt))
            {
            }
            else
            {
                ReportCountInt = 10;
            }

            EmailReport_CollectInfo(ReportBy, ReportCountInt);
            MessageBox.Show("Check your email");
        }
        private void EmailReport_CollectInfo(string ReportBy, int ReportCountInt) // .
        {
            DataTable PlateHistory;
            String str_PlateState = dgv_Plates.SelectedRows[0].Cells["Region"].Value.ToString();
            String str_DistinctDays = dgv_Plates.SelectedRows[0].Cells["Distinct Days"].Value.ToString();

            PlateHistory = new DataTable();
            PlateHistory.Columns.Add("LocalTime");

            if (ReportCountInt > dgv_OtherHits.Rows.Count)
            {
                ReportCountInt = dgv_OtherHits.Rows.Count;
            }

            for (int i = 0; i < ReportCountInt; i++)
            {
                DataRow dataRow2 = PlateHistory.NewRow();
                dataRow2["LocalTime"] = dgv_OtherHits.Rows[i].Cells["Local Time"].Value.ToString();
                PlateHistory.Rows.Add(dataRow2);
            }

            EmailReport(ReportBy, ReportCountInt, str_PlateState, str_DistinctDays, PlateHistory);
        }
        private void EmailReport(string ReportBy, int ReportCountInt, string str_PlateState, string str_DistinctDays, DataTable PlateHistory)
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress(Constants.emailSignIn);
            mail.To.Add(new MailAddress(Constants.emailDefaultTo));
            mail.IsBodyHtml = true;
            mail.Subject = "License Plate Report";
            string st = "<html><head><style>";
            st += "table, th, td {border: 1px solid black;}";
            st += "</style></head><body>";
            st += "<img src = \"$LOGOIMAGE$\" />";
            st += "<br /><br />";
            st += "This report exported by: " + ReportBy + " on " + DateTime.Now.Date.ToShortDateString() + ".";
            st += "<br />License Plate: <b>" + str_PlateState + ": " + lbl_CurrentPlate.Text + "</b>";
            st += "<br /><img src = \"$LPIMAGE$\" />";
            st += "<br /><br />Total Distinct Days Seen: <b>" + str_DistinctDays + "</b>";

            if (txt_VIN.Text != "" && txt_VIN.Text != "Error")
            {
                st += "<br />Car Year: " + txt_Year.Text;
                st += "<br />Car Make: " + txt_Make.Text;
                st += "<br />Car Model: " + txt_Model.Text;
                st += "<br />Car VIN: " + txt_VIN.Text;
            }

            st += "<br /><br />Best Car Image:<br />";
            st += "<img src = \"$BESTCARIMAGE$\" />";

            //Creates the table of times the car was seen.
            st += "<br /><br />The last " + ReportCountInt + " times the car was noticed:<br />";
            st += "<table>";
            st += "<tr style=\"background - color:#D3D3D3\"><th>Time</th></tr>";
            foreach (DataRow dataRow in PlateHistory.Rows)
            {
                st += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td></tr>";
            }
            st += "</table>";
            st += "</body></html>";

            //Update all the Image Placeholders to code that will allow the images to show up inline in the email
            string contentID1 = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$LOGOIMAGE$", "cid:" + contentID1);

            string contentIDBESTCAR = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$BESTCARIMAGE$", "cid:" + contentIDBESTCAR);

            string contentIDLP = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$LPIMAGE$", "cid:" + contentIDLP);

            //Add to Alternate View
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(st, null, "text/html");

            //Add the actual images
            LinkedResource imagelink1 = new LinkedResource(Constants.LogoLocation, "image/jpeg");
            imagelink1.ContentId = contentID1;
            imagelink1.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelink1);

            var imageStreamBestCar = GetImageStream(pb_Car.Image);
            LinkedResource imagelinkBestCar = new LinkedResource(imageStreamBestCar, "image/jpeg");
            imagelinkBestCar.ContentId = contentIDBESTCAR;
            imagelinkBestCar.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelinkBestCar);

            var imageStreamLP = GetImageStream(pb_Plate.Image);
            LinkedResource imagelinkLP = new LinkedResource(imageStreamLP, "image/jpeg");
            imagelinkLP.ContentId = contentIDLP;
            imagelinkLP.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelinkLP);

            mail.AlternateViews.Add(htmlView);
            mail.Body = st;

            using (SmtpClient smtp = new SmtpClient()) // Information for Gmail, change if you use another provider.
            {
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Constants.emailSignIn, Constants.emailPassword);
                smtp.Host = Constants.emailServer;
                smtp.Send(mail);
            }
            MessageBox.Show("Check your email");
        }

        private void Btn_DailyReport_Click(object sender, EventArgs e)
        {
            string ReportBy = Prompt.ShowDialog("Report Created By", "Who created this report?");
            EmailDailyReport_CollectInfo(ReportBy);
        }
        private void EmailDailyReport_CollectInfo(string ReportBy)
        {
            DataTable PlateList;
            String str_PlateState = dgv_Plates.SelectedRows[0].Cells["Region"].Value.ToString();
            String str_DistinctDays = dgv_Plates.SelectedRows[0].Cells["Distinct Days"].Value.ToString();

            PlateList = new DataTable();
            PlateList.Columns.Add("LocalTime");
            PlateList.Columns.Add("Plate");
            PlateList.Columns.Add("State");
            PlateList.Columns.Add("DistinctDays");
            PlateList.Columns.Add("Year");
            PlateList.Columns.Add("Make");
            PlateList.Columns.Add("Model");
            PlateList.Columns.Add("VIN");

            for (int i = 0; i < dgv_Plates.Rows.Count; i++)
            {
                DataRow dataRow = PlateList.NewRow();
                dataRow["LocalTime"] = dgv_Plates.Rows[i].Cells["Local Time"].Value.ToString();
                dataRow["Plate"] = dgv_Plates.Rows[i].Cells["Plate"].Value.ToString();
                dataRow["State"] = dgv_Plates.Rows[i].Cells["Region"].Value.ToString();
                dataRow["DistinctDays"] = dgv_Plates.Rows[i].Cells["Distinct Days"].Value.ToString();
                dataRow["Year"] = dgv_Plates.Rows[i].Cells["Yr"].Value.ToString();
                dataRow["Make"] = dgv_Plates.Rows[i].Cells["Car Make"].Value.ToString();
                dataRow["Model"] = dgv_Plates.Rows[i].Cells["Car Model"].Value.ToString();
                dataRow["VIN"] = dgv_Plates.Rows[i].Cells["VIN"].Value.ToString();
                PlateList.Rows.Add(dataRow);
            }
            EmailDailyReport(ReportBy, PlateList);
        }
        private void EmailDailyReport(string ReportBy, DataTable PlateList)
        {
            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress(Constants.emailSignIn);
            mail.To.Add(new MailAddress(Constants.emailDefaultTo));
            mail.IsBodyHtml = true;
            mail.Subject = "License Plate Daily Report";
            string st = "<html><head><style>";
            st += "table, th, td {border: 1px solid black;}";
            st += "</style></head><body>";
            st += "<img src = \"$LOGOIMAGE$\" width=\"400\"/>";
            st += "<br /><br />";
            st += "This report exported by: " + ReportBy + " on " + DateTime.Now.Date.ToShortDateString() + ".";

            string tbl1 = "<table>";
            tbl1 += "<tr style=\"background - color:#D3D3D3\"><th>Time</th><th>Plate</th><th>State</th><th>Distinct Days</th><th>VIN</th><th>Year</th><th>Make</th><th>Model</th></tr>";

            string tbl2 = "<table>";
            tbl2 += "<tr style=\"background - color:#D3D3D3\"><th>Time</th><th>Plate</th><th>State</th><th>Distinct Days</th><th>VIN</th><th>Year</th><th>Make</th><th>Model</th></tr>";

            foreach (DataRow dataRow in PlateList.Rows)
            {
                if (dataRow["DistinctDays"].ToString() == "1")
                {
                    tbl1 += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td><td>" + dataRow["Plate"].ToString() + "</td><td>" + dataRow["State"].ToString() + "</td><td>" + dataRow["DistinctDays"].ToString() + "</td><td>" + dataRow["VIN"].ToString() + "</td><td>" + dataRow["Year"].ToString() + "</td><td>" + dataRow["Make"].ToString() + "</td><td>" + dataRow["Model"].ToString() + "</td></tr>";
                }
                else
                {
                    tbl2 += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td><td>" + dataRow["Plate"].ToString() + "</td><td>" + dataRow["State"].ToString() + "</td><td>" + dataRow["DistinctDays"].ToString() + "</td><td>" + dataRow["VIN"].ToString() + "</td><td>" + dataRow["Year"].ToString() + "</td><td>" + dataRow["Make"].ToString() + "</td><td>" + dataRow["Model"].ToString() + "</td></tr>";
                }
            }
            tbl1 += "</table>";
            tbl2 += "</table>";

            st += "<br /><br />Plates first seen Today<br />";
            st += tbl1;

            st += "<br /><br />Other Plates<br />";
            st += tbl2;
            st += "</body></html>";

            //Update all the Image Placeholders to code that will allow the images to show up inline in the email
            string contentID1 = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$LOGOIMAGE$", "cid:" + contentID1);

            //Add to Alternate View
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(st, null, "text/html");

            //Add the actual images
            LinkedResource imagelink1 = new LinkedResource(Constants.LogoLocation, "image/jpeg");
            imagelink1.ContentId = contentID1;
            imagelink1.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            htmlView.LinkedResources.Add(imagelink1);

            mail.AlternateViews.Add(htmlView);
            mail.Body = st;

            using (SmtpClient smtp = new SmtpClient()) // Information for Gmail, change if you use another provider.
            {
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Constants.emailSignIn, Constants.emailPassword);
                smtp.Host = Constants.emailServer;
                smtp.Send(mail);
            }
        }

        #endregion


        #region Summary Tab
        private void Load_PlateSummary()
        {
            dataAdapter = new SqlDataAdapter("Exec sp_LPR_PlateSummary @StartDate, @EndDate, @CurrentOffset", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");

            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            dgv_PlateSummary.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_PlateSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv_PlateSummary.RowHeadersVisible = false;

            bs_PlateSummary.DataSource = table;
            dgv_PlateSummary.RowHeadersVisible = true;
            dgv_PlateSummary.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
        private void Load_PlateSummaryChart() // Code should support multiple series if query supplied multiple.
        {
            chart_PlateSummary.Series.Clear();

            dataAdapter = new SqlDataAdapter("Exec sp_LPR_PlateChart @StartDate, @EndDate, @CurrentOffset", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");

            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            chart_PlateSummary.DataSource = table;

            string currentStatus = "NoStatus";
            List<DateTime> xvals = new List<DateTime>();
            List<Int32> yvals = new List<Int32>();
            string seriesName;
            foreach (DataRow row2 in table.Rows)
            {
                if (row2["status"].ToString() != currentStatus)
                {
                    if (currentStatus != "NoStatus") // Save Prior Series
                    {
                        chart_PlateSummary.Series[currentStatus].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                        chart_PlateSummary.Series[currentStatus].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                        chart_PlateSummary.Series[currentStatus].Points.DataBindXY(xvals.ToArray(), yvals.ToArray());
                        chart_PlateSummary.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd htt";
                        chart_PlateSummary.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                        chart_PlateSummary.ChartAreas[0].AxisX.LabelStyle.Angle = 60;
                        chart_PlateSummary.ChartAreas[0].AxisX.Interval = 0.04167 * (((TimeSpan)(dtp_End.Value.Date - dtp_Start.Value.Date)).Days + 1);
                    }
                    
                    //Create a new Series
                    xvals = new List<DateTime>();
                    yvals = new List<Int32>();
                    seriesName = row2["Status"].ToString();
                    chart_PlateSummary.Series.Add(seriesName);
                    chart_PlateSummary.Series[seriesName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chart_PlateSummary.Series[seriesName].BorderWidth = 3;
                    currentStatus = row2["status"].ToString();
                }
                xvals.Add(Convert.ToDateTime(row2["Hour"].ToString()));
                yvals.Add(Convert.ToInt32(row2["Hits"].ToString()));
            }

            //Add the final Series
            try
            {
                chart_PlateSummary.Series[currentStatus].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chart_PlateSummary.Series[currentStatus].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                chart_PlateSummary.Series[currentStatus].Points.DataBindXY(xvals.ToArray(), yvals.ToArray());
                chart_PlateSummary.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd htt";
                chart_PlateSummary.ChartAreas[0].AxisX.IsLabelAutoFit = false;
                chart_PlateSummary.ChartAreas[0].AxisX.LabelStyle.Angle = 60;
                chart_PlateSummary.ChartAreas[0].AxisX.Interval = 0.04167 * (((TimeSpan)(dtp_End.Value.Date - dtp_Start.Value.Date)).Days + 1);

                //Bind!
                chart_PlateSummary.Legends[0].Enabled = false;
                chart_PlateSummary.ChartAreas[0].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 0, 100, 100);
                chart_PlateSummary.DataBind();
            }
            catch
            { }
           
            
        }
        private void Load_PlateSummaryChart2()
        {
            chart_PlateSummaryPie.Series[0].Points.Clear();
            chart_PlateSummaryPie.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

            dataAdapter = new SqlDataAdapter("Exec sp_LPR_PlatePie @StartDate, @EndDate, @CurrentOffset", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");

            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            foreach (DataRow row2 in table.Rows)
            {
                chart_PlateSummaryPie.Series[0].Points.AddXY(row2["Status"].ToString(), Convert.ToInt32(row2["Hits"].ToString()));
            }

            //Bind!
            chart_PlateSummaryPie.ChartAreas[0].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 0, 80, 100);
            chart_PlateSummaryPie.Series[0].Label = "#VALX: #VALY";
        }
        private void Dgv_PlateSummary_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_PlateSummary.SelectedRows.Count > 0)
            {
                Set_Plate_Details(dgv_PlateSummary.SelectedRows[0].Cells["Plate"].Value.ToString());
            }
        }              
        #endregion


        #region Helper Functions
        public static Bitmap CropImage(Image source, int x, int y, int width, int height) 
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
        } //Creates new (cropped) image from source image
        private static Stream GetImageStream(Image image) //Converts an image into a memory stream
        {
            var imageConverter = new ImageConverter();
            var imgaBytes = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            var memoryStream = new MemoryStream(imgaBytes);

            return memoryStream;
        }

        private void write_event(string myMessage, EventLogEntryType myType)
        {
            try
            {
                string source = ".NET Runtime";
                EventLog systemEventLog = new EventLog("Application");
                systemEventLog.Source = source;
                systemEventLog.WriteEntry("LPR Viewer: " + myMessage, myType, 1001);
            }
            catch { }
        }
        #endregion


        #region Forensic Functions
        private void LicensePlateData_Lookup(string AC_Plate, string AC_State)
        {     
            AC_State = AC_State.Replace("us-", "");

            //Default blank states to Default State
            if (AC_State == "")
            {
                AC_State = Constants.DefaultState;
            }
            
            //Try to get JSON Response (If it fails, try again with Default State)
            string AC_json = "";

            try
            {
                AC_json = (new WebClient()).DownloadString("https://licenseplatedata.com/consumer-api/" + Constants.LPD_API + "/" + AC_State + "/" + AC_Plate);

                if (AC_json.Contains("Sorry we could not find info on the license plate you entered."))
                {
                    AC_json = "";

                    if (AC_State.ToUpper() != Constants.DefaultState.ToUpper()) // Only try the default state if that isn't the one we already tried...
                    {
                        AC_json = (new WebClient()).DownloadString("https://licenseplatedata.com/consumer-api/" + Constants.LPD_API + "/" + Constants.DefaultState + "/" + AC_Plate);

                        if (AC_json.Contains("Sorry we could not find info on the license plate you entered."))
                        {
                            AC_json = "";
                        }
                    }
                }
            }
            catch
            {}

            if (AC_json != "") //If I got a response, parse and then save
            {
                LicensePlateData_Response currentPlate = new LicensePlateData_Response();

                // They don't have it wrapped in brackets...
                string AC_Json2 = "[" + AC_json + "]";

                var currentPlate_List = JsonSerializer.Deserialize<List<LicensePlateData_Response>>(AC_Json2);
                currentPlate = currentPlate_List.First();

                if (currentPlate.error == false)
                {
                    AutoCheck_DBUpdate(AC_Plate, currentPlate.licensePlateLookup.vin.EmptyIfNull(), currentPlate.licensePlateLookup.year.EmptyIfNull(), currentPlate.licensePlateLookup.make.EmptyIfNull(), currentPlate.licensePlateLookup.model.EmptyIfNull(), "", currentPlate.licensePlateLookup.style.EmptyIfNull(), currentPlate.licensePlateLookup.engine.EmptyIfNull(), "", currentPlate.licensePlateLookup.color.name.EmptyIfNull());
                }
                else
                {
                    AutoCheck_DBUpdate(AC_Plate, "Error", "", "", "", "", "", "", "Error", "");
                }
            }
            else //If I didn't get a response, still need to save something so I don't ever try it again automatically
            {
                AutoCheck_DBUpdate(AC_Plate, "Error", "", "", "", "", "", "", "Error", "");
            }
        }
        private void AutoCheck_DBUpdate(string AC_Plate, string vin, string year, string make, string model, string body, string vehicleClass, string engine, string status, string color)
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                //Delete existing entry (e.g. if doing a manual refresh) (Unless new entry is error, then don't delete old)
                if (vin != "Error")
                {
                    using (sql_Command = new SqlCommand("Delete From LPR_AutoCheck Where Plate = @Plate", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@Plate", AC_Plate);
                        sql_Connection.Open();
                        sql_Command.ExecuteNonQuery();
                        sql_Connection.Close();
                    }
                }

                if (vin == "Error")
                {
                    using (sql_Command = new SqlCommand("Delete From LPR_AutoCheck Where Plate = @Plate AND VIN = 'error' AND (Date_Imported is NULL OR Date_Imported < DateAdd(month, -1, GetDate()))", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@Plate", AC_Plate);
                        sql_Connection.Open();
                        sql_Command.ExecuteNonQuery();
                        sql_Connection.Close();
                    }
                }

                //Add new entry
                using (sql_Command = new SqlCommand("Insert Into LPR_AutoCheck (Plate, vin, year, make, model, body, vehicleClass, engine, status, color, date_imported) VALUES (@Plate, @vin, @year, @make, @model, @body, @vehicleClass, @engine, @status, @color, GetDate())", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Plate", AC_Plate);
                    sql_Command.Parameters.AddWithValue("@vin", vin);
                    sql_Command.Parameters.AddWithValue("@year", year);
                    sql_Command.Parameters.AddWithValue("@make", make);
                    sql_Command.Parameters.AddWithValue("@model", model);
                    sql_Command.Parameters.AddWithValue("@body", body);
                    sql_Command.Parameters.AddWithValue("@vehicleClass", vehicleClass);
                    sql_Command.Parameters.AddWithValue("@engine", engine);
                    sql_Command.Parameters.AddWithValue("@status", status);
                    sql_Command.Parameters.AddWithValue("@color", color);
                    sql_Connection.Open();
                    try
                    {
                        sql_Command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        write_event(e.Message.ToString(), EventLogEntryType.Error);

                        try
                        {
                            sql_Command.CommandText = "Update LPR_AutoCheck Set date_imported = GetDate() Where Plate = @Plate";
                            sql_Command.ExecuteNonQuery();
                        }
                        catch (Exception e2)
                        {
                            write_event(e2.Message.ToString(), EventLogEntryType.Error);
                        }
                    }
                    sql_Connection.Close();
                }
            }
        }
        private void Btn_AutoCheck_Click(object sender, EventArgs e)
        {
            string AC_Plate = lbl_CurrentPlate.Text;
            string AC_State = dgv_OtherHits.SelectedRows[0].Cells["Region"].Value.ToString();
            LicensePlateData_Lookup(AC_Plate, AC_State);

            Set_Plate_Details(AC_Plate);
        }
        private void Btn_Forensic_Manual_Update_Click(object sender, EventArgs e)
        {
            string AC_Plate = lbl_CurrentPlate.Text;

            if (btn_Forensic_Manual_Update.Text == "Edit")
            {
                txt_VIN.Enabled = true;
                txt_Year.Enabled = true;
                txt_Make.Enabled = true;
                txt_Model.Enabled = true;
                txt_Color.Enabled = true;

                btn_Forensic_Manual_Update.Text = "Save";
            }
            else if (btn_Forensic_Manual_Update.Text == "Save")
            {
                txt_VIN.Enabled = false;
                txt_Year.Enabled = false;
                txt_Make.Enabled = false;
                txt_Model.Enabled = false;
                txt_Color.Enabled = false;

                btn_Forensic_Manual_Update.Text = "Edit";

                if (txt_VIN.Text == "Error")
                {
                    txt_VIN.Text = "Manual";
                }

                AutoCheck_DBUpdate(AC_Plate, txt_VIN.Text, txt_Year.Text, txt_Make.Text, txt_Model.Text, "", "", "", "Manual", txt_Color.Text);
                Set_Plate_Details(AC_Plate);
            }
        }

        #endregion

        #region Forensice Functions Old
        private void AutoCheck_Lookup(string AC_Plate, string AC_State)
        {
            AC_State = AC_State.Replace("us-", "");

            //Default blank states to Default State
            if (AC_State == "")
            {
                AC_State = Constants.DefaultState;
            }

            //Try to get JSON Response (If it fails, try again with Default State)
            string AC_json = "";
            try
            {
                AC_json = (new WebClient()).DownloadString("https://www.autocheck.com/consumer-api/meta/v1/summary/plate/" + AC_Plate + "/state/" + AC_State);
            }
            catch
            {
                try
                {
                    AC_json = (new WebClient()).DownloadString("https://www.autocheck.com/consumer-api/meta/v1/summary/plate/" + AC_Plate + "/state/" + Constants.DefaultState);
                }
                catch { }
            }

            if (AC_json != "") //If I got a response, parse and then save
            {
                AutoCheck_Plate currentPlate = new AutoCheck_Plate();
                var currentPlate_List = JsonSerializer.Deserialize<List<AutoCheck_Plate>>(AC_json);
                currentPlate = currentPlate_List.First();

                AutoCheck_DBUpdate(AC_Plate, currentPlate.vin.EmptyIfNull(), currentPlate.year.EmptyIfNull(), currentPlate.make.EmptyIfNull(), currentPlate.model.EmptyIfNull(), currentPlate.body.EmptyIfNull(), currentPlate.vehicleClass.EmptyIfNull(), currentPlate.engine.EmptyIfNull(), currentPlate.status.EmptyIfNull(), "");
            }
            else //If I didn't get a response, still need to save something so I don't ever try it again automatically
            {
                AutoCheck_DBUpdate(AC_Plate, "Error", "", "", "", "", "", "", "Error", "");
            }
        }
        private void LPL_IO_Lookup(string AC_Plate, string AC_State)
        {
            AC_State = AC_State.Replace("us-", "");

            //Default blank states to Default State
            if (AC_State == "")
            {
                AC_State = Constants.DefaultState;
            }

            //Try to get JSON Response (If it fails, try again with Default State)
            string AC_json = "";
            string AC_json_Split_Start = "<script id=\"__NEXT_DATA__\" type=\"application/json\">";
            string AC_json_Split_End = "</script>";

            try
            {
                AC_json = (new WebClient()).DownloadString("https://licenseplatelookup.io/lookup/" + AC_Plate + ":" + AC_State);
                AC_json = AC_json.After(AC_json_Split_Start);
                AC_json = AC_json.Before(AC_json_Split_End);

                if (AC_json.Contains("invalid_request"))
                {
                    AC_json = "";
                    AC_json = (new WebClient()).DownloadString("https://licenseplatelookup.io/lookup/" + AC_Plate + ":" + Constants.DefaultState);
                    AC_json = AC_json.After(AC_json_Split_Start);
                    AC_json = AC_json.Before(AC_json_Split_End);

                    if (AC_json.Contains("invalid_request"))
                    {
                        AC_json = "";
                    }
                }
            }
            catch
            { }

            if (AC_json != "") //If I got a response, parse and then save
            {
                LPL_io_Root currentPlate = new LPL_io_Root();

                // They don't have it wrapped in brackets...
                AC_json = "[" + AC_json + "]";

                var currentPlate_List = JsonSerializer.Deserialize<List<LPL_io_Root>>(AC_json);
                currentPlate = currentPlate_List.First();

                AutoCheck_DBUpdate(AC_Plate, currentPlate.props.initialState.data.VIN.EmptyIfNull(), currentPlate.props.initialState.data.basic.year.EmptyIfNull(), currentPlate.props.initialState.data.basic.make.EmptyIfNull(), currentPlate.props.initialState.data.basic.model.EmptyIfNull(), currentPlate.props.initialState.data.basic.body_class.EmptyIfNull(), currentPlate.props.initialState.data.basic.body_class.EmptyIfNull(), "", "", "");
            }
            else //If I didn't get a response, still need to save something so I don't ever try it again automatically
            {
                AutoCheck_DBUpdate(AC_Plate, "Error", "", "", "", "", "", "", "Error", "");
            }
        }



        #endregion
    }

    public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 500,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen
                };
                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
                Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }


    public class LicensePlateData_Plate
    {
        public string vin { get; set; }
        public string name { get; set; }
        public string engine { get; set; }
        public string style { get; set; }
        public string year { get; set; }
        public string make { get; set; }
        public string model { get; set; } 
        public LicensePlateData_Response_Color color { get; set; }
    }
    public class LicensePlateData_Response
    {
        public bool error { get; set; }
        public string query_time { get; set; }
        public int code { get; set; }
        public string message { get; set; }
        public string requestIP { get; set; }
        public LicensePlateData_Plate licensePlateLookup { get; set; }
        public bool cache { get; set; }
    }
    public class LicensePlateData_Response_Color
    {
        public string name { get; set; }
        public string abbreviation { get; set; }
    }

    #region "ALPR Local Stuff"
    public class ALPR_Candidate
    {
        public string plate { get; set; }
        public double confidence { get; set; }
        public int matches_template { get; set; }
    }

    public class ALPR_Coordinate
    {
        public int x { get; set; }
        public int y { get; set; }
    }

    public class ALPR_VehicleRegion
    {
        public int x { get; set; }
        public int y { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class ALPR_BestPlate
    {
        public string plate { get; set; }
        public double confidence { get; set; }
        public int matches_template { get; set; }
        public int plate_index { get; set; }
        public string region { get; set; }
        public int region_confidence { get; set; }
        public double processing_time_ms { get; set; }
        public int requested_topn { get; set; }
        public List<ALPR_Coordinate> coordinates { get; set; }
        public string plate_crop_jpeg { get; set; }
        public ALPR_VehicleRegion vehicle_region { get; set; }
        public List<ALPR_Candidate> candidates { get; set; }
    }

    public class ALPR_Color
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_Make
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_MakeModel
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_BodyType
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_Year
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_Orientation
    {
        public string name { get; set; }
        public double confidence { get; set; }
    }

    public class ALPR_Vehicle
    {
        public List<ALPR_Color> color { get; set; }
        public List<ALPR_Make> make { get; set; }
        public List<ALPR_MakeModel> make_model { get; set; }
        public List<ALPR_BodyType> body_type { get; set; }
        public List<ALPR_Year> year { get; set; }
        public List<ALPR_Orientation> orientation { get; set; }
    }

    public class ALPR_Root
    {
        public string data_type { get; set; }
        public int version { get; set; }
        public long epoch_start { get; set; }
        public long epoch_end { get; set; }
        public int frame_start { get; set; }
        public int frame_end { get; set; }
        public string company_id { get; set; }
        public string agent_uid { get; set; }
        public string agent_version { get; set; }
        public string agent_type { get; set; }
        public int camera_id { get; set; }
        public string country { get; set; }
        public List<string> uuids { get; set; }
        public List<int> plate_indexes { get; set; }
        public List<ALPR_Candidate> candidates { get; set; }
        public string vehicle_crop_jpeg { get; set; }
        public ALPR_BestPlate best_plate { get; set; }
        public double best_confidence { get; set; }
        public string best_uuid { get; set; }
        public string best_plate_number { get; set; }
        public string best_region { get; set; }
        public double best_region_confidence { get; set; }
        public bool matches_template { get; set; }
        public int best_image_width { get; set; }
        public int best_image_height { get; set; }
        public double travel_direction { get; set; }
        public bool is_parked { get; set; }
        public bool is_preview { get; set; }
        public ALPR_Vehicle vehicle { get; set; }
    }


    #endregion


    #region Old Forensic Classes
    public class AutoCheck_Plate
    {
        public string code { get; set; }
        public string vin { get; set; }
        public string year { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string countryOfAssembly { get; set; }
        public string body { get; set; }
        public string vehicleClass { get; set; }
        public int recordCount { get; set; }
        public int scoreRangeLow { get; set; }
        public int scoreRangeHigh { get; set; }
        public string buybackAssurance { get; set; }
        public bool lemonRecord { get; set; }
        public bool accidentRecord { get; set; }
        public bool floodRecord { get; set; }
        public bool singleOwner { get; set; }
        public string engine { get; set; }
        public string status { get; set; }
    }

    public class LPL_io_Plate
    {
        public string number { get; set; }
        public string state { get; set; }
    }
    public class LPL_io_Basic
    {
        public string body_class { get; set; }
        public int doors { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string series { get; set; }
        public string type { get; set; }
        public string year { get; set; }
    }
    public class LPL_io_NCSA
    {
        public string NCSABodyType { get; set; }
        public string NCSAMake { get; set; }
        public string NCSAModel { get; set; }
    }
    public class LPL_io_Engine
    {
        public string configuration { get; set; }
        public string cycles { get; set; }
        public string cylinders { get; set; }
        public string hp { get; set; }
        public string hp_to { get; set; }
        public string kw { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string displacementCC { get; set; }
        public string displacementCI { get; set; }
        public string displacementL { get; set; }
    }
    public class LPL_io_Fuel
    {
        public string injectionType { get; set; }
        public string primaryType { get; set; }
        public string secondaryType { get; set; }
    }
    public class LPL_io_Manufacturer
    {
        public string name { get; set; }
        public string id { get; set; }
        public string type { get; set; }
    }
    public class LPL_io_Plant
    {
        public string city { get; set; }
        public string companyName { get; set; }
        public string country { get; set; }
        public string state { get; set; }
    }
    public class LPL_io_Premium
    {
    }
    public class LPL_io_Data
    {
        public LPL_io_Plate plate { get; set; }
        public LPL_io_Basic basic { get; set; }
        public LPL_io_NCSA NCSA { get; set; }
        public LPL_io_Engine engine { get; set; }
        public LPL_io_Fuel fuel { get; set; }
        public LPL_io_Manufacturer manufacturer { get; set; }
        public LPL_io_Plant plant { get; set; }
        public string _id { get; set; }
        public string VIN { get; set; }
        public LPL_io_Premium premium { get; set; }
    }
    public class LPL_io_InitialState
    {
        public LPL_io_Data data { get; set; }
    }
    public class LPL_io_PageProps
    {
        public string number { get; set; }
        public string state { get; set; }
    }
    public class LPL_io_InitialProps
    {
        public LPL_io_PageProps pageProps { get; set; }
    }
    public class LPL_io_Props
    {
        public bool isServer { get; set; }
        public LPL_io_InitialState initialState { get; set; }
        public LPL_io_InitialProps initialProps { get; set; }
    }
    public class LPL_io_Query
    {
        public string plate { get; set; }
    }
    public class LPL_io_Root
    {
        public LPL_io_Props props { get; set; }
        public string page { get; set; }
        public LPL_io_Query query { get; set; }
        public string buildId { get; set; }
        public bool isFallback { get; set; }
    }
    #endregion




    public static class Extensions
    {
        public static string EmptyIfNull(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }
    }

    static class SubstringExtensions
    {
        /// <summary>
        /// Get string value between [first] a and [last] b.
        /// </summary>
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            return value.Substring(adjustedPosA);
        }
    }
}
