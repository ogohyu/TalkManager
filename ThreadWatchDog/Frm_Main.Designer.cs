namespace ThreadWatchDog
{
    partial class Frm_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Main));
            this.lbl_CurrentTime = new System.Windows.Forms.Label();
            this.Timer_WatchDog = new System.Windows.Forms.Timer(this.components);
            this.lbl_Copyright = new System.Windows.Forms.Label();
            this.lbl_Title = new System.Windows.Forms.Label();
            this.lbl_ProgramStatus = new System.Windows.Forms.Label();
            this.Pb_ProgramStatus = new System.Windows.Forms.PictureBox();
            this.lbl_CountTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Pb_ProgramStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_CurrentTime
            // 
            this.lbl_CurrentTime.AutoSize = true;
            this.lbl_CurrentTime.Location = new System.Drawing.Point(45, 27);
            this.lbl_CurrentTime.Name = "lbl_CurrentTime";
            this.lbl_CurrentTime.Size = new System.Drawing.Size(11, 12);
            this.lbl_CurrentTime.TabIndex = 6;
            this.lbl_CurrentTime.Text = "-";
            // 
            // Timer_WatchDog
            // 
            this.Timer_WatchDog.Tick += new System.EventHandler(this.Timer_WatchDog_Tick);
            // 
            // lbl_Copyright
            // 
            this.lbl_Copyright.AutoSize = true;
            this.lbl_Copyright.Font = new System.Drawing.Font("굴림", 8F);
            this.lbl_Copyright.Location = new System.Drawing.Point(204, 3);
            this.lbl_Copyright.Name = "lbl_Copyright";
            this.lbl_Copyright.Size = new System.Drawing.Size(69, 11);
            this.lbl_Copyright.TabIndex = 11;
            this.lbl_Copyright.Text = "© WOWOW";
            // 
            // lbl_Title
            // 
            this.lbl_Title.AutoSize = true;
            this.lbl_Title.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Title.Location = new System.Drawing.Point(7, 8);
            this.lbl_Title.Name = "lbl_Title";
            this.lbl_Title.Size = new System.Drawing.Size(159, 12);
            this.lbl_Title.TabIndex = 10;
            this.lbl_Title.Text = "TalkManager WatchDog";
            // 
            // lbl_ProgramStatus
            // 
            this.lbl_ProgramStatus.AutoSize = true;
            this.lbl_ProgramStatus.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ProgramStatus.Location = new System.Drawing.Point(6, 65);
            this.lbl_ProgramStatus.Name = "lbl_ProgramStatus";
            this.lbl_ProgramStatus.Size = new System.Drawing.Size(176, 13);
            this.lbl_ProgramStatus.TabIndex = 9;
            this.lbl_ProgramStatus.Text = "프로그램 상태 불러오는 중";
            // 
            // Pb_ProgramStatus
            // 
            this.Pb_ProgramStatus.Location = new System.Drawing.Point(9, 27);
            this.Pb_ProgramStatus.Name = "Pb_ProgramStatus";
            this.Pb_ProgramStatus.Size = new System.Drawing.Size(30, 30);
            this.Pb_ProgramStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Pb_ProgramStatus.TabIndex = 8;
            this.Pb_ProgramStatus.TabStop = false;
            // 
            // lbl_CountTime
            // 
            this.lbl_CountTime.AutoSize = true;
            this.lbl_CountTime.Location = new System.Drawing.Point(45, 45);
            this.lbl_CountTime.Name = "lbl_CountTime";
            this.lbl_CountTime.Size = new System.Drawing.Size(11, 12);
            this.lbl_CountTime.TabIndex = 7;
            this.lbl_CountTime.Text = "-";
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(276, 87);
            this.Controls.Add(this.lbl_CurrentTime);
            this.Controls.Add(this.lbl_Copyright);
            this.Controls.Add(this.lbl_Title);
            this.Controls.Add(this.lbl_ProgramStatus);
            this.Controls.Add(this.Pb_ProgramStatus);
            this.Controls.Add(this.lbl_CountTime);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Frm_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "WatchDog Thread";
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Pb_ProgramStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_CurrentTime;
        private System.Windows.Forms.Timer Timer_WatchDog;
        private System.Windows.Forms.Label lbl_Copyright;
        private System.Windows.Forms.Label lbl_Title;
        private System.Windows.Forms.Label lbl_ProgramStatus;
        private System.Windows.Forms.PictureBox Pb_ProgramStatus;
        private System.Windows.Forms.Label lbl_CountTime;
    }
}

