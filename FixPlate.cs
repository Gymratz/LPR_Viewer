using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Forms;

namespace LPR
{
    public partial class FixPlate : Form
    {
        private SqlCommand sql_Command = new SqlCommand();
        private SqlConnection sql_Connection = new SqlConnection();
        public FixPlate()
        {
            InitializeComponent();
        }

        private void FixPlate_Load(object sender, EventArgs e)
        {

        }

        public void SetPlate(string plateValue, string pk)
        {
            lbl_CurrentPlate.Text = plateValue;
            txt_NewValue.Text = plateValue;
            lbl_PK.Text = pk;
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_NewValue.Text == "")
                {
                    MessageBox.Show("You need to enter a value.");
                    return;
                }

                using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
                {
                    using (sql_Command = new SqlCommand("Update LPR_PlateHits Set best_plate = @Plate Where pk = @pk", sql_Connection))
                    {
                        sql_Command.Parameters.AddWithValue("@Plate", txt_NewValue.Text);
                        sql_Command.Parameters.AddWithValue("@pk", lbl_PK.Text);
                        sql_Connection.Open();
                        sql_Command.ExecuteNonQuery();
                        sql_Connection.Close();
                    }

                    if (chk_AutoUpdate.Checked == true)
                    {
                        using (sql_Command = new SqlCommand("Delete From LPR_PlateCorrections Where wrongPlate = @wrongPlate", sql_Connection))
                        {
                            sql_Command.Parameters.AddWithValue("@wrongPlate", lbl_CurrentPlate.Text);
                            sql_Connection.Open();
                            sql_Command.ExecuteNonQuery();
                            sql_Connection.Close();
                        }

                        using (sql_Command = new SqlCommand("Insert Into LPR_PlateCorrections (wrongPlate, rightPlate) VALUES (@wrongPlate, @rightPlate)", sql_Connection))
                        {
                            sql_Command.Parameters.AddWithValue("@wrongPlate", lbl_CurrentPlate.Text);
                            sql_Command.Parameters.AddWithValue("@rightPlate", txt_NewValue.Text);
                            sql_Connection.Open();
                            sql_Command.ExecuteNonQuery();
                            sql_Connection.Close();
                        }
                        CleanUpDB();
                    }
                }
            }
            catch (Exception e2)
            {
                write_event(e2.Message.ToString(), EventLogEntryType.Error);
            }
            DialogResult = DialogResult.OK;
        }


        private void Btn_HideAll_Click(object sender, EventArgs e)
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Insert Into LPR_AutoHidePlates (Plate) VALUES (@Plate)", sql_Connection))
                {
                    sql_Command.Parameters.AddWithValue("@Plate", lbl_CurrentPlate.Text);
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }

            CleanUpDB();

            DialogResult = DialogResult.OK;
        }

        private void CleanUpDB()
        {
            using (sql_Connection = new SqlConnection(Constants.str_SqlCon))
            {
                using (sql_Command = new SqlCommand("Exec sp_LPR_FixPlates", sql_Connection))
                {
                    sql_Connection.Open();
                    sql_Command.ExecuteNonQuery();
                    sql_Connection.Close();
                }
            }
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
    }
}
