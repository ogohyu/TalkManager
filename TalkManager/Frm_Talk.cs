using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// 21.01.22 10:51 1.2.0 - ThreadWatchDog이랑 엮어서 처음 나감
// 21.01.25 20:34 1.2.1 - 741 line의 SendReturn 을 743의 else에서 741로 옮김, 그 외에 KeyboardHook 등 여러개에 Thread.Abort 추가 및 헛손질 안하게 isRetry 추가
// 21.02.01 12:28 1.2.2 - 내보내기, 가리기 기록 활성화 - 뭔가 오류가 있다고함..

namespace TalkManager
{
    public partial class Frm_Talk : Form
    {
        #region Keyboard Hooking
        [DllImport("user32.dll")] // 후킹 세팅 위한 거
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc callback, IntPtr hInstance, uint dwThreadId);

        [DllImport("user32.dll")] // 후킹 없애기
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")] // 후킹 들어온거 throw
        private static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")] // 전역에 세팅하기 위한 거
        private static extern IntPtr LoadLibrary(string lpFileName);
        #endregion

        #region INI
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        #region Window
        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [DllImport("user32.dll")] // 최상위 창 핸들값 가져오는 API
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")] // 인자로 받아온 핸들의 자식의 핸들값 가져오는 API
        private static extern IntPtr FindWindowEx(IntPtr hWnd1, IntPtr hWnd2, string lpsz1, string lpsz2);

