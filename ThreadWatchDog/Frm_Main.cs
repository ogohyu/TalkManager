using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using ThreadWatchDog.Properties;

namespace ThreadWatchDog
{
    public partial class Frm_Main : Form
    {
        #region Dll Import
        [DllImport("user32.dll")] // 최상위 창 핸들값 가져오는 API
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")]  // 윈도우의 좌표를 가져옴
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")] // 입력 제어
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")] // 커서 위치 세팅
        static extern int SetCursorPos(int x, int y);
        #endregion

        delegate void SetTextCallBack(string text);

        public Frm_Main()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(10, 940);
            this.Show();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            Timer_WatchDog.Interval = 1000;
            Timer_WatchDog.Tick += Timer_WatchDog_Tick;
            Timer_WatchDog.Start();

            Thread Thread_WatchDog = new Thread(delegate ()
            {
                while (true)
                {
                    Process[] Target = Process.GetProcessesByName("TalkManager");
                    Thread.Sleep(200);

                    if (Target.Length != 0)
                    {
                        Pb_ProgramStatus.Image = Resources.success;
                        lbl_ProgramStatus.Text = "프로그램 정상 작동 중";
                        Thread.Sleep(200);
                    }
                    else
                    {
                        Pb_ProgramStatus.Image = Resources.error;
                        lbl_ProgramStatus.Text = "프로그램이 종료되었습니다.";

                        if (Target.Length != 0)
                        {
                            for (int i = 0; i < Target.Length; i++)
                            {
                                Target[i].Kill();
                                Thread.Sleep(200);
                            }
                        }
                        else
                        {
                            Pb_ProgramStatus.Image = Resources.warning;

                            string path = Application.StartupPath + @".\TalkManager.exe";
                            lbl_ProgramStatus.Text = "프로그램을 다시 실행합니다.";
                            Process.Start(path);
                            Thread.Sleep(1000);

                            lbl_ProgramStatus.Text = "잠시만 기다려주세요";
                            Thread.Sleep(1000);

                            IntPtr hTalk = FindWindow(null, "TalkManager");
                            if (hTalk != IntPtr.Zero)
                            {
                                Thread.Sleep(250);
                                RECT lpRect;
                                GetWindowRect(hTalk, out lpRect);

                                SetCursorPos(lpRect.Left + 100, lpRect.Top + 50);
                                mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                            }
                        }
                    }
                    GC.Collect();
                }
            });

            Thread_WatchDog.IsBackground = true;
            Thread_WatchDog.Priority = ThreadPriority.AboveNormal;
            Thread_WatchDog.Start();
        }

        int Total_Sec_Count = 0;
        int Total_Min_Count = 0;
        int Total_Hour_Count = 0;
        private void Timer_WatchDog_Tick(object sender, EventArgs e)
        {
            Total_Sec_Count++;

            if (Total_Sec_Count >= 60)
            {
                Total_Min_Count++;
                Total_Sec_Count = 0;
                if (Total_Min_Count >= 60)
                {
                    Total_Min_Count = 0;
                    Total_Hour_Count++;

                    if (Total_Hour_Count % 12 == 0)
                    {
                        Process[] Target = Process.GetProcessesByName("TalkManager");
                        Thread.Sleep(200);

                        if (Target.Length != 0)
                        {
                            for (int i = 0; i < Target.Length; i++)
                            {
                                Target[i].Kill();
                                Thread.Sleep(200);
                            }
                        }

                        Pb_ProgramStatus.Image = Resources.warning;

                        string path = Application.StartupPath + @".\TalkManager.exe";
                        lbl_ProgramStatus.Text = "안정성 향상을 위해 프로그램을 다시 실행합니다.";
                        Process.Start(path);
                        Thread.Sleep(1000);

                        lbl_ProgramStatus.Text = "잠시만 기다려주세요";
                        Thread.Sleep(1000);

                        IntPtr hTalk = FindWindow(null, "TalkManager");
                        if (hTalk != IntPtr.Zero)
                        {
                            Thread.Sleep(250);
                            RECT lpRect;
                            GetWindowRect(hTalk, out lpRect);

                            SetCursorPos(lpRect.Left + 100, lpRect.Top + 50);
                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                        }
                    }
                }
            }

            lbl_CountTime.Text = string.Format("실행 시간 : {0}시간 {1}분 {2}초", Total_Hour_Count, Total_Min_Count, Total_Sec_Count);
            lbl_CurrentTime.Text = DateTime.Now.ToString("현재 시간 : yy년 MM월 dd일 HH시 mm분 ss초");

            GC.Collect();
        }
    }
}
