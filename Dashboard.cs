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

        public Dashboard()
        {
            InitializeComponent();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            Settings_Populate();

            //Set Date Ranges
            dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            dtp_Start.Value = DateTime.Now.AddDays(-1);
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

            Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());

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

            dgv_OtherHits.Columns["Hits Day"].Visible = false;
            dgv_OtherHits.Columns["Hits Week"].Visible = false;
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
            dgv_OtherHits.Columns["Distinct Days"].Visible = false;
            dgv_OtherHits.Columns["Alert_Address"].Visible = false;


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

                            //lbl_Total.Text = String.Format("{0:#,##0}", Convert.ToInt32(db_reader["Displayed_Total"].ToString())) + " / " + String.Format("{0:#,##0}", Convert.ToInt32(db_reader["All_Total"].ToString()));
                            //lbl_Unique.Text = String.Format("{0:#,##0}", Convert.ToInt32(db_reader["Displayed_Distinct"].ToString())) + " / " + String.Format("{0:#,##0}", Convert.ToInt32(db_reader["All_Distinct"].ToString()));
                        }
                    }
                }
            }
        }
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            if (tc_Dashboard.SelectedTab == tp_Main)
            {
                Load_Plates();
                dgv_Plates.Focus();
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
        private void Btn_SearchClear_Click(object sender, EventArgs e)
        {
            txt_PlateSearch.Text = "";
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

            config.AppSettings.Settings["LogoLocation"].Value = txt_LogoLocation.Text;

            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");

            Settings_Populate();
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
            //cb_PlateStatusSearch.Items.Insert(0, "%");
            //cb_PlateStatusSearch.SelectedItem = "%";

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

            dataAdapter = new SqlDataAdapter("Exec sp_LPR_AllPlates @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH, @Status, @Camera", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", dtp_Start.Value.Date);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", dtp_End.Value.Date.ToShortDateString() + " 23:59:59");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Plate", str_PlateSearch);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@HideNeighbors", chk_HideNeighbors.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@IdentifyDupes", chk_IdentifyDups.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@TopPH", 9999999);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Status", str_PlateStatusSearch);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Camera", str_CameraList);
            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            dgv_Plates.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_Plates.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv_Plates.RowHeadersVisible = false;

            bs_Plates.DataSource = table;

            dgv_Plates.RowHeadersVisible = true;
            dgv_Plates.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
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

            using (SqlConnection db_connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (SqlCommand db_command = new SqlCommand("Exec sp_LPR_GetDBStats @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH, @Status, @Camera", db_connection))
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

                using (sql_Command = new SqlCommand("Insert Into LPR_KnownPlates (Plate, Description, Status, Alert_Address) VALUES (@Plate, @Description, @Status, @AlertAddress)", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Plate", lbl_CurrentPlate.Text);
                    sql_Command.Parameters.AddWithValue("@Description", txt_PlateDescription.Text);
                    sql_Command.Parameters.AddWithValue("@Status", cb_PlateStatus.Text);
                    sql_Command.Parameters.AddWithValue("@AlertAddress", txt_PlateAlert.Text);
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
            dataAdapter = new SqlDataAdapter("Exec sp_LPR_AllPlates @StartDate, @EndDate, @Plate, @HideNeighbors, @CurrentOffset, @IdentifyDupes, @TopPH", Constants.str_SqlCon);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@StartDate", Convert.ToDateTime("6/1/2019"));
            dataAdapter.SelectCommand.Parameters.AddWithValue("@EndDate", DateTime.Now.AddDays(1));
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Plate", str_PlateSearch);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@HideNeighbors", chk_HideNeighbors.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@CurrentOffset", Constants.str_UTC_Offset + ":00");
            dataAdapter.SelectCommand.Parameters.AddWithValue("@IdentifyDupes", chk_IdentifyDups.Checked);
            dataAdapter.SelectCommand.Parameters.AddWithValue("@TopPH", txt_TopPH.Text);

            DataTable table = new DataTable
            {
                Locale = CultureInfo.InvariantCulture
            };
            dataAdapter.Fill(table);

            // Disabling resizing and hiding headers while loading to improve speed, enable again at end.
            dgv_OtherHits.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dgv_OtherHits.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            dgv_OtherHits.RowHeadersVisible = false;

            bs_OtherHits.DataSource = table;

            dgv_OtherHits.RowHeadersVisible = true;
            dgv_OtherHits.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);                                           

            if (tc_Dashboard.SelectedTab == tp_Main)
            {
                //Set single plate grid to same record as main grid
                String searchValue = dgv_Plates.SelectedRows[0].Cells["pk"].Value.ToString();
                int rowIndex = -1;
                foreach (DataGridViewRow row in dgv_OtherHits.Rows)
                {
                    if (row.Cells["pk"].Value.ToString().Equals(searchValue))
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
        }
        private void Set_Plate_Image_Data()
        {
            //Gets Image and saves the full image
            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData("http://127.0.0.1:8355/img/" + dgv_OtherHits.SelectedRows[0].Cells["Picture"].Value.ToString() + ".jpg");
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            img_Full = Image.FromStream(ms);

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

            //If Vehicle crop wouldn't fill, expand Height/Width
            if (vehicle_height < 561)
            {
                vehicle_height = 561;
            }
            
            if (vehicle_width < 997)
            {
                vehicle_width = 997;
            }

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
                Set_Plate_Image_Data();
                Set_Plate_Image_Active();
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
        #endregion
        public static Bitmap CropImage(Image source, int x, int y, int width, int height) 
        {
            Rectangle crop = new Rectangle(x, y, width, height);

            var bmp = new Bitmap(crop.Width, crop.Height);
            using (var gr = Graphics.FromImage(bmp))
            {
                gr.DrawImage(source, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
            }
            return bmp;
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

        private void Dgv_PlateSummary_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_PlateSummary.SelectedRows.Count > 0)
            {
                Set_Plate_Details(dgv_PlateSummary.SelectedRows[0].Cells["Plate"].Value.ToString());
            }
        }

        private void Dtp_Start_Enter(object sender, EventArgs e)
        {
            dtp_Start.MaxDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
        }

        //Added 2020.10.15 to refresh "Other Hits"
        private void Btn_UpdateOtherHits_Click(object sender, EventArgs e)
        {
            Set_Plate_Details(dgv_Plates.SelectedRows[0].Cells["Plate"].Value.ToString());
        }

        
        private void EmailReport_CollectInfo(string ReportBy, int ReportCountInt)
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
            st += "<img src = \"$LOGOIMAGE$\" width=\"400\"/>";
            st += "<br /><br />";
            st += "This report exported by: " + ReportBy + " on " + DateTime.Now.Date.ToShortDateString() + ".";
            st += "<br />License Plate: <b>" + lbl_CurrentPlate.Text + "</b>";
            st += "<br />License Plate State: <b>" + str_PlateState + "</b>";
            st += "<br />Total Distinct Days Seen: <b>" + str_DistinctDays + "</b>";


            st += "<br /><br />Best Car Image:<br />";
            st += "<img src = \"$BESTCARIMAGE$\"< />";

            //Creates the table of times the car was seen.
            st += "<br /><br />The last " + ReportCountInt + " times the car was noticed:<br />";
            st += "<table>";
            //st += "<tr style=\"background - color:#D3D3D3\"><th>Time</th><th>Plate</th><th>State</th></tr>";
            st += "<tr style=\"background - color:#D3D3D3\"><th>Time</th></tr>";
            foreach (DataRow dataRow in PlateHistory.Rows)
            {
                //st += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td><td>" + dataRow["Plate"].ToString() + "</td><td>" + dataRow["State"].ToString() + "</td></tr>";
                st += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td></tr>";
            }
            st += "</table>";
            st += "</body></html>";

            //Update all the Image Placeholders to code that will allow the images to show up inline in the email
            string contentID1 = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$LOGOIMAGE$", "cid:" + contentID1);
            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(st, null, "text/html");

            string contentIDBESTCAR = Guid.NewGuid().ToString().Replace("-", "");
            st = st.Replace("$BESTCARIMAGE$", "cid:" + contentIDBESTCAR);

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
        }

        //Converts an image into a memory stream
        private static Stream GetImageStream(Image image)
        {
            var imageConverter = new ImageConverter();
            var imgaBytes = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
            var memoryStream = new MemoryStream(imgaBytes);

            return memoryStream;
        }

        //Ad-hoc prompting for values.
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

            for (int i = 0; i < dgv_Plates.Rows.Count; i++)
            {
                DataRow dataRow = PlateList.NewRow();
                dataRow["LocalTime"] = dgv_Plates.Rows[i].Cells["Local Time"].Value.ToString();
                dataRow["Plate"] = dgv_Plates.Rows[i].Cells["Plate"].Value.ToString();
                dataRow["State"] = dgv_Plates.Rows[i].Cells["Region"].Value.ToString();
                dataRow["DistinctDays"] = dgv_Plates.Rows[i].Cells["Distinct Days"].Value.ToString();
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
            tbl1 += "<tr style=\"background - color:#D3D3D3\"><th>Time</th><th>Plate</th><th>State</th><th>Distinct Days</th></tr>";

            string tbl2 = "<table>";
            tbl2 += "<tr style=\"background - color:#D3D3D3\"><th>Time</th><th>Plate</th><th>State</th><th>Distinct Days</th></tr>";

            foreach (DataRow dataRow in PlateList.Rows)
            {
                if (dataRow["DistinctDays"].ToString() == "1")
                {
                    tbl1 += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td><td>" + dataRow["Plate"].ToString() + "</td><td>" + dataRow["State"].ToString() + "</td><td>" + dataRow["DistinctDays"].ToString() + "</td></tr>";
                }
                else
                {
                    tbl2 += "<tr><td>" + dataRow["LocalTime"].ToString() + "</td><td>" + dataRow["Plate"].ToString() + "</td><td>" + dataRow["State"].ToString() + "</td><td>" + dataRow["DistinctDays"].ToString() + "</td></tr>";
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
            MessageBox.Show("Check your email");
        }

        private void Btn_StatusClear_Click(object sender, EventArgs e)
        {
            cb_PlateStatusSearch.Text = "";
        }

        private void Btn_CameraClear_Click(object sender, EventArgs e)
        {
            cb_CameraList.Text = "";
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
    }
}
