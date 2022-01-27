namespace TalkManager
{
    partial class Frm_Talk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Talk));
            this.Panel_ChatRoom = new System.Windows.Forms.Panel();
            this.TextBox_ChatRoom = new System.Windows.Forms.TextBox();
            this.Lbl_ChatRoom = new System.Windows.Forms.Label();
            this.CheckBox_Information = new System.Windows.Forms.CheckBox();
            this.Panel_Information_Time = new System.Windows.Forms.Panel();
            this.TextBox_Information_Time = new System.Windows.Forms.TextBox();
            this.Lbl_Information_Time = new System.Windows.Forms.Label();
            this.Btn_Start = new System.Windows.Forms.Button();
            this.Panel_Information = new System.Windows.Forms.Panel();
            this.TextBox_Information = new System.Windows.Forms.TextBox();
            this.Lbl_Information = new System.Windows.Forms.Label();
            this.CheckBox_Search = new System.Windows.Forms.CheckBox();
            this.Panel_Search = new System.Windows.Forms.Panel();
            this.TextBox_Search = new System.Windows.Forms.TextBox();
            this.Lbl_Search = new System.Windows.Forms.Label();
            this.CheckBox_Excluder = new System.Windows.Forms.CheckBox();
            this.Panel_Myname = new System.Windows.Forms.Panel();
            this.TextBox_Myname = new System.Windows.Forms.TextBox();
            this.Lbl_Myname = new System.Windows.Forms.Label();
            this.Panel_Excluder = new System.Windows.Forms.Panel();
            this.TextBox_Excluder = new System.Windows.Forms.TextBox();
            this.Lbl_Excluder = new System.Windows.Forms.Label();
            this.Panel_Restart = new System.Windows.Forms.Panel();
            this.TextBox_Restart = new System.Windows.Forms.TextBox();
            this.Lbl_Restart = new System.Windows.Forms.Label();
            this.TextBox_Log = new System.Windows.Forms.TextBox();
            this.Panel_Delay = new System.Windows.Forms.Panel();
            this.TextBox_Delay = new System.Windows.Forms.TextBox();
            this.Lbl_Delay = new System.Windows.Forms.Label();
            this.Panel_ChatRoom.SuspendLayout();
            this.Panel_Information_Time.SuspendLayout();
            this.Panel_Information.SuspendLayout();
            this.Panel_Search.SuspendLayout();
            this.Panel_Myname.SuspendLayout();
            this.Panel_Excluder.SuspendLayout();
            this.Panel_Restart.SuspendLayout();
            this.Panel_Delay.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_ChatRoom
            // 
            this.Panel_ChatRoom.Controls.Add(this.TextBox_ChatRoom);
            this.Panel_ChatRoom.Controls.Add(this.Lbl_ChatRoom);
            this.Panel_ChatRoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_ChatRoom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_ChatRoom.Location = new System.Drawing.Point(0, 100);
            this.Panel_ChatRoom.Name = "Panel_ChatRoom";
            this.Panel_ChatRoom.Size = new System.Drawing.Size(334, 120);
            this.Panel_ChatRoom.TabIndex = 3;
            // 
            // TextBox_ChatRoom
            // 
            this.TextBox_ChatRoom.BackColor = System.Drawing.Color.White;
            this.TextBox_ChatRoom.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_ChatRoom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_ChatRoom.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_ChatRoom.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_ChatRoom.Location = new System.Drawing.Point(119, 0);
            this.TextBox_ChatRoom.Multiline = true;
            this.TextBox_ChatRoom.Name = "TextBox_ChatRoom";
            this.TextBox_ChatRoom.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_ChatRoom.Size = new System.Drawing.Size(215, 120);
            this.TextBox_ChatRoom.TabIndex = 1;
            // 
            // Lbl_ChatRoom
            // 
            this.Lbl_ChatRoom.AutoSize = true;
            this.Lbl_ChatRoom.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_ChatRoom.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_ChatRoom.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_ChatRoom.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_ChatRoom.Location = new System.Drawing.Point(0, 0);
            this.Lbl_ChatRoom.Name = "Lbl_ChatRoom";
            this.Lbl_ChatRoom.Size = new System.Drawing.Size(119, 25);
            this.Lbl_ChatRoom.TabIndex = 0;
            this.Lbl_ChatRoom.Text = "채팅방 목록 :";
            // 
            // CheckBox_Information
            // 
            this.CheckBox_Information.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBox_Information.BackColor = System.Drawing.Color.PowderBlue;
            this.CheckBox_Information.Dock = System.Windows.Forms.DockStyle.Top;
            this.CheckBox_Information.FlatAppearance.BorderSize = 0;
            this.CheckBox_Information.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.CheckBox_Information.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBox_Information.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.CheckBox_Information.ForeColor = System.Drawing.Color.White;
            this.CheckBox_Information.Location = new System.Drawing.Point(0, 220);
            this.CheckBox_Information.Name = "CheckBox_Information";
            this.CheckBox_Information.Size = new System.Drawing.Size(334, 37);
            this.CheckBox_Information.TabIndex = 4;
            this.CheckBox_Information.Text = "고정문구 미실행";
            this.CheckBox_Information.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBox_Information.UseVisualStyleBackColor = false;
            this.CheckBox_Information.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Panel_Information_Time
            // 
            this.Panel_Information_Time.Controls.Add(this.TextBox_Information_Time);
            this.Panel_Information_Time.Controls.Add(this.Lbl_Information_Time);
            this.Panel_Information_Time.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Information_Time.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Information_Time.Location = new System.Drawing.Point(0, 257);
            this.Panel_Information_Time.Name = "Panel_Information_Time";
            this.Panel_Information_Time.Size = new System.Drawing.Size(334, 25);
            this.Panel_Information_Time.TabIndex = 5;
            // 
            // TextBox_Information_Time
            // 
            this.TextBox_Information_Time.BackColor = System.Drawing.Color.White;
            this.TextBox_Information_Time.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Information_Time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Information_Time.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Information_Time.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Information_Time.Location = new System.Drawing.Point(131, 0);
            this.TextBox_Information_Time.Name = "TextBox_Information_Time";
            this.TextBox_Information_Time.Size = new System.Drawing.Size(203, 20);
            this.TextBox_Information_Time.TabIndex = 1;
            // 
            // Lbl_Information_Time
            // 
            this.Lbl_Information_Time.AutoSize = true;
            this.Lbl_Information_Time.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Information_Time.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Information_Time.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Information_Time.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Information_Time.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Information_Time.Name = "Lbl_Information_Time";
            this.Lbl_Information_Time.Size = new System.Drawing.Size(131, 25);
            this.Lbl_Information_Time.TabIndex = 0;
            this.Lbl_Information_Time.Text = "실행시간 (분) :";
            // 
            // Btn_Start
            // 
            this.Btn_Start.BackColor = System.Drawing.Color.SteelBlue;
            this.Btn_Start.Dock = System.Windows.Forms.DockStyle.Top;
            this.Btn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Start.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.Btn_Start.ForeColor = System.Drawing.Color.White;
            this.Btn_Start.Location = new System.Drawing.Point(0, 0);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(334, 50);
            this.Btn_Start.TabIndex = 0;
            this.Btn_Start.Text = "시작";
            this.Btn_Start.UseVisualStyleBackColor = false;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // Panel_Information
            // 
            this.Panel_Information.Controls.Add(this.TextBox_Information);
            this.Panel_Information.Controls.Add(this.Lbl_Information);
            this.Panel_Information.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Information.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Information.Location = new System.Drawing.Point(0, 282);
            this.Panel_Information.Name = "Panel_Information";
            this.Panel_Information.Size = new System.Drawing.Size(334, 120);
            this.Panel_Information.TabIndex = 6;
            // 
            // TextBox_Information
            // 
            this.TextBox_Information.BackColor = System.Drawing.Color.White;
            this.TextBox_Information.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Information.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Information.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Information.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Information.Location = new System.Drawing.Point(101, 0);
            this.TextBox_Information.Multiline = true;
            this.TextBox_Information.Name = "TextBox_Information";
            this.TextBox_Information.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Information.Size = new System.Drawing.Size(233, 120);
            this.TextBox_Information.TabIndex = 1;
            // 
            // Lbl_Information
            // 
            this.Lbl_Information.AutoSize = true;
            this.Lbl_Information.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Information.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Information.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Information.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Information.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Information.Name = "Lbl_Information";
            this.Lbl_Information.Size = new System.Drawing.Size(101, 25);
            this.Lbl_Information.TabIndex = 0;
            this.Lbl_Information.Text = "고정 문구 :";
            // 
            // CheckBox_Search
            // 
            this.CheckBox_Search.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBox_Search.BackColor = System.Drawing.Color.PowderBlue;
            this.CheckBox_Search.Dock = System.Windows.Forms.DockStyle.Top;
            this.CheckBox_Search.FlatAppearance.BorderSize = 0;
            this.CheckBox_Search.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.CheckBox_Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBox_Search.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.CheckBox_Search.ForeColor = System.Drawing.Color.White;
            this.CheckBox_Search.Location = new System.Drawing.Point(0, 402);
            this.CheckBox_Search.Name = "CheckBox_Search";
            this.CheckBox_Search.Size = new System.Drawing.Size(334, 37);
            this.CheckBox_Search.TabIndex = 7;
            this.CheckBox_Search.Text = "검색 미실행";
            this.CheckBox_Search.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBox_Search.UseVisualStyleBackColor = false;
            this.CheckBox_Search.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Panel_Search
            // 
            this.Panel_Search.Controls.Add(this.TextBox_Search);
            this.Panel_Search.Controls.Add(this.Lbl_Search);
            this.Panel_Search.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Search.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Search.Location = new System.Drawing.Point(0, 439);
            this.Panel_Search.Name = "Panel_Search";
            this.Panel_Search.Size = new System.Drawing.Size(334, 120);
            this.Panel_Search.TabIndex = 8;
            // 
            // TextBox_Search
            // 
            this.TextBox_Search.BackColor = System.Drawing.Color.White;
            this.TextBox_Search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Search.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Search.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Search.Location = new System.Drawing.Point(101, 0);
            this.TextBox_Search.Multiline = true;
            this.TextBox_Search.Name = "TextBox_Search";
            this.TextBox_Search.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Search.Size = new System.Drawing.Size(233, 120);
            this.TextBox_Search.TabIndex = 1;
            // 
            // Lbl_Search
            // 
            this.Lbl_Search.AutoSize = true;
            this.Lbl_Search.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Search.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Search.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Search.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Search.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Search.Name = "Lbl_Search";
            this.Lbl_Search.Size = new System.Drawing.Size(101, 25);
            this.Lbl_Search.TabIndex = 0;
            this.Lbl_Search.Text = "검색 문구 :";
            // 
            // CheckBox_Excluder
            // 
            this.CheckBox_Excluder.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBox_Excluder.BackColor = System.Drawing.Color.PowderBlue;
            this.CheckBox_Excluder.Dock = System.Windows.Forms.DockStyle.Top;
            this.CheckBox_Excluder.FlatAppearance.BorderSize = 0;
            this.CheckBox_Excluder.FlatAppearance.CheckedBackColor = System.Drawing.Color.SteelBlue;
            this.CheckBox_Excluder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBox_Excluder.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.CheckBox_Excluder.ForeColor = System.Drawing.Color.White;
            this.CheckBox_Excluder.Location = new System.Drawing.Point(0, 559);
            this.CheckBox_Excluder.Name = "CheckBox_Excluder";
            this.CheckBox_Excluder.Size = new System.Drawing.Size(334, 37);
            this.CheckBox_Excluder.TabIndex = 9;
            this.CheckBox_Excluder.Text = "제외자 미실행";
            this.CheckBox_Excluder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBox_Excluder.UseVisualStyleBackColor = false;
            this.CheckBox_Excluder.CheckedChanged += new System.EventHandler(this.CheckBox_CheckedChanged);
            // 
            // Panel_Myname
            // 
            this.Panel_Myname.Controls.Add(this.TextBox_Myname);
            this.Panel_Myname.Controls.Add(this.Lbl_Myname);
            this.Panel_Myname.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Myname.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Myname.Location = new System.Drawing.Point(0, 75);
            this.Panel_Myname.Name = "Panel_Myname";
            this.Panel_Myname.Size = new System.Drawing.Size(334, 25);
            this.Panel_Myname.TabIndex = 2;
            // 
            // TextBox_Myname
            // 
            this.TextBox_Myname.BackColor = System.Drawing.Color.White;
            this.TextBox_Myname.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Myname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Myname.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Myname.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Myname.Location = new System.Drawing.Point(101, 0);
            this.TextBox_Myname.Name = "TextBox_Myname";
            this.TextBox_Myname.Size = new System.Drawing.Size(233, 20);
            this.TextBox_Myname.TabIndex = 1;
            // 
            // Lbl_Myname
            // 
            this.Lbl_Myname.AutoSize = true;
            this.Lbl_Myname.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Myname.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Myname.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Myname.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Myname.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Myname.Name = "Lbl_Myname";
            this.Lbl_Myname.Size = new System.Drawing.Size(101, 25);
            this.Lbl_Myname.TabIndex = 0;
            this.Lbl_Myname.Text = "내 대화명 :";
            // 
            // Panel_Excluder
            // 
            this.Panel_Excluder.Controls.Add(this.TextBox_Excluder);
            this.Panel_Excluder.Controls.Add(this.Lbl_Excluder);
            this.Panel_Excluder.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Excluder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Excluder.Location = new System.Drawing.Point(0, 596);
            this.Panel_Excluder.Name = "Panel_Excluder";
            this.Panel_Excluder.Size = new System.Drawing.Size(334, 120);
            this.Panel_Excluder.TabIndex = 10;
            // 
            // TextBox_Excluder
            // 
            this.TextBox_Excluder.BackColor = System.Drawing.Color.White;
            this.TextBox_Excluder.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Excluder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Excluder.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Excluder.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Excluder.Location = new System.Drawing.Point(77, 0);
            this.TextBox_Excluder.Multiline = true;
            this.TextBox_Excluder.Name = "TextBox_Excluder";
            this.TextBox_Excluder.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Excluder.Size = new System.Drawing.Size(257, 120);
            this.TextBox_Excluder.TabIndex = 1;
            // 
            // Lbl_Excluder
            // 
            this.Lbl_Excluder.AutoSize = true;
            this.Lbl_Excluder.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Excluder.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Excluder.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Excluder.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Excluder.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Excluder.Name = "Lbl_Excluder";
            this.Lbl_Excluder.Size = new System.Drawing.Size(77, 25);
            this.Lbl_Excluder.TabIndex = 0;
            this.Lbl_Excluder.Text = "제외자 :";
            // 
            // Panel_Restart
            // 
            this.Panel_Restart.Controls.Add(this.TextBox_Restart);
            this.Panel_Restart.Controls.Add(this.Lbl_Restart);
            this.Panel_Restart.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Restart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Restart.Location = new System.Drawing.Point(0, 716);
            this.Panel_Restart.Name = "Panel_Restart";
            this.Panel_Restart.Size = new System.Drawing.Size(334, 25);
            this.Panel_Restart.TabIndex = 11;
            // 
            // TextBox_Restart
            // 
            this.TextBox_Restart.BackColor = System.Drawing.Color.White;
            this.TextBox_Restart.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Restart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Restart.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Restart.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Restart.Location = new System.Drawing.Point(149, 0);
            this.TextBox_Restart.Name = "TextBox_Restart";
            this.TextBox_Restart.Size = new System.Drawing.Size(185, 20);
            this.TextBox_Restart.TabIndex = 1;
            // 
            // Lbl_Restart
            // 
            this.Lbl_Restart.AutoSize = true;
            this.Lbl_Restart.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Restart.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Restart.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Restart.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Restart.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Restart.Name = "Lbl_Restart";
            this.Lbl_Restart.Size = new System.Drawing.Size(149, 25);
            this.Lbl_Restart.TabIndex = 0;
            this.Lbl_Restart.Text = "재시작시간 (분) :";
            // 
            // TextBox_Log
            // 
            this.TextBox_Log.BackColor = System.Drawing.Color.White;
            this.TextBox_Log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Log.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Log.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Log.Location = new System.Drawing.Point(0, 741);
            this.TextBox_Log.Multiline = true;
            this.TextBox_Log.Name = "TextBox_Log";
            this.TextBox_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_Log.Size = new System.Drawing.Size(334, 0);
            this.TextBox_Log.TabIndex = 11;
            // 
            // Panel_Delay
            // 
            this.Panel_Delay.Controls.Add(this.TextBox_Delay);
            this.Panel_Delay.Controls.Add(this.Lbl_Delay);
            this.Panel_Delay.Dock = System.Windows.Forms.DockStyle.Top;
            this.Panel_Delay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Panel_Delay.Location = new System.Drawing.Point(0, 50);
            this.Panel_Delay.Name = "Panel_Delay";
            this.Panel_Delay.Size = new System.Drawing.Size(334, 25);
            this.Panel_Delay.TabIndex = 1;
            // 
            // TextBox_Delay
            // 
            this.TextBox_Delay.BackColor = System.Drawing.Color.White;
            this.TextBox_Delay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_Delay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_Delay.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.TextBox_Delay.ForeColor = System.Drawing.Color.SteelBlue;
            this.TextBox_Delay.Location = new System.Drawing.Point(138, 0);
            this.TextBox_Delay.Name = "TextBox_Delay";
            this.TextBox_Delay.Size = new System.Drawing.Size(196, 20);
            this.TextBox_Delay.TabIndex = 1;
            // 
            // Lbl_Delay
            // 
            this.Lbl_Delay.AutoSize = true;
            this.Lbl_Delay.BackColor = System.Drawing.Color.Transparent;
            this.Lbl_Delay.Dock = System.Windows.Forms.DockStyle.Left;
            this.Lbl_Delay.Font = new System.Drawing.Font("맑은 고딕", 13F, System.Drawing.FontStyle.Bold);
            this.Lbl_Delay.ForeColor = System.Drawing.Color.SteelBlue;
            this.Lbl_Delay.Location = new System.Drawing.Point(0, 0);
            this.Lbl_Delay.Name = "Lbl_Delay";
            this.Lbl_Delay.Size = new System.Drawing.Size(138, 25);
            this.Lbl_Delay.TabIndex = 0;
            this.Lbl_Delay.Text = "지연시간 (ms) :";
            // 
            // Frm_Talk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(334, 741);
            this.Controls.Add(this.TextBox_Log);
            this.Controls.Add(this.Panel_Restart);
            this.Controls.Add(this.Panel_Excluder);
            this.Controls.Add(this.CheckBox_Excluder);
            this.Controls.Add(this.Panel_Search);
            this.Controls.Add(this.CheckBox_Search);
            this.Controls.Add(this.Panel_Information);
            this.Controls.Add(this.Panel_Information_Time);
            this.Controls.Add(this.CheckBox_Information);
            this.Controls.Add(this.Panel_ChatRoom);
            this.Controls.Add(this.Panel_Myname);
            this.Controls.Add(this.Panel_Delay);
            this.Controls.Add(this.Btn_Start);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Frm_Talk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TalkManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Talk_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Talk_Load);
            this.Panel_ChatRoom.ResumeLayout(false);
            this.Panel_ChatRoom.PerformLayout();
            this.Panel_Information_Time.ResumeLayout(false);
            this.Panel_Information_Time.PerformLayout();
            this.Panel_Information.ResumeLayout(false);
            this.Panel_Information.PerformLayout();
            this.Panel_Search.ResumeLayout(false);
            this.Panel_Search.PerformLayout();
            this.Panel_Myname.ResumeLayout(false);
            this.Panel_Myname.PerformLayout();
            this.Panel_Excluder.ResumeLayout(false);
            this.Panel_Excluder.PerformLayout();
            this.Panel_Restart.ResumeLayout(false);
            this.Panel_Restart.PerformLayout();
            this.Panel_Delay.ResumeLayout(false);
            this.Panel_Delay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Panel_ChatRoom;
        private System.Windows.Forms.TextBox TextBox_ChatRoom;
        private System.Windows.Forms.Label Lbl_ChatRoom;
        private System.Windows.Forms.CheckBox CheckBox_Information;
        private System.Windows.Forms.Panel Panel_Information_Time;
        private System.Windows.Forms.Button Btn_Start;
        private System.Windows.Forms.TextBox TextBox_Information_Time;
        private System.Windows.Forms.Label Lbl_Information_Time;
        private System.Windows.Forms.Panel Panel_Information;
        private System.Windows.Forms.TextBox TextBox_Information;
        private System.Windows.Forms.Label Lbl_Information;
        private System.Windows.Forms.CheckBox CheckBox_Search;
        private System.Windows.Forms.Panel Panel_Search;
        private System.Windows.Forms.TextBox TextBox_Search;
        private System.Windows.Forms.Label Lbl_Search;
        private System.Windows.Forms.CheckBox CheckBox_Excluder;
        private System.Windows.Forms.Panel Panel_Myname;
        private System.Windows.Forms.TextBox TextBox_Myname;
        private System.Windows.Forms.Label Lbl_Myname;
        private System.Windows.Forms.Panel Panel_Excluder;
        private System.Windows.Forms.TextBox TextBox_Excluder;
        private System.Windows.Forms.Label Lbl_Excluder;
        private System.Windows.Forms.Panel Panel_Restart;
        private System.Windows.Forms.TextBox TextBox_Restart;
        private System.Windows.Forms.Label Lbl_Restart;
        private System.Windows.Forms.TextBox TextBox_Log;
        private System.Windows.Forms.Panel Panel_Delay;
        private System.Windows.Forms.TextBox TextBox_Delay;
        private System.Windows.Forms.Label Lbl_Delay;
    }
}