        [DllImport("user32.dll")] // 파라미터의 윈도우를 상위로 가져옴
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")] // 현재 상위 윈도우를 가져옴
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)] // 윈도우을 옮겨주고 크기를 바꿔줌
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]  // 윈도우의 좌표를 가져옴
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        #endregion

        #region Message
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);

        [DllImport("user32.dll")]
        private static extern uint PostMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #endregion

        #region Mouse
        [DllImport("user32.dll")] // 입력 제어
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")] // 커서 위치 세팅
        static extern int SetCursorPos(int x, int y);
        #endregion

        #region Keyboard
        [DllImport("user32.dll")] // 입력 제어
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);
        #endregion

        #region Clipboard
        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);
        #endregion

        #region 변수
        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        private static HookProc KeyBoardHookProc = null;
        private static IntPtr hKeyboardHook = IntPtr.Zero;

        private static bool isRunning = false;
        private static string saveFileText = "";
        private static Thread TalkManager_Thread = null;
        private static object lockThread = new object();
        #endregion

        #region Form
        public Frm_Talk()
        {
            InitializeComponent();
        }

        private void Frm_Talk_Load(object sender, EventArgs e)
        {
            TextBox_Log.Visible = false; // @@ DEBUG용도

            KeyBoardHookProc = KeyboardhookProcedure;

            GetINI();
            SetINI();
            EnableComponent(true);
        }

        private void Frm_Talk_FormClosing(object sender, FormClosingEventArgs e)
        {
            SetINI();
            UnHookKeyboard();
            if (TalkManager_Thread != null) TalkManager_Thread.Abort();

            if (saveFileText != "")
            {
                if (new DirectoryInfo(@".\Log").Exists == false) new DirectoryInfo(@".\Log").Create(); // 기존폴더가 없으면 만들고
                using (StreamWriter sw = new StreamWriter(saveFileText, true, Encoding.UTF8)) // CSV에 쓰기
                {
                    sw.WriteLine(TextBox_Log.Text);
                }
            }

            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region Keyboard Hooking
        public void SetKeyboardHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hKeyboardHook = SetWindowsHookEx(13, KeyBoardHookProc, hInstance, 0); // WH_KEYBOARD_LL = 13
        }

        public void UnHookKeyboard()
        {
            UnhookWindowsHookEx(hKeyboardHook);
        }

        public IntPtr KeyboardhookProcedure(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)0x100) // WM_KEYDOWN = 0x100
            {
                if (Marshal.ReadInt32(lParam) == 160) // (int)Keys.LShiftKey = 160
                {
                    UnHookKeyboard();
                    if (TalkManager_Thread.IsAlive) TalkManager_Thread.Abort();

                    Process[] Target = Process.GetProcessesByName("ThreadWatchDog");
                    Thread.Sleep(200);
                    if (Target.Length != 0)
                    {
                        for (int i = 0; i < Target.Length; i++)
                        {
                            Target[i].Kill();
                            Thread.Sleep(200);
                        }
                    }

                    isRunning = false;
                    EnableComponent(true);
                    CloseWindow(null, 0, true);

                    return (IntPtr)1;
                }
            }

            return CallNextHookEx(hKeyboardHook, nCode, (int)wParam, lParam);
        }
        #endregion

        #region INI
        private void GetINI()
        {
            // 윈도우 사이즈
            this.Size = new Size(Convert.ToInt32(GetStr_ini("Window", "Window_width", this.Size.Width.ToString())), Convert.ToInt32(GetStr_ini("Window", "Window_height", this.Size.Height.ToString())));
            this.Location = new Point(Convert.ToInt32(GetStr_ini("Window", "Window_x", this.Location.X.ToString())), Convert.ToInt32(GetStr_ini("Window", "Window_y", this.Location.Y.ToString())));

            // 지연 시간
            TextBox_Delay.Text = GetStr_ini("Delay", "Delay_Time", "250");

            // 내 이름
            TextBox_Myname.Text = GetStr_ini("Myname", "Myname_Text", "");

            // 채팅방명
            TextBox_ChatRoom.Text = GetStr_ini("ChatRoom", "ChatRoom_Text", "").Replace("<br>", "\r\n");

            // 고정문구
            CheckBox_Information.Checked = Convert.ToBoolean(Convert.ToInt32(GetStr_ini("Information", "Information_isChecked", "0")));
            TextBox_Information_Time.Text = GetStr_ini("Information", "Information_Time", "15");
            TextBox_Information.Text = GetStr_ini("Information", "Information_Text", "").Replace("<br>", "\r\n");

            // 검색
            CheckBox_Search.Checked = Convert.ToBoolean(Convert.ToInt32(GetStr_ini("Search", "Search_isChecked", "0")));
            TextBox_Search.Text = GetStr_ini("Search", "Search_Text", "").Replace("<br>", "\r\n");

            // 제외자명
            CheckBox_Excluder.Checked = Convert.ToBoolean(Convert.ToInt32(GetStr_ini("Excluder", "Excluder_isChecked", "0")));
            TextBox_Excluder.Text = GetStr_ini("Excluder", "Excluder_Text", "").Replace("<br>", "\r\n");

            // 재시작 타임
            TextBox_Restart.Text = GetStr_ini("Restart", "Restart_Time", "5");

            CheckBox_CheckedChanged(null, null);
        }

        private void SetINI()
        {
            // 윈도우 사이즈
            SetStr_ini("Window", "Window_width", this.Size.Width.ToString());
            SetStr_ini("Window", "Window_height", this.Size.Height.ToString());
            SetStr_ini("Window", "Window_x", this.Location.X.ToString());
            SetStr_ini("Window", "Window_y", this.Location.Y.ToString());

            // 지연 시간
            SetStr_ini("Delay", "Delay_Time", TextBox_Delay.Text);

            // 내 이름
            SetStr_ini("Myname", "Myname_Text", TextBox_Myname.Text);

            // 채팅방명
            SetStr_ini("ChatRoom", "ChatRoom_Text", TextBox_ChatRoom.Text.Replace("\r\n", "<br>"));

            // 고정문구
            SetStr_ini("Information", "Information_isChecked", Convert.ToInt32(CheckBox_Information.Checked).ToString());
            SetStr_ini("Information", "Information_Time", TextBox_Information_Time.Text);
            SetStr_ini("Information", "Information_Text", TextBox_Information.Text.Replace("\r\n", "<br>"));

            // 검색
            SetStr_ini("Search", "Search_isChecked", Convert.ToInt32(CheckBox_Search.Checked).ToString());
            SetStr_ini("Search", "Search_Text", TextBox_Search.Text.Replace("\r\n", "<br>"));

            // 제외여부
            SetStr_ini("Excluder", "Excluder_isChecked", Convert.ToInt32(CheckBox_Excluder.Checked).ToString());
            SetStr_ini("Excluder", "Excluder_Text", TextBox_Excluder.Text.Replace("\r\n", "<br>"));

            // 재시작 타임
            SetStr_ini("Restart", "Restart_Time", TextBox_Restart.Text);
        }

        public static string GetStr_ini(string section, string key, string strDefault) // ini 데이터 가져오기
        {
            StringBuilder strRet = new StringBuilder(255); // 255라고 선언되어있긴 하지만 리턴값 길이가 늘어나면 자동으로 늘어남
            GetPrivateProfileString(section, key, strDefault, strRet, 255, @".\TalkManager.ini");

            return strRet.ToString();
        }

        public static void SetStr_ini(string section, string key, string value) // ini 데이터 넣기
        {
            WritePrivateProfileString(section, key, value, @".\TalkManager.ini");
            return;
        }
        #endregion

        #region Message
        private void SendReturn(IntPtr hWndEdit)
        {
            PostMessage(hWndEdit, 0x100, 0xD, 0); // WM_KEYDOWN = 0x100 / VK_RETURN = 0xD
            PostMessage(hWndEdit, 0x101, 0xD, 0); // WM_KEYUP = 0x101 / VK_RETURN = 0xD
            Thread.Sleep(100);
        }

        private void SendText(IntPtr hWndEdit, string strText, int sleepTime)
        {
            SendMessage(hWndEdit, 0x0C, 0, strText); // WM_SETTEXT = 0x0C
            Thread.Sleep(sleepTime);
            SendReturn(hWndEdit);
        }
        #endregion

        #region Clipboard
        private static string GetTextFromClipboard()
        {
            uint CF_UNICODETEXT = 13;

            if (!IsClipboardFormatAvailable(CF_UNICODETEXT)) return null;
            if (!OpenClipboard(IntPtr.Zero)) return null;

            string data = null;
            var hGlobal = GetClipboardData(CF_UNICODETEXT);
            if (hGlobal != IntPtr.Zero)
            {
                var lpwcstr = GlobalLock(hGlobal);
                if (lpwcstr != IntPtr.Zero)
                {
                    data = Marshal.PtrToStringUni(lpwcstr);
                    GlobalUnlock(lpwcstr);
                }
            }
            CloseClipboard();

            return data;
        }
        #endregion

        private void EnableComponent(bool isEnabled)
        {
            TextBox_Delay.Enabled = isEnabled;
            TextBox_Myname.Enabled = isEnabled;
            TextBox_ChatRoom.Enabled = isEnabled;
            CheckBox_Information.Enabled = isEnabled;
            TextBox_Information_Time.Enabled = isEnabled;
            TextBox_Information.Enabled = isEnabled;
            CheckBox_Search.Enabled = isEnabled;
            TextBox_Search.Enabled = isEnabled;
            CheckBox_Excluder.Enabled = isEnabled;
            TextBox_Excluder.Enabled = isEnabled;
            TextBox_Restart.Enabled = isEnabled;
            TextBox_Log.Enabled = isEnabled;

            if (isEnabled) Btn_Start.Text = "시작";
            else Btn_Start.Text = "중지 ( 왼쪽 Shift 키 )";
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Information.Checked)
            {
                CheckBox_Information.Text = "고정문구 실행";
                Panel_Information_Time.Visible = true;
                Panel_Information.Visible = true;
            }
            else
            {
                CheckBox_Information.Text = "고정문구 미실행";
                Panel_Information_Time.Visible = false;
                Panel_Information.Visible = false;
            }

            if (CheckBox_Search.Checked)
            {
                CheckBox_Search.Text = "검색 실행";
                Panel_Search.Visible = true;
                CheckBox_Excluder.Visible = true;
                if (CheckBox_Excluder.Checked)
                {
                    CheckBox_Excluder.Text = "제외자 제외";
                    Panel_Excluder.Visible = true;
                }
                else
                {
                    CheckBox_Excluder.Text = "제외자 없음";
                    Panel_Excluder.Visible = false;
                }
            }
            else
            {
                CheckBox_Search.Text = "검색 미실행";
                Panel_Search.Visible = false;
                CheckBox_Excluder.Visible = false;
                Panel_Excluder.Visible = false;
            }
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            if (isRunning)
            {
                UnHookKeyboard();
                if (TalkManager_Thread.IsAlive) TalkManager_Thread.Abort();

                Process[] Target = Process.GetProcessesByName("ThreadWatchDog");
                Thread.Sleep(200);
                if (Target.Length != 0)
                {
                    for (int i = 0; i < Target.Length; i++)
                    {
                        Target[i].Kill();
                        Thread.Sleep(200);
                    }
                }

                isRunning = false;
                EnableComponent(true);
                CloseWindow(null, 0, true);
            }
            else
            {
                SetINI();
                if (!Check_Component_Data()) return;

                CloseWindow(null, 0, true);
                EnableComponent(false);
                isRunning = true;
                
                #region searchArr[,] 위치
                const int NUM_CHAT = 0; // 대화방
                const int NUM_CHAT_SEARCH = 1; // 대화방 검색부분
                const int NUM_CHAT_CHAT = 2; // 대화방 대화부분
                const int NUM_CHAT_EDIT = 3; // 대화방 입력부분
                #endregion

                #region h_Chat[,] 위치
                const int ISCONTAINS = 0;
                const int ISEXCLUDE = 1;
                const int ISKICKOFF = 2;
                #endregion

                #region 변수 세팅
                bool informChecked = false, searchChecked = false, isNotClicked = true, isRetry = false;
                int informTime = 0, restartTime = 0, searchIndex = 0, kickOffCheck = 0, excludeCount = 0, delayTime = 0;
                int[,] searchArr = null;
                string informText = "", clipboardText = "";
                string[] chatRoomText = null, searchText = null, excluderText = null, kickOffText = null;
                DateTime restartTimeTemp = DateTime.Now;
                DateTime[] informTimeTemp = null;
                List<string> listRecord = null;
                RECT profileRect = new RECT();
                RECT[] chatRoomRect = null;
                IntPtr hKakaotalk = IntPtr.Zero, h_Room_Search = IntPtr.Zero, h_Profile = IntPtr.Zero;
                IntPtr[,] h_Chat = new IntPtr[1, 1];
                CultureInfo culture = new CultureInfo("ko-KR");
                Process[] Target;
                #endregion

                #region 기록 저장 세팅
                if (saveFileText != "")
                {
                    if (new DirectoryInfo(@".\Log").Exists == false) new DirectoryInfo(@".\Log").Create(); // 기존폴더가 없으면 만들고
                    using (StreamWriter sw = new StreamWriter(saveFileText, true, Encoding.UTF8)) // CSV에 쓰기
                    {
                        sw.WriteLine(TextBox_Log.Text);
                    }
                }
                saveFileText = @".\Log\TalkManager_Log_" + DateTime.Now.ToString("yyMMdd_HHmm") + ".txt";
                TextBox_Log.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[시작]\r\n";
                #endregion

                #region 채팅방 세팅
                chatRoomText = TextBox_ChatRoom.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int iRoom = 0; iRoom < chatRoomText.Length; iRoom++)
                {
                    chatRoomText[iRoom].Trim();
                    TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[채팅방 설정] " + chatRoomText[iRoom] + "\r\n");
                }
                #endregion

                #region 고정문구 세팅
                if (CheckBox_Information.Checked)
                {
                    informChecked = true;
                    informTime = Math.Abs(Convert.ToInt32(TextBox_Information_Time.Text));
                    informTimeTemp = new DateTime[chatRoomText.Length];
                    for (int iInform = 0; iInform < chatRoomText.Length; iInform++)
                    {
                        informTimeTemp[iInform] = DateTime.Now.AddMinutes(informTime * (-1));
                    }
                    informText = TextBox_Information.Text;
                    TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[고정문구 시간 설정] " + informTime + "분\r\n");
                    TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[고정문구 설정] " + informText + "\r\n");
                }
                #endregion

                #region 검색 세팅
                if (CheckBox_Search.Checked)
                {
                    searchChecked = true;
                    searchText = TextBox_Search.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int iSearch = 0; iSearch < searchText.Length; iSearch++)
                    {
                        searchText[iSearch].Trim();
                        TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[제외문구 설정] " + searchText[iSearch] + "\r\n");
                    }

                    kickOffText = new string[chatRoomText.Length];
                    for (int iKickOff = 0; iKickOff < chatRoomText.Length; iKickOff++)
                    {
                        kickOffText[iKickOff] = "";
                    }
                }
                #endregion

                #region 제외자 세팅
                if (CheckBox_Search.Checked && CheckBox_Excluder.Checked)
                {
                    excluderText = TextBox_Excluder.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    for (int iExcluder = 0; iExcluder < excluderText.Length; iExcluder++)
                    {
                        excluderText[iExcluder].Trim();
                        TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[제외자 설정] " + excluderText[iExcluder] + "\r\n");
                    }

                    Array.Resize(ref excluderText, excluderText.Length + 1);
                    excluderText[excluderText.Length - 1] = TextBox_Myname.Text;
                    TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[내 이름 설정] " + excluderText[excluderText.Length - 1] + "\r\n");
                }
                else
                {
                    excluderText = new string[1];
                    excluderText[0] = TextBox_Myname.Text;
                    TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[내 이름 설정] " + excluderText[excluderText.Length - 1] + "\r\n");
                }
                #endregion

                #region 지연시간 / 재시작 세팅
                delayTime = Math.Abs(Convert.ToInt32(TextBox_Delay.Text));
                TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[지연 시간 설정] " + delayTime + "ms\r\n");

                restartTime = Math.Abs(Convert.ToInt32(TextBox_Restart.Text));
                restartTimeTemp = DateTime.Now;
                TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[재시작 시간 설정] " + restartTime + "분\r\n");
                #endregion

                #region 채팅방 열기
                hKakaotalk = FindWindow("EVA_Window_Dblclk", "카카오톡"); // 카카오톡 창 
                if (hKakaotalk == IntPtr.Zero)
                {
                    TextBox_Log.AppendText("[오류 " + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]카카오톡이 실행되지 않습니다.\r\n");
                    MessageBox.Show("카카오톡을 실행해주세요.", "경고", MessageBoxButtons.OK);
                    isRunning = false;
                }
                else
                {
                    hKakaotalk = FindWindowEx(hKakaotalk, IntPtr.Zero, "EVA_ChildWindow", null); // v온라인모드 / 잠금모드
                    if (hKakaotalk == IntPtr.Zero)
                    {
                        TextBox_Log.AppendText("[오류 " + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]창1이 실행되지 않습니다.\r\n");
                        isRunning = false;
                    }
                    else
                    {
                        h_Room_Search = FindWindowEx(hKakaotalk, IntPtr.Zero, "EVA_Window", null); // v연락처목록 / 대화방목록 / 더보기
                        if (h_Room_Search == IntPtr.Zero)
                        {
                            TextBox_Log.AppendText("[오류 " + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]창2가 실행되지 않습니다.\r\n");
                            isRunning = false;
                        }
                        else
                        {
                            h_Room_Search = FindWindowEx(hKakaotalk, h_Room_Search, "EVA_Window", null); // 연락처목록 / v대화방목록 / 더보기
                            if (h_Room_Search == IntPtr.Zero)
                            {
                                TextBox_Log.AppendText("[오류 " + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]창3이 실행되지 않습니다.\r\n");
                                isRunning = false;
                            }
                            else
                            {
                                h_Room_Search = FindWindowEx(h_Room_Search, IntPtr.Zero, "Edit", null); // 대화방목록의 검색창
                                if (h_Room_Search == IntPtr.Zero)
                                {
                                    TextBox_Log.AppendText("[오류 " + DateTime.Now.ToString("MM-dd HH:mm:ss") + "]창4가 실행되지 않습니다.\r\n");
                                    isRunning = false;
                                }
                                else
                                {
                                    h_Chat = OpenWindow(h_Room_Search, chatRoomText);
                                }
                            }
                        }
                    }
                }
                #endregion

                if (isRunning)
                {
                    chatRoomRect = GetChatRoomRect(h_Chat, chatRoomText.Length);

                    #region 감시 프로그램 세팅
                    Target = Process.GetProcessesByName("ThreadWatchDog");
                    Thread.Sleep(200);
                    if (Target.Length == 0) Process.Start(@".\ThreadWatchDog.exe");
                    #endregion

                    UnHookKeyboard();
                    Thread.Sleep(200);
                    SetKeyboardHook();
                }
                else
                {
                    Target = Process.GetProcessesByName("ThreadWatchDog");
                    Thread.Sleep(200);
                    if (Target.Length != 0)
                    {
                        for (int i = 0; i < Target.Length; i++)
                        {
                            Target[i].Kill();
                            Thread.Sleep(200);
                        }
                    }

                    EnableComponent(true);
                    CloseWindow(null, 0, true);
                    return;
                }

                TalkManager_Thread = new Thread(delegate ()
                {
                    lock (lockThread)
                    {
                        while (isRunning)
                        {
                            if (restartTimeTemp.AddMinutes(restartTime) <= DateTime.Now)
                            {
                                restartTimeTemp = restartTimeTemp.AddMinutes(restartTime);

                                CloseWindow(h_Chat, chatRoomText.Length, false);
                                h_Chat = OpenWindow(h_Room_Search, chatRoomText);
                                if (isRunning) chatRoomRect = GetChatRoomRect(h_Chat, chatRoomText.Length);
                                else break;

                                if (new DirectoryInfo(@".\Log").Exists == false) new DirectoryInfo(@".\Log").Create(); // 기존폴더가 없으면 만들고
                                using (StreamWriter sw = new StreamWriter(saveFileText, true, Encoding.UTF8)) // CSV에 쓰기
                                {
                                    sw.Write(TextBox_Log.Text);
                                }

                                TextBox_Log.Text = DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[재시작]\r\n";
                            }

                            for (int iRoom = 0; iRoom < chatRoomText.Length; iRoom++)
                            {
                                if (!isRunning) break;

                                if (informChecked)
                                {
                                    if (informTimeTemp[iRoom].AddMinutes(informTime) <= DateTime.Now)
                                    {
                                        informTimeTemp[iRoom] = informTimeTemp[iRoom].AddMinutes(informTime);

                                        SendText(h_Chat[NUM_CHAT_EDIT, iRoom], informText, 100);
                                        TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[고정문구] " + chatRoomText[iRoom] + " 에 고정문구가 실행되었습니다.\r\n");
                                    }
                                }

                                if (searchChecked)
                                {
                                    SetForegroundWindow(h_Chat[NUM_CHAT_CHAT, iRoom]);
                                    keybd_event(0x23, 0, 0x0000, 0); // END KEYEVENTF_KEYDOWN
                                    keybd_event(0x23, 0, 0x0002, 0); // END KEYEVENTF_KEYUP
                                    Thread.Sleep(100);

                                    keybd_event(0x26, 0, 0x0000, 0); // UP KEYEVENTF_KEYDOWN
                                    keybd_event(0x26, 0, 0x0002, 0); // UP KEYEVENTF_KEYUP

                                    keybd_event(0xA2, 0, 0x0000, 0); // VK_LCONTROL KEYEVENTF_KEYDOWN
                                    keybd_event(0x41, 0, 0x0000, 0); // A KEYEVENTF_KEYDOWN
                                    keybd_event(0x41, 0, 0x0002, 0); // A KEYEVENTF_KEYUP
                                    keybd_event(0x43, 0, 0x0000, 0); // C KEYEVENTF_KEYDOWN
                                    keybd_event(0x43, 0, 0x0002, 0); // C KEYEVENTF_KEYUP
                                    keybd_event(0xA2, 0, 0x0002, 0); // VK_LCONTROL KEYEVENTF_KEYUP
                                    Thread.Sleep(100); // 100아니면 뻑남

                                    searchIndex = -1;
                                    clipboardText = GetTextFromClipboard();
                                    Thread.Sleep(100);
                                    for (int iSearch = 0; iSearch < searchText.Length; iSearch++)
                                    {
                                        if (searchIndex <= clipboardText.Split(new string[] { searchText[iSearch] }, StringSplitOptions.None).Length - 1) searchIndex = iSearch;
                                    }

                                    if (searchIndex > -1)
                                    {
                                        listRecord = new List<string>(clipboardText.Split(new string[] { "\r\n[" }, StringSplitOptions.None));

                                        for (int iRecord = 1; iRecord < listRecord.Count; iRecord++)
                                        {
                                            if (!listRecord[iRecord].Contains("] [오"))
                                            {
                                                listRecord[iRecord - 1] = listRecord[iRecord - 1] + listRecord[iRecord];
                                                listRecord.RemoveAt(iRecord);
                                                iRecord--;
                                            }
                                        }

                                        excludeCount = 0;
                                        kickOffCheck = 0;
                                        searchArr = new int[3, listRecord.Count];
                                        for (int iRecord = 0; iRecord < listRecord.Count; iRecord++)
                                        {
                                            searchArr[ISCONTAINS, iRecord] = 0; // 제외문자 가지고있는지
                                            searchArr[ISEXCLUDE, iRecord] = 0; // 제외를 할건지 (제외자인지)
                                            searchArr[ISKICKOFF, iRecord] = 0; // 내보내기 대상인지

                                            if (listRecord[iRecord].ToLower(culture).Contains(searchText[searchIndex].ToLower(culture))) // 제외자만 말한건지 체크하려고
                                            {
                                                searchArr[ISCONTAINS, iRecord] = 1;
                                                excludeCount++;

                                                for (int iExcluder = 0; iExcluder < excluderText.Length; iExcluder++)
                                                {
                                                    if (listRecord[iRecord].Substring(0, excluderText[iExcluder].Length).Contains(excluderText[iExcluder]))
                                                    {
                                                        searchArr[ISEXCLUDE, iRecord] = 1;
                                                        excludeCount--;
                                                        break;
                                                    }
                                                }

                                                if (excludeCount == 1 && kickOffCheck == 0)
                                                {
                                                    kickOffCheck = 1;
                                                    searchArr[ISKICKOFF, iRecord] = 1;
                                                }
                                            }
                                        }

                                        if (excludeCount > 0) // 제외자가 아닌 다른 사람이 제외 문자를 말한거면
                                        {
                                            SetCursorPos(chatRoomRect[iRoom].Left + 166, chatRoomRect[iRoom].Top + 49); // 대화방 검색창 돋보기 위치
                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004

                                            SendMessage(h_Chat[NUM_CHAT_SEARCH, iRoom], 0x0C, 0, searchText[searchIndex]); // WM_SETTEXT
                                            Thread.Sleep(100);
                                            SendReturn(h_Chat[NUM_CHAT_SEARCH, iRoom]);
                                            Thread.Sleep(100);

                                            // 내보내기
                                            for (int iRecord = listRecord.Count - 1; iRecord >= 0; iRecord--)
                                            {
                                                if (searchArr[ISCONTAINS, iRecord] == 1)
                                                {
                                                    if (searchArr[ISKICKOFF, iRecord] == 1)
                                                    {
                                                        if (listRecord[iRecord].Substring(0, listRecord[iRecord].IndexOf("] [오")) != kickOffText[iRoom])
                                                        {
                                                            // 프로필 쪽 커서
                                                            SetCursorPos(chatRoomRect[iRoom].Left + 21, chatRoomRect[iRoom].Top + 149);

                                                            // 프로필 누르기
                                                            isNotClicked = true;
                                                            while (isNotClicked)
                                                            {
                                                                mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                                mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                                Thread.Sleep(100);

                                                                // 프로필 윈도우 가져오기
                                                                if (GetForegroundWindow() == h_Chat[NUM_CHAT, iRoom])
                                                                {
                                                                    keybd_event(0x26, 0, 0x0000, 0); // UP KEYEVENTF_KEYDOWN
                                                                    keybd_event(0x26, 0, 0x0002, 0); // UP KEYEVENTF_KEYUP
                                                                    Thread.Sleep(10);
                                                                }
                                                                else isNotClicked = false;
                                                            }

                                                            // 프로필 윈도우 가져오기
                                                            h_Profile = GetForegroundWindow();
                                                            GetWindowRect(h_Profile, out profileRect);
                                                            Thread.Sleep(100);

                                                            // 프로필 내보내기 누르기
                                                            SetCursorPos(profileRect.Left + 140, profileRect.Bottom - 47);
                                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                            Thread.Sleep(100);

                                                            if (GetForegroundWindow() == h_Profile)
                                                            {
                                                                // 프로필 v내보내기 / 내보내고 신고
                                                                SetCursorPos(profileRect.Left + 100, profileRect.Bottom - 47);
                                                                mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                                mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                                Thread.Sleep(100);

                                                                // 내보내기 확인
                                                                SetCursorPos(profileRect.Left + 100, profileRect.Bottom - 240);
                                                                mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                                mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                                Thread.Sleep(100);

                                                                kickOffText[iRoom] = listRecord[iRecord].Substring(0, listRecord[iRecord].IndexOf("] [오"));
                                                                TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[내보내기] " + chatRoomText[iRoom] + " 에 내보내기가 실행되었습니다.\r\n");
                                                                TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[기록] [" + listRecord[iRecord] + "\r\n"); // @@ DEBUG용도
                                                            }
                                                            else
                                                            {
                                                                // 신고하기 취소
                                                                SetForegroundWindow(h_Chat[NUM_CHAT_CHAT, iRoom]); //  0 = NUM_CHAT
                                                                keybd_event(0x1B, 0, 0x0000, 0); // 0x1B = ESCAPE / 0x0000 = KEYEVENTF_KEYDOWN
                                                                keybd_event(0x1B, 0, 0x0002, 0); // 0x1B = ESCAPE / 0x0002 = KEYEVENTF_KEYUP
                                                                Thread.Sleep(10);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // 컨트롤 A 해제하려고
                                                            SetCursorPos(chatRoomRect[iRoom].Right - 41, chatRoomRect[iRoom].Top + 149);
                                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                            Thread.Sleep(100);
                                                        }
                                                        break;
                                                    }
                                                    else SendReturn(h_Chat[NUM_CHAT_SEARCH, iRoom]);
                                                }
                                            }

                                            // 가리기
                                            kickOffCheck = 0;
                                            for (int iRecord = 0; iRecord < listRecord.Count; iRecord++)
                                            {
                                                if (searchArr[ISCONTAINS, iRecord] == 1) // 제외문자 포함 여부
                                                {
                                                    if (searchArr[ISKICKOFF, iRecord] == 1) kickOffCheck = 1;
                                                    else if (kickOffCheck == 1)
                                                    {
                                                        //밑에 화살표 누르기
                                                        SetCursorPos(chatRoomRect[iRoom].Left + 258, chatRoomRect[iRoom].Top + 96);
                                                        mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                        mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                        Thread.Sleep(250); // 떨리는거때문에 넣어봄
                                                    }

                                                    if (kickOffCheck == 1)
                                                    {
                                                        if (searchArr[ISEXCLUDE, iRecord] == 0) // 제외자가 아니면
                                                        {
                                                            // 커서 세팅
                                                            SetCursorPos(chatRoomRect[iRoom].Left + 61, chatRoomRect[iRoom].Top + 149);

                                                            // 오른쪽 버튼 누르고
                                                            mouse_event(0x00000008, 0, 0, 0, 0); // 오른쪽 버튼 누르고 RBDOWN = 0x00000008
                                                            mouse_event(0x000000010, 0, 0, 0, 0); // 떼고 RBUP = 0x000000010
                                                            Thread.Sleep(250); // 창이 나와야 해서 좀 걸림

                                                            // 커서세팅하고 sleep 걸고
                                                            SetCursorPos(chatRoomRect[iRoom].Left + 71, chatRoomRect[iRoom].Top + 349);
                                                            Thread.Sleep(250); // 창이 나와야 해서 좀 걸림

                                                            // 차일드쪽으로 커서세팅한다음에
                                                            SetCursorPos(chatRoomRect[iRoom].Left + 181, chatRoomRect[iRoom].Top + 349);
                                                            Thread.Sleep(100);

                                                            // 왼쪽버튼누르기
                                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                            Thread.Sleep(250);

                                                            // 확인쪽으로 커서세팅하고
                                                            SetCursorPos(chatRoomRect[iRoom].Left + 86, chatRoomRect[iRoom].Top + 259);

                                                            // 왼쪽버튼누르기
                                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                                            Thread.Sleep(100);

                                                            TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[가리기] " + chatRoomText[iRoom] + " 에 가리기가 실행되었습니다.\r\n");
                                                            TextBox_Log.AppendText(DateTime.Now.ToString("yy-MM-dd HH:mm:ss") + "[기록] [" + listRecord[iRecord] + "\r\n"); // @@ DEBUG용도
                                                        }
                                                    }
                                                }
                                            }

                                            // 대화방 검색창 엑스 누르기
                                            SetCursorPos(chatRoomRect[iRoom].Left + 331, chatRoomRect[iRoom].Top + 96);
                                            mouse_event(0x00000002, 0, 0, 0, 0); // 왼쪽 버튼 누르고 LBDOWN = 0x00000002
                                            mouse_event(0x00000004, 0, 0, 0, 0); // 떼고 LBUP = 0x00000004
                                            Thread.Sleep(100);

                                            if (isRetry) isRetry = false;
                                            else
                                            {
                                                iRoom -= 1;
                                                isRetry = true;
                                            }
                                        }
                                    }

                                    Thread.Sleep(delayTime);
                                }
                                GC.Collect();
                            }
                        }

                        UnHookKeyboard();

                        Target = Process.GetProcessesByName("ThreadWatchDog");
                        Thread.Sleep(200);
                        if (Target.Length != 0)
                        {
                            for (int i = 0; i < Target.Length; i++)
                            {
                                Target[i].Kill();
                                Thread.Sleep(200);
                            }
                        }

                        EnableComponent(true);
                        CloseWindow(null, 0, true);
                    }
                });

                TalkManager_Thread.Priority = ThreadPriority.Normal;
                TalkManager_Thread.IsBackground = true;
                TalkManager_Thread.Name = "TalkManager_" + DateTime.Now.ToString("yy_MM_dd_HH_mm_ss") + ".exe";

                try
                {
                    TalkManager_Thread.Start(); // 쓰레드 시작
                }
                catch (Exception ee)
                {
                    UnHookKeyboard();
                    if (TalkManager_Thread.IsAlive) TalkManager_Thread.Abort();

                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] Thread가 중지되었습니다.\r\n");
                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] " + ee.StackTrace + "\r\n");
                    MessageBox.Show("오류에 의해 중지되었습니다.\r\n" + ee.StackTrace, "경고", MessageBoxButtons.OK);

                    Application.Exit();
                }
            }
        }

        private bool Check_Component_Data()
        {
            if (TextBox_Delay.Text == "")
            {
                MessageBox.Show("지연 시간을 입력해주세요.", "경고", MessageBoxButtons.OK);
                return false;
            }
            else if (!int.TryParse(TextBox_Delay.Text, out int a))
            {
                MessageBox.Show("지연 시간을 숫자로 입력해주세요.", "경고", MessageBoxButtons.OK);
                return false;
            }
            else if (Convert.ToInt32(TextBox_Delay.Text) < 250)
            {
                MessageBox.Show("지연 시간은 최소 250ms입니다.", "경고", MessageBoxButtons.OK);
                return false;
            }

            if (TextBox_Myname.Text == "")
            {
                MessageBox.Show("카카오톡의 내 이름을 입력해주세요. ( 모든 채팅방의 내 이름이 같아야 합니다. )", "경고", MessageBoxButtons.OK);
                return false;
            }

            if (TextBox_ChatRoom.Text == "")
            {
                MessageBox.Show("실행할 대화방의 정확한 이름을 입력해주세요.", "경고", MessageBoxButtons.OK);
                return false;
            }

            if (CheckBox_Information.Checked)
            {
                if (TextBox_Information_Time.Text == "")
                {
                    MessageBox.Show("고정문구 실행 시간을 입력해주세요.", "경고", MessageBoxButtons.OK);
                    return false;
                }
                else if (!int.TryParse(TextBox_Information_Time.Text, out int b))
                {
                    MessageBox.Show("고정문구 실행 시간을 숫자로 입력해주세요.", "경고", MessageBoxButtons.OK);
                    return false;
                }
                else if (Convert.ToInt32(TextBox_Information_Time.Text) < 1)
                {
                    MessageBox.Show("고정문구 실행 시간은 최소 1분입니다.", "경고", MessageBoxButtons.OK);
                    return false;
                }

                if (TextBox_Information.Text == "")
                {
                    MessageBox.Show("고정문구를 입력해주세요.", "경고", MessageBoxButtons.OK);
                    return false;
                }
            }

            if (CheckBox_Search.Checked)
            {
                if (TextBox_Search.Text == "")
                {
                    MessageBox.Show("검색할 문구를 입력해주세요.", "경고", MessageBoxButtons.OK);
                    return false;
                }

                if (CheckBox_Excluder.Checked)
                {
                    if (TextBox_Excluder.Text == "")
                    {
                        MessageBox.Show("제외자를 입력해주세요.", "경고", MessageBoxButtons.OK);
                        return false;
                    }
                }
            }

            if (TextBox_Restart.Text == "")
            {
                MessageBox.Show("재시작 실행 시간을 입력해주세요.", "경고", MessageBoxButtons.OK);
                return false;
            }
            else if (!int.TryParse(TextBox_Restart.Text, out int c))
            {
                MessageBox.Show("재시작 실행 시간을 숫자로 입력해주세요.", "경고", MessageBoxButtons.OK);
                return false;
            }
            else if (Convert.ToInt32(TextBox_Restart.Text) < 1)
            {
                MessageBox.Show("재시작 실행 시간은 최소 1분입니다.", "경고", MessageBoxButtons.OK);
                return false;
            }

            return true;
        }

        private IntPtr[,] OpenWindow(IntPtr h_Room_Search, string[] strChatRoom)
        {
            const int ROOM_WIDTH = 350;
            const int ROOM_HEIGHT = 410;

            const int NUM_CHAT = 0; // 대화방
            const int NUM_CHAT_SEARCH = 1; // 대화방 검색부분
            const int NUM_CHAT_CHAT = 2; // 대화방 대화부분
            const int NUM_CHAT_EDIT = 3; // 대화방 입력부분

            IntPtr[,] h_Chat = new IntPtr[4, strChatRoom.Length];

            for (int iRoom = 0; iRoom < strChatRoom.Length; iRoom++)
            {
                SendText(h_Room_Search, strChatRoom[iRoom], 500);
                SendMessage(h_Room_Search, 0x0C, 0, ""); // WM_SETTEXT
                Thread.Sleep(250);

                h_Chat[NUM_CHAT, iRoom] = FindWindow(null, strChatRoom[iRoom]);
                if (h_Chat[NUM_CHAT, iRoom] == IntPtr.Zero)
                {
                    isRunning = false;
                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] 창5가 실행되지 않습니다.\r\n");
                    MessageBox.Show("채팅방의 이름을 정확히 입력하였는지 다시 확인해주세요.");
                    break;
                }
                Thread.Sleep(100);

                MoveWindow(h_Chat[NUM_CHAT, iRoom], ROOM_WIDTH * (iRoom % 5), ROOM_HEIGHT * (iRoom / 5), ROOM_WIDTH, ROOM_HEIGHT, true); // 대화창위치X, 대화창위치Y, 대화창너비, 대화창높이
                //MoveWindow(h_Chat[NUM_CHAT, iRoom], ROOM_WIDTH * (iRoom % 5) + 2660, ROOM_HEIGHT * (iRoom / 5), ROOM_WIDTH, ROOM_HEIGHT, true); // 대화창위치X, 대화창위치Y, 대화창너비, 대화창높이 - @@ 회사용
                Thread.Sleep(100);

                h_Chat[NUM_CHAT_CHAT, iRoom] = FindWindowEx(h_Chat[NUM_CHAT, iRoom], IntPtr.Zero, "EVA_VH_ListControl_Dblclk", null); // 대화창의 대화부분 
                if (h_Chat[NUM_CHAT_CHAT, iRoom] == IntPtr.Zero)
                {
                    isRunning = false;
                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] 창6이 실행되지 않습니다.\r\n");
                    break;
                }

                h_Chat[NUM_CHAT_SEARCH, iRoom] = FindWindowEx(h_Chat[NUM_CHAT, iRoom], IntPtr.Zero, "Edit", null); // 대화창의 검색부분
                if (h_Chat[NUM_CHAT_SEARCH, iRoom] == IntPtr.Zero)
                {
                    isRunning = false;
                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] 창7이 실행되지 않습니다.\r\n");
                    break;
                }

                h_Chat[NUM_CHAT_EDIT, iRoom] = FindWindowEx(h_Chat[NUM_CHAT, iRoom], IntPtr.Zero, "RICHEDIT50W", null); // 대화창의 작성부분 
                if (h_Chat[NUM_CHAT_EDIT, iRoom] == IntPtr.Zero)
                {
                    isRunning = false;
                    TextBox_Log.AppendText(DateTime.Now.ToString("MM-dd HH:mm:ss") + "[오류] 창8이 실행되지 않습니다.\r\n");
                    break;
                }

                SetForegroundWindow(h_Chat[NUM_CHAT_CHAT, iRoom]);
                keybd_event(0x24, 0, 0x0000, 0); // HOME KEYEVENTF_KEYDOWN
                keybd_event(0x24, 0, 0x0002, 0); // HOME KEYEVENTF_KEYUP
                Thread.Sleep(10);
                keybd_event(0x24, 0, 0x0000, 0); // HOME KEYEVENTF_KEYDOWN
                keybd_event(0x24, 0, 0x0002, 0); // HOME KEYEVENTF_KEYUP
                Thread.Sleep(10);
                keybd_event(0x23, 0, 0x0000, 0); // END KEYEVENTF_KEYDOWN
                keybd_event(0x23, 0, 0x0002, 0); // END KEYEVENTF_KEYUP
                Thread.Sleep(100);
            }

            return h_Chat;
        }

        private void CloseWindow(IntPtr[,] h_ChatRoom, int roomCount, bool isEnd)
        {
            if (isEnd)
            {
                string[] chatRoomText = TextBox_ChatRoom.Text.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int iRoom = 0; iRoom < chatRoomText.Length; iRoom++)
                {
                    chatRoomText[iRoom].Trim();
                }

                IntPtr h_Chat;
                for (int iRoom = 0; iRoom < chatRoomText.Length; iRoom++)
                {
                    h_Chat = FindWindow(null, chatRoomText[iRoom]);
                    Thread.Sleep(100);
                    if (h_Chat != IntPtr.Zero)
                    {
                        SetForegroundWindow(h_Chat); //  0 = NUM_CHAT
                        keybd_event(0x1B, 0, 0x0000, 0); // 0x1B = ESCAPE / 0x0000 = KEYEVENTF_KEYDOWN
                        keybd_event(0x1B, 0, 0x0002, 0); // 0x1B = ESCAPE / 0x0002 = KEYEVENTF_KEYUP
                        Thread.Sleep(100);
                    }
                }
            }
            else
            {
                for (int iRoom = 0; iRoom < roomCount; iRoom++)
                {
                    SetForegroundWindow(h_ChatRoom[0, iRoom]); //  0 = NUM_CHAT
                    keybd_event(0x1B, 0, 0x0000, 0); // 0x1B = ESCAPE / 0x0000 = KEYEVENTF_KEYDOWN
                    keybd_event(0x1B, 0, 0x0002, 0); // 0x1B = ESCAPE / 0x0002 = KEYEVENTF_KEYUP
                    Thread.Sleep(100);
                }
            }
        }

        private RECT[] GetChatRoomRect(IntPtr[,] h_ChatRoom, int roomCount)
        {
            RECT[] chatRoomRect = new RECT[roomCount];

            for (int iRoom = 0; iRoom < roomCount; iRoom++)
            {
                chatRoomRect[iRoom] = new RECT();
                GetWindowRect(h_ChatRoom[0, iRoom], out chatRoomRect[iRoom]); // 0 = NUM_CHAT
                Thread.Sleep(100);
            }

            return chatRoomRect;
        }
    }
}
