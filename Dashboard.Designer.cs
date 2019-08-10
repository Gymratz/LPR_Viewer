namespace LPR
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.dgv_Plates = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_DayForward = new System.Windows.Forms.Button();
            this.btn_DayBack = new System.Windows.Forms.Button();
            this.btn_DateRange_All = new System.Windows.Forms.Button();
            this.btn_DateRange_Today = new System.Windows.Forms.Button();
            this.dtp_End = new System.Windows.Forms.DateTimePicker();
            this.dtp_Start = new System.Windows.Forms.DateTimePicker();
            this.pb_Plate = new System.Windows.Forms.PictureBox();
            this.tc_Dashboard = new System.Windows.Forms.TabControl();
            this.tp_Main = new System.Windows.Forms.TabPage();
            this.lbl_Unique = new System.Windows.Forms.Label();
            this.lbl_Total = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chk_IdentifyDups = new System.Windows.Forms.CheckBox();
            this.btn_SearchClear = new System.Windows.Forms.Button();
            this.chk_HideNeighbors = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_PlateSearch = new System.Windows.Forms.TextBox();
            this.gb_SpecificPlate = new System.Windows.Forms.GroupBox();
            this.btn_HidePlateEntry_Dup = new System.Windows.Forms.Button();
            this.txt_PlateAlert = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_FixPlateEntry = new System.Windows.Forms.Button();
            this.dgv_OtherHits = new System.Windows.Forms.DataGridView();
            this.btn_KnownPlateUpdate = new System.Windows.Forms.Button();
            this.txt_PlateDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cb_PlateStatus = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_CurrentPlate = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rb_CarImage_Car = new System.Windows.Forms.RadioButton();
            this.rb_CarImage_Full = new System.Windows.Forms.RadioButton();
            this.pb_Car = new System.Windows.Forms.PictureBox();
            this.tp_Summary = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chart_PlateSummary = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_PlateSummaryPie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgv_PlateSummary = new System.Windows.Forms.DataGridView();
            this.tp_Settings = new System.Windows.Forms.TabPage();
            this.txt_PlateStatusOptions = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_programName = new System.Windows.Forms.TextBox();
            this.btn_SettingsSave = new System.Windows.Forms.Button();
            this.txt_SqlCon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Plates)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Plate)).BeginInit();
            this.tc_Dashboard.SuspendLayout();
            this.tp_Main.SuspendLayout();
            this.gb_SpecificPlate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OtherHits)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Car)).BeginInit();
            this.tp_Summary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_PlateSummary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_PlateSummaryPie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PlateSummary)).BeginInit();
            this.tp_Settings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Refresh.Location = new System.Drawing.Point(7, 144);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(523, 47);
            this.btn_Refresh.TabIndex = 13;
            this.btn_Refresh.Text = "Search License Plate Data";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.Btn_Refresh_Click);
            // 
            // dgv_Plates
            // 
            this.dgv_Plates.AllowUserToAddRows = false;
            this.dgv_Plates.AllowUserToDeleteRows = false;
            this.dgv_Plates.AllowUserToResizeRows = false;
            this.dgv_Plates.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Plates.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgv_Plates.Location = new System.Drawing.Point(3, 573);
            this.dgv_Plates.MultiSelect = false;
            this.dgv_Plates.Name = "dgv_Plates";
            this.dgv_Plates.ReadOnly = true;
            this.dgv_Plates.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Plates.Size = new System.Drawing.Size(1533, 359);
            this.dgv_Plates.TabIndex = 14;
            this.dgv_Plates.SelectionChanged += new System.EventHandler(this.Dgv_Plates_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_DayForward);
            this.groupBox2.Controls.Add(this.btn_DayBack);
            this.groupBox2.Controls.Add(this.btn_DateRange_All);
            this.groupBox2.Controls.Add(this.btn_DateRange_Today);
            this.groupBox2.Controls.Add(this.dtp_End);
            this.groupBox2.Controls.Add(this.dtp_Start);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(105, 132);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "View Range";
            // 
            // btn_DayForward
            // 
            this.btn_DayForward.Location = new System.Drawing.Point(80, 72);
            this.btn_DayForward.Name = "btn_DayForward";
            this.btn_DayForward.Size = new System.Drawing.Size(16, 23);
            this.btn_DayForward.TabIndex = 6;
            this.btn_DayForward.Text = ">";
            this.btn_DayForward.UseVisualStyleBackColor = true;
            this.btn_DayForward.Click += new System.EventHandler(this.Btn_DayForward_Click);
            // 
            // btn_DayBack
            // 
            this.btn_DayBack.Location = new System.Drawing.Point(7, 72);
            this.btn_DayBack.Name = "btn_DayBack";
            this.btn_DayBack.Size = new System.Drawing.Size(16, 23);
            this.btn_DayBack.TabIndex = 5;
            this.btn_DayBack.Text = "<";
            this.btn_DayBack.UseVisualStyleBackColor = true;
            this.btn_DayBack.Click += new System.EventHandler(this.Btn_DayBack_Click);
            // 
            // btn_DateRange_All
            // 
            this.btn_DateRange_All.Location = new System.Drawing.Point(7, 101);
            this.btn_DateRange_All.Name = "btn_DateRange_All";
            this.btn_DateRange_All.Size = new System.Drawing.Size(89, 23);
            this.btn_DateRange_All.TabIndex = 4;
            this.btn_DateRange_All.Text = "All";
            this.btn_DateRange_All.UseVisualStyleBackColor = true;
            this.btn_DateRange_All.Click += new System.EventHandler(this.Btn_DateRange_All_Click);
            // 
            // btn_DateRange_Today
            // 
            this.btn_DateRange_Today.Location = new System.Drawing.Point(29, 72);
            this.btn_DateRange_Today.Name = "btn_DateRange_Today";
            this.btn_DateRange_Today.Size = new System.Drawing.Size(45, 23);
            this.btn_DateRange_Today.TabIndex = 2;
            this.btn_DateRange_Today.Text = "Today";
            this.btn_DateRange_Today.UseVisualStyleBackColor = true;
            this.btn_DateRange_Today.Click += new System.EventHandler(this.Btn_DateRange_Today_Click);
            // 
            // dtp_End
            // 
            this.dtp_End.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_End.Location = new System.Drawing.Point(7, 46);
            this.dtp_End.MinDate = new System.DateTime(2019, 6, 1, 0, 0, 0, 0);
            this.dtp_End.Name = "dtp_End";
            this.dtp_End.Size = new System.Drawing.Size(89, 20);
            this.dtp_End.TabIndex = 1;
            this.dtp_End.Value = new System.DateTime(2019, 6, 8, 0, 0, 0, 0);
            // 
            // dtp_Start
            // 
            this.dtp_Start.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtp_Start.Location = new System.Drawing.Point(7, 20);
            this.dtp_Start.MaxDate = new System.DateTime(2019, 7, 17, 0, 0, 0, 0);
            this.dtp_Start.MinDate = new System.DateTime(2019, 6, 1, 0, 0, 0, 0);
            this.dtp_Start.Name = "dtp_Start";
            this.dtp_Start.Size = new System.Drawing.Size(89, 20);
            this.dtp_Start.TabIndex = 0;
            this.dtp_Start.Value = new System.DateTime(2019, 7, 17, 0, 0, 0, 0);
            this.dtp_Start.Enter += new System.EventHandler(this.Dtp_Start_Enter);
            // 
            // pb_Plate
            // 
            this.pb_Plate.Location = new System.Drawing.Point(356, 16);
            this.pb_Plate.Name = "pb_Plate";
            this.pb_Plate.Size = new System.Drawing.Size(161, 74);
            this.pb_Plate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_Plate.TabIndex = 18;
            this.pb_Plate.TabStop = false;
            // 
            // tc_Dashboard
            // 
            this.tc_Dashboard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_Dashboard.Controls.Add(this.tp_Main);
            this.tc_Dashboard.Controls.Add(this.tp_Summary);
            this.tc_Dashboard.Controls.Add(this.tp_Settings);
            this.tc_Dashboard.Location = new System.Drawing.Point(12, 12);
            this.tc_Dashboard.Name = "tc_Dashboard";
            this.tc_Dashboard.SelectedIndex = 0;
            this.tc_Dashboard.Size = new System.Drawing.Size(1547, 961);
            this.tc_Dashboard.TabIndex = 19;
            this.tc_Dashboard.SelectedIndexChanged += new System.EventHandler(this.Tc_Dashboard_SelectedIndexChanged);
            // 
            // tp_Main
            // 
            this.tp_Main.BackColor = System.Drawing.Color.Honeydew;
            this.tp_Main.Controls.Add(this.lbl_Unique);
            this.tp_Main.Controls.Add(this.lbl_Total);
            this.tp_Main.Controls.Add(this.label7);
            this.tp_Main.Controls.Add(this.label6);
            this.tp_Main.Controls.Add(this.chk_IdentifyDups);
            this.tp_Main.Controls.Add(this.btn_SearchClear);
            this.tp_Main.Controls.Add(this.chk_HideNeighbors);
            this.tp_Main.Controls.Add(this.label4);
            this.tp_Main.Controls.Add(this.txt_PlateSearch);
            this.tp_Main.Controls.Add(this.gb_SpecificPlate);
            this.tp_Main.Controls.Add(this.groupBox3);
            this.tp_Main.Controls.Add(this.pb_Car);
            this.tp_Main.Controls.Add(this.groupBox2);
            this.tp_Main.Controls.Add(this.btn_Refresh);
            this.tp_Main.Controls.Add(this.dgv_Plates);
            this.tp_Main.Location = new System.Drawing.Point(4, 22);
            this.tp_Main.Name = "tp_Main";
            this.tp_Main.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Main.Size = new System.Drawing.Size(1539, 935);
            this.tp_Main.TabIndex = 0;
            this.tp_Main.Text = "Main Dashboard";
            // 
            // lbl_Unique
            // 
            this.lbl_Unique.AutoSize = true;
            this.lbl_Unique.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Unique.Location = new System.Drawing.Point(361, 26);
            this.lbl_Unique.Name = "lbl_Unique";
            this.lbl_Unique.Size = new System.Drawing.Size(81, 13);
            this.lbl_Unique.TabIndex = 31;
            this.lbl_Unique.Text = "500 / 20,000";
            // 
            // lbl_Total
            // 
            this.lbl_Total.AutoSize = true;
            this.lbl_Total.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Total.Location = new System.Drawing.Point(361, 7);
            this.lbl_Total.Name = "lbl_Total";
            this.lbl_Total.Size = new System.Drawing.Size(92, 13);
            this.lbl_Total.TabIndex = 30;
            this.lbl_Total.Text = "1,000 / 25,000";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "Displayed / Total Unique";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(223, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "Displayed / Total Records";
            // 
            // chk_IdentifyDups
            // 
            this.chk_IdentifyDups.AutoSize = true;
            this.chk_IdentifyDups.Location = new System.Drawing.Point(117, 25);
            this.chk_IdentifyDups.Name = "chk_IdentifyDups";
            this.chk_IdentifyDups.Size = new System.Drawing.Size(88, 17);
            this.chk_IdentifyDups.TabIndex = 27;
            this.chk_IdentifyDups.Text = "Identify Dups";
            this.chk_IdentifyDups.UseVisualStyleBackColor = true;
            // 
            // btn_SearchClear
            // 
            this.btn_SearchClear.Location = new System.Drawing.Point(512, 118);
            this.btn_SearchClear.Name = "btn_SearchClear";
            this.btn_SearchClear.Size = new System.Drawing.Size(18, 20);
            this.btn_SearchClear.TabIndex = 26;
            this.btn_SearchClear.Text = "X";
            this.btn_SearchClear.UseVisualStyleBackColor = true;
            this.btn_SearchClear.Click += new System.EventHandler(this.Btn_SearchClear_Click);
            // 
            // chk_HideNeighbors
            // 
            this.chk_HideNeighbors.AutoSize = true;
            this.chk_HideNeighbors.Location = new System.Drawing.Point(117, 6);
            this.chk_HideNeighbors.Name = "chk_HideNeighbors";
            this.chk_HideNeighbors.Size = new System.Drawing.Size(99, 17);
            this.chk_HideNeighbors.TabIndex = 25;
            this.chk_HideNeighbors.Text = "Hide Neighbors";
            this.chk_HideNeighbors.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(295, 125);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Plate to Search For";
            // 
            // txt_PlateSearch
            // 
            this.txt_PlateSearch.Location = new System.Drawing.Point(393, 118);
            this.txt_PlateSearch.Name = "txt_PlateSearch";
            this.txt_PlateSearch.Size = new System.Drawing.Size(118, 20);
            this.txt_PlateSearch.TabIndex = 23;
            // 
            // gb_SpecificPlate
            // 
            this.gb_SpecificPlate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gb_SpecificPlate.Controls.Add(this.btn_HidePlateEntry_Dup);
            this.gb_SpecificPlate.Controls.Add(this.txt_PlateAlert);
            this.gb_SpecificPlate.Controls.Add(this.label5);
            this.gb_SpecificPlate.Controls.Add(this.btn_FixPlateEntry);
            this.gb_SpecificPlate.Controls.Add(this.dgv_OtherHits);
            this.gb_SpecificPlate.Controls.Add(this.btn_KnownPlateUpdate);
            this.gb_SpecificPlate.Controls.Add(this.txt_PlateDescription);
            this.gb_SpecificPlate.Controls.Add(this.label3);
            this.gb_SpecificPlate.Controls.Add(this.cb_PlateStatus);
            this.gb_SpecificPlate.Controls.Add(this.label2);
            this.gb_SpecificPlate.Controls.Add(this.lbl_CurrentPlate);
            this.gb_SpecificPlate.Controls.Add(this.pb_Plate);
            this.gb_SpecificPlate.Location = new System.Drawing.Point(6, 197);
            this.gb_SpecificPlate.Name = "gb_SpecificPlate";
            this.gb_SpecificPlate.Size = new System.Drawing.Size(523, 370);
            this.gb_SpecificPlate.TabIndex = 22;
            this.gb_SpecificPlate.TabStop = false;
            this.gb_SpecificPlate.Text = "Car Details";
            // 
            // btn_HidePlateEntry_Dup
            // 
            this.btn_HidePlateEntry_Dup.Location = new System.Drawing.Point(454, 96);
            this.btn_HidePlateEntry_Dup.Name = "btn_HidePlateEntry_Dup";
            this.btn_HidePlateEntry_Dup.Size = new System.Drawing.Size(63, 23);
            this.btn_HidePlateEntry_Dup.TabIndex = 29;
            this.btn_HidePlateEntry_Dup.Text = "Duplicate";
            this.btn_HidePlateEntry_Dup.UseVisualStyleBackColor = true;
            this.btn_HidePlateEntry_Dup.Click += new System.EventHandler(this.Btn_HidePlateEntry_Click);
            // 
            // txt_PlateAlert
            // 
            this.txt_PlateAlert.Location = new System.Drawing.Point(74, 70);
            this.txt_PlateAlert.Name = "txt_PlateAlert";
            this.txt_PlateAlert.Size = new System.Drawing.Size(276, 20);
            this.txt_PlateAlert.TabIndex = 28;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Alert ?";
            // 
            // btn_FixPlateEntry
            // 
            this.btn_FixPlateEntry.Location = new System.Drawing.Point(171, 96);
            this.btn_FixPlateEntry.Name = "btn_FixPlateEntry";
            this.btn_FixPlateEntry.Size = new System.Drawing.Size(140, 23);
            this.btn_FixPlateEntry.TabIndex = 26;
            this.btn_FixPlateEntry.Text = "Fix Entry";
            this.btn_FixPlateEntry.UseVisualStyleBackColor = true;
            this.btn_FixPlateEntry.Click += new System.EventHandler(this.Btn_FixPlateEntry_Click);
            // 
            // dgv_OtherHits
            // 
            this.dgv_OtherHits.AllowUserToAddRows = false;
            this.dgv_OtherHits.AllowUserToDeleteRows = false;
            this.dgv_OtherHits.AllowUserToResizeRows = false;
            this.dgv_OtherHits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_OtherHits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_OtherHits.Location = new System.Drawing.Point(3, 125);
            this.dgv_OtherHits.MultiSelect = false;
            this.dgv_OtherHits.Name = "dgv_OtherHits";
            this.dgv_OtherHits.ReadOnly = true;
            this.dgv_OtherHits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OtherHits.Size = new System.Drawing.Size(517, 242);
            this.dgv_OtherHits.TabIndex = 25;
            this.dgv_OtherHits.SelectionChanged += new System.EventHandler(this.Dgv_OtherHits_SelectionChanged);
            // 
            // btn_KnownPlateUpdate
            // 
            this.btn_KnownPlateUpdate.Location = new System.Drawing.Point(4, 96);
            this.btn_KnownPlateUpdate.Name = "btn_KnownPlateUpdate";
            this.btn_KnownPlateUpdate.Size = new System.Drawing.Size(161, 23);
            this.btn_KnownPlateUpdate.TabIndex = 24;
            this.btn_KnownPlateUpdate.Text = "Add / Update Known Plate";
            this.btn_KnownPlateUpdate.UseVisualStyleBackColor = true;
            this.btn_KnownPlateUpdate.Click += new System.EventHandler(this.Btn_KnownPlateUpdate_Click);
            // 
            // txt_PlateDescription
            // 
            this.txt_PlateDescription.Location = new System.Drawing.Point(74, 43);
            this.txt_PlateDescription.Name = "txt_PlateDescription";
            this.txt_PlateDescription.Size = new System.Drawing.Size(276, 20);
            this.txt_PlateDescription.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Description";
            // 
            // cb_PlateStatus
            // 
            this.cb_PlateStatus.FormattingEnabled = true;
            this.cb_PlateStatus.Location = new System.Drawing.Point(220, 16);
            this.cb_PlateStatus.Name = "cb_PlateStatus";
            this.cb_PlateStatus.Size = new System.Drawing.Size(130, 21);
            this.cb_PlateStatus.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Status";
            // 
            // lbl_CurrentPlate
            // 
            this.lbl_CurrentPlate.AutoSize = true;
            this.lbl_CurrentPlate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CurrentPlate.Location = new System.Drawing.Point(7, 19);
            this.lbl_CurrentPlate.Name = "lbl_CurrentPlate";
            this.lbl_CurrentPlate.Size = new System.Drawing.Size(50, 20);
            this.lbl_CurrentPlate.TabIndex = 19;
            this.lbl_CurrentPlate.Text = "Plate";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rb_CarImage_Car);
            this.groupBox3.Controls.Add(this.rb_CarImage_Full);
            this.groupBox3.Location = new System.Drawing.Point(1446, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(87, 24);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            // 
            // rb_CarImage_Car
            // 
            this.rb_CarImage_Car.AutoSize = true;
            this.rb_CarImage_Car.Location = new System.Drawing.Point(48, 7);
            this.rb_CarImage_Car.Name = "rb_CarImage_Car";
            this.rb_CarImage_Car.Size = new System.Drawing.Size(41, 17);
            this.rb_CarImage_Car.TabIndex = 21;
            this.rb_CarImage_Car.Text = "Car";
            this.rb_CarImage_Car.UseVisualStyleBackColor = true;
            // 
            // rb_CarImage_Full
            // 
            this.rb_CarImage_Full.AutoSize = true;
            this.rb_CarImage_Full.Checked = true;
            this.rb_CarImage_Full.Location = new System.Drawing.Point(6, 7);
            this.rb_CarImage_Full.Name = "rb_CarImage_Full";
            this.rb_CarImage_Full.Size = new System.Drawing.Size(41, 17);
            this.rb_CarImage_Full.TabIndex = 20;
            this.rb_CarImage_Full.TabStop = true;
            this.rb_CarImage_Full.Text = "Full";
            this.rb_CarImage_Full.UseVisualStyleBackColor = true;
            this.rb_CarImage_Full.CheckedChanged += new System.EventHandler(this.Rb_CarImage_Full_CheckedChanged);
            // 
            // pb_Car
            // 
            this.pb_Car.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pb_Car.Location = new System.Drawing.Point(536, 6);
            this.pb_Car.Name = "pb_Car";
            this.pb_Car.Size = new System.Drawing.Size(997, 561);
            this.pb_Car.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_Car.TabIndex = 19;
            this.pb_Car.TabStop = false;
            // 
            // tp_Summary
            // 
            this.tp_Summary.Controls.Add(this.splitContainer1);
            this.tp_Summary.Controls.Add(this.dgv_PlateSummary);
            this.tp_Summary.Location = new System.Drawing.Point(4, 22);
            this.tp_Summary.Name = "tp_Summary";
            this.tp_Summary.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Summary.Size = new System.Drawing.Size(1539, 935);
            this.tp_Summary.TabIndex = 1;
            this.tp_Summary.Text = "Summary";
            this.tp_Summary.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(696, 573);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.chart_PlateSummary);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart_PlateSummaryPie);
            this.splitContainer1.Size = new System.Drawing.Size(837, 359);
            this.splitContainer1.SplitterDistance = 412;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 17;
            // 
            // chart_PlateSummary
            // 
            this.chart_PlateSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart_PlateSummary.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_PlateSummary.Legends.Add(legend1);
            this.chart_PlateSummary.Location = new System.Drawing.Point(0, 0);
            this.chart_PlateSummary.Name = "chart_PlateSummary";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_PlateSummary.Series.Add(series1);
            this.chart_PlateSummary.Size = new System.Drawing.Size(409, 356);
            this.chart_PlateSummary.TabIndex = 16;
            this.chart_PlateSummary.Text = "chart1";
            // 
            // chart_PlateSummaryPie
            // 
            this.chart_PlateSummaryPie.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea2.Name = "ChartArea1";
            this.chart_PlateSummaryPie.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart_PlateSummaryPie.Legends.Add(legend2);
            this.chart_PlateSummaryPie.Location = new System.Drawing.Point(8, 1);
            this.chart_PlateSummaryPie.Name = "chart_PlateSummaryPie";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart_PlateSummaryPie.Series.Add(series2);
            this.chart_PlateSummaryPie.Size = new System.Drawing.Size(430, 356);
            this.chart_PlateSummaryPie.TabIndex = 17;
            this.chart_PlateSummaryPie.Text = "chart2";
            // 
            // dgv_PlateSummary
            // 
            this.dgv_PlateSummary.AllowUserToAddRows = false;
            this.dgv_PlateSummary.AllowUserToDeleteRows = false;
            this.dgv_PlateSummary.AllowUserToResizeRows = false;
            this.dgv_PlateSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dgv_PlateSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_PlateSummary.Location = new System.Drawing.Point(3, 573);
            this.dgv_PlateSummary.MultiSelect = false;
            this.dgv_PlateSummary.Name = "dgv_PlateSummary";
            this.dgv_PlateSummary.ReadOnly = true;
            this.dgv_PlateSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_PlateSummary.Size = new System.Drawing.Size(696, 359);
            this.dgv_PlateSummary.TabIndex = 15;
            this.dgv_PlateSummary.SelectionChanged += new System.EventHandler(this.Dgv_PlateSummary_SelectionChanged);
            // 
            // tp_Settings
            // 
            this.tp_Settings.Controls.Add(this.txt_PlateStatusOptions);
            this.tp_Settings.Controls.Add(this.label9);
            this.tp_Settings.Controls.Add(this.label8);
            this.tp_Settings.Controls.Add(this.txt_programName);
            this.tp_Settings.Controls.Add(this.btn_SettingsSave);
            this.tp_Settings.Controls.Add(this.txt_SqlCon);
            this.tp_Settings.Controls.Add(this.label1);
            this.tp_Settings.Location = new System.Drawing.Point(4, 22);
            this.tp_Settings.Name = "tp_Settings";
            this.tp_Settings.Padding = new System.Windows.Forms.Padding(3);
            this.tp_Settings.Size = new System.Drawing.Size(1539, 935);
            this.tp_Settings.TabIndex = 2;
            this.tp_Settings.Text = "Settings";
            this.tp_Settings.UseVisualStyleBackColor = true;
            // 
            // txt_PlateStatusOptions
            // 
            this.txt_PlateStatusOptions.Location = new System.Drawing.Point(9, 108);
            this.txt_PlateStatusOptions.Multiline = true;
            this.txt_PlateStatusOptions.Name = "txt_PlateStatusOptions";
            this.txt_PlateStatusOptions.Size = new System.Drawing.Size(269, 49);
            this.txt_PlateStatusOptions.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 92);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(217, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Plate Status Options separated by semicolon";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(307, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Program Name";
            // 
            // txt_programName
            // 
            this.txt_programName.Location = new System.Drawing.Point(310, 20);
            this.txt_programName.Name = "txt_programName";
            this.txt_programName.Size = new System.Drawing.Size(172, 20);
            this.txt_programName.TabIndex = 12;
            // 
            // btn_SettingsSave
            // 
            this.btn_SettingsSave.Location = new System.Drawing.Point(9, 178);
            this.btn_SettingsSave.Name = "btn_SettingsSave";
            this.btn_SettingsSave.Size = new System.Drawing.Size(269, 60);
            this.btn_SettingsSave.TabIndex = 11;
            this.btn_SettingsSave.Text = "Save / Update Settings";
            this.btn_SettingsSave.UseVisualStyleBackColor = true;
            this.btn_SettingsSave.Click += new System.EventHandler(this.Btn_SettingsSave_Click);
            // 
            // txt_SqlCon
            // 
            this.txt_SqlCon.Location = new System.Drawing.Point(9, 20);
            this.txt_SqlCon.Multiline = true;
            this.txt_SqlCon.Name = "txt_SqlCon";
            this.txt_SqlCon.Size = new System.Drawing.Size(269, 49);
            this.txt_SqlCon.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "SQL Connection Information";
            // 
            // Dashboard
            // 
            this.AcceptButton = this.btn_Refresh;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(1569, 981);
            this.Controls.Add(this.tc_Dashboard);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Dashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LPR";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Plates)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Plate)).EndInit();
            this.tc_Dashboard.ResumeLayout(false);
            this.tp_Main.ResumeLayout(false);
            this.tp_Main.PerformLayout();
            this.gb_SpecificPlate.ResumeLayout(false);
            this.gb_SpecificPlate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OtherHits)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Car)).EndInit();
            this.tp_Summary.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_PlateSummary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_PlateSummaryPie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_PlateSummary)).EndInit();
            this.tp_Settings.ResumeLayout(false);
            this.tp_Settings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.DataGridView dgv_Plates;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dtp_End;
        private System.Windows.Forms.DateTimePicker dtp_Start;
        private System.Windows.Forms.PictureBox pb_Plate;
        private System.Windows.Forms.TabControl tc_Dashboard;
        private System.Windows.Forms.TabPage tp_Main;
        private System.Windows.Forms.TabPage tp_Summary;
        private System.Windows.Forms.Button btn_DateRange_All;
        private System.Windows.Forms.Button btn_DateRange_Today;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rb_CarImage_Car;
        private System.Windows.Forms.RadioButton rb_CarImage_Full;
        private System.Windows.Forms.PictureBox pb_Car;
        private System.Windows.Forms.GroupBox gb_SpecificPlate;
        private System.Windows.Forms.Label lbl_CurrentPlate;
        private System.Windows.Forms.ComboBox cb_PlateStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_PlateDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_KnownPlateUpdate;
        private System.Windows.Forms.DataGridView dgv_OtherHits;
        private System.Windows.Forms.TextBox txt_PlateSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_FixPlateEntry;
        private System.Windows.Forms.CheckBox chk_HideNeighbors;
        private System.Windows.Forms.TextBox txt_PlateAlert;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btn_SearchClear;
        private System.Windows.Forms.Button btn_HidePlateEntry_Dup;
        private System.Windows.Forms.DataGridView dgv_PlateSummary;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_PlateSummary;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_PlateSummaryPie;
        private System.Windows.Forms.CheckBox chk_IdentifyDups;
        private System.Windows.Forms.TabPage tp_Settings;
        private System.Windows.Forms.TextBox txt_SqlCon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_SettingsSave;
        private System.Windows.Forms.Button btn_DayForward;
        private System.Windows.Forms.Button btn_DayBack;
        private System.Windows.Forms.Label lbl_Unique;
        private System.Windows.Forms.Label lbl_Total;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_programName;
        private System.Windows.Forms.TextBox txt_PlateStatusOptions;
        private System.Windows.Forms.Label label9;
    }
}

