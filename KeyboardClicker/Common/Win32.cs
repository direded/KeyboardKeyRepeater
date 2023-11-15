using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace KeyboardClicker.Common;

public class Win32 {
	public static Color GetColorAt(IntPtr dc, int x, int y) {
		uint a = GetPixel(dc, x, y);
		return Color.FromArgb((int) (a & 0x000000FF),
				(int) (a & 0x0000FF00) >> 8,
				(int) (a & 0x00FF0000) >> 16);
	}

	public static void MouseClick() {
		mouse_event((uint) (Win32.MOUSEEVENTF.LEFTDOWN | Win32.MOUSEEVENTF.LEFTUP), 0, 0, 0, 0);
	}

	public static void MouseClick(IntPtr handle, int x, int y) {
		Win32.POINT p;
		p = new Win32.POINT(x, y);
		Win32.ClientToScreen(handle, ref p);
		Win32.SetCursorPos(p.x, p.y);
		Win32.mouse_event((uint) (Win32.MOUSEEVENTF.LEFTDOWN | Win32.MOUSEEVENTF.LEFTUP), (uint) p.x, (uint) p.y, 0, 0);
	}

	public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

	[DllImport("user32.dll")]
	public static extern bool GetCursorPos(ref Point lpPoint);

	[DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
	public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

	[DllImport("user32.dll")]
	public static extern long SetCursorPos(int x, int y);

	[DllImport("user32.dll")]
	public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

	//[DllImport("user32.dll")]
	//public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

	[DllImport("user32.dll")]
	public static extern void keybd_event(int bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

	[DllImport("user32.dll")]
	public static extern bool SetForegroundWindow(IntPtr hWnd);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetDesktopWindow();

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetWindowDC(IntPtr window);

	[DllImport("gdi32.dll", SetLastError = true)]
	public static extern uint GetPixel(IntPtr dc, int x, int y);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern int ReleaseDC(IntPtr window, IntPtr dc);

	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool BringWindowToTop(IntPtr hWnd);

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, uint wMsg, UIntPtr wParam, IntPtr lParam); //used for maximizing the screen

	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

	[DllImport("user32.dll")]
	public static extern int GetSystemMetrics(int nIndex);

	[DllImport("dwmapi.dll")]
	public static extern int DwmGetWindowAttribute(IntPtr hwnd, int dwAttribute, out RECT pvAttribute, int cbAttribute);

	[DllImport("user32.dll")]
	public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

	[DllImport("user32.dll")]
	public static extern int SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte bAlpha, int dwFlags);

	[DllImport("user32.dll", SetLastError = true)]
	public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

	[DllImport("user32.dll")]
	public static extern bool ShowWindow(IntPtr hwnd, int cmdShow);

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern IntPtr GetForegroundWindow();

	[DllImport("user32.dll")]
	public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

	public delegate IntPtr LowLevelWindowsHookProc(int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelWindowsHookProc lpfn, IntPtr hMod, uint dwThreadId);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	public static extern bool UnhookWindowsHookEx(IntPtr hhk);

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	[DllImport("kernel32.dll")]
	public static extern IntPtr LoadLibrary(string lpFileName);

	[DllImport(@"user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
	public static extern int GetMessage(ref MSG message, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
	[DllImport(@"user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
	public static extern bool TranslateMessage(ref MSG message);
	[DllImport(@"user32.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
	public static extern long DispatchMessage(ref MSG message);

	[return: MarshalAs(UnmanagedType.Bool)]
	[DllImport("user32.dll", SetLastError = true)]
	public static extern bool PostThreadMessage(uint threadId, int msg, IntPtr wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	public static extern uint GetCurrentThreadId();

	[StructLayout(LayoutKind.Sequential)]
	public struct MSG {
		IntPtr hwnd;
		public uint message;
		UIntPtr wParam;
		IntPtr lParam;
		uint time;
		POINT pt;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct POINT {
		public int x;
		public int y;

		public POINT(int X, int Y) {
			x = X;
			y = Y;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT {
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	[Flags]
	public enum WindowStyles : uint {
		WS_OVERLAPPED = 0x00000000,
		WS_POPUP = 0x80000000,
		WS_CHILD = 0x40000000,
		WS_MINIMIZE = 0x20000000,
		WS_VISIBLE = 0x10000000,
		WS_DISABLED = 0x08000000,
		WS_CLIPSIBLINGS = 0x04000000,
		WS_CLIPCHILDREN = 0x02000000,
		WS_MAXIMIZE = 0x01000000,
		WS_BORDER = 0x00800000,
		WS_DLGFRAME = 0x00400000,
		WS_VSCROLL = 0x00200000,
		WS_HSCROLL = 0x00100000,
		WS_SYSMENU = 0x00080000,
		WS_THICKFRAME = 0x00040000,
		WS_GROUP = 0x00020000,
		WS_TABSTOP = 0x00010000,

		WS_MINIMIZEBOX = 0x00020000,
		WS_MAXIMIZEBOX = 0x00010000,

		WS_CAPTION = 0x00C00000,
		WS_TILED = WS_OVERLAPPED,
		WS_ICONIC = WS_MINIMIZE,
		WS_SIZEBOX = WS_THICKFRAME,
		WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

		WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
		WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
		WS_CHILDWINDOW = WS_CHILD,

		//Extended Window Styles

		WS_EX_DLGMODALFRAME = 0x00000001,
		WS_EX_NOPARENTNOTIFY = 0x00000004,
		WS_EX_TOPMOST = 0x00000008,
		WS_EX_ACCEPTFILES = 0x00000010,
		WS_EX_TRANSPARENT = 0x00000020,

		//#if(WINVER >= 0x0400)

		WS_EX_MDICHILD = 0x00000040,
		WS_EX_TOOLWINDOW = 0x00000080,
		WS_EX_WINDOWEDGE = 0x00000100,
		WS_EX_CLIENTEDGE = 0x00000200,
		WS_EX_CONTEXTHELP = 0x00000400,

		WS_EX_RIGHT = 0x00001000,
		WS_EX_LEFT = 0x00000000,
		WS_EX_RTLREADING = 0x00002000,
		WS_EX_LTRREADING = 0x00000000,
		WS_EX_LEFTSCROLLBAR = 0x00004000,
		WS_EX_RIGHTSCROLLBAR = 0x00000000,

		WS_EX_CONTROLPARENT = 0x00010000,
		WS_EX_STATICEDGE = 0x00020000,
		WS_EX_APPWINDOW = 0x00040000,

		WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
		WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
		//#endif /* WINVER >= 0x0400 */

		//#if(WIN32WINNT >= 0x0500)

		WS_EX_LAYERED = 0x00080000,
		//#endif /* WIN32WINNT >= 0x0500 */

		//#if(WINVER >= 0x0500)

		WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
		WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
									  //#endif /* WINVER >= 0x0500 */

		//#if(WIN32WINNT >= 0x0500)

		WS_EX_COMPOSITED = 0x02000000,
		WS_EX_NOACTIVATE = 0x08000000
		//#endif /* WIN32WINNT >= 0x0500 */

	}

	[Flags]
	public enum DwmWindowAttribute : uint {
		DWMWA_NCRENDERING_ENABLED = 1,
		DWMWA_NCRENDERING_POLICY,
		DWMWA_TRANSITIONS_FORCEDISABLED,
		DWMWA_ALLOW_NCPAINT,
		DWMWA_CAPTION_BUTTON_BOUNDS,
		DWMWA_NONCLIENT_RTL_LAYOUT,
		DWMWA_FORCE_ICONIC_REPRESENTATION,
		DWMWA_FLIP3D_POLICY,
		DWMWA_EXTENDED_FRAME_BOUNDS,
		DWMWA_HAS_ICONIC_BITMAP,
		DWMWA_DISALLOW_PEEK,
		DWMWA_EXCLUDED_FROM_PEEK,
		DWMWA_CLOAK,
		DWMWA_CLOAKED,
		DWMWA_FREEZE_REPRESENTATION,
		DWMWA_LAST
	}

	public enum GetWindowLongConst {
		GWL_WNDPROC = (-4),
		GWL_HINSTANCE = (-6),
		GWL_HWNDPARENT = (-8),
		GWL_STYLE = (-16),
		GWL_EXSTYLE = (-20),
		GWL_USERDATA = (-21),
		GWL_ID = (-12)
	}

	public enum LWA {
		ColorKey = 0x1,
		Alpha = 0x2,
	}

	[StructLayout(LayoutKind.Sequential)]
	public class MouseHookStruct {
		public POINT pt;
		public int hwnd;
		public int wHitTestCode;
		public int dwExtraInfo;
	}

	[StructLayout(LayoutKind.Sequential)]
	public class KbdLLHookStruct {
		public int vkCode;
		public int scanCode;
		public int flags;
		public int time;
		public int dwExtraInfo;
	}


	public const int LWA_COLORKEY = 0x00000001;

	public const int GWL_EXSTYLE = -20;

	public const uint WS_POPUP = 0x80000000;
	public const uint WS_EX_LAYERED = 0x00080000;
	public const uint WS_EX_TRANSPARENT = 0x00000020;
	public const uint WS_EX_TOOLWINDOW = 0x00000080;
	public const uint WS_EX_TOPMOST = 0x00000008;

	public const short SWP_NOMOVE = 0X2;
	public const short SWP_NOSIZE = 1;
	public const short SWP_NOZORDER = 0X4;
	public const int SWP_SHOWWINDOW = 0x0040;
	public const int SWP_NOACTIVATE = 0x0010;

	public const int SM_CYBORDER = 6;
	public const int SM_CYFRAME = 33;
	public const int SM_CYCAPTION = 4;
	public const int SM_CXPADDEDBORDER = 92;

	public const int SW_SHOWNOACTIVATE = 4;
	public const int SW_HIDE = 0;

	public const int WH_MOUSE = 7;
	public const int WH_MOUSE_LL = 14;
	public const int WH_KEYBOARD_LL = 13;

	public const int WM_KEYDOWN = 13;
	public const int WM_QUIT = 0x0012;

	public const uint WM_LBUTTONDOWN = 0x0201;
	public const uint WM_RBUTTONDOWN = 0x0204;

	public const int LLKHF_INJECTED = 0x00000010;

	[DllImport("user32.dll")]
	internal static extern uint SendInput(
	   uint nInputs,
	   [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs,
	   int cbSize);


	// Declare the INPUT struct
	[StructLayout(LayoutKind.Sequential)]
	public struct INPUT {
		public uint type;
		public InputUnion U;
		public static int Size {
			get { return Marshal.SizeOf(typeof(INPUT)); }
		}
	}

	// Declare the InputUnion struct
	[StructLayout(LayoutKind.Explicit)]
	public struct InputUnion {
		[FieldOffset(0)]
		internal MOUSEINPUT mi;
		[FieldOffset(0)]
		internal KEYBDINPUT ki;
		[FieldOffset(0)]
		internal HARDWAREINPUT hi;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MOUSEINPUT {
		internal int dx;
		internal int dy;
		internal MouseEventDataXButtons mouseData;
		internal MOUSEEVENTF dwFlags;
		internal uint time;
		internal UIntPtr dwExtraInfo;
	}

	[Flags]
	public enum MouseEventDataXButtons : uint {
		Nothing = 0x00000000,
		XBUTTON1 = 0x00000001,
		XBUTTON2 = 0x00000002
	}

	[Flags]
	public enum MOUSEEVENTF : uint {
		ABSOLUTE = 0x8000,
		HWHEEL = 0x01000,
		MOVE = 0x0001,
		MOVE_NOCOALESCE = 0x2000,
		LEFTDOWN = 0x0002,
		LEFTUP = 0x0004,
		RIGHTDOWN = 0x0008,
		RIGHTUP = 0x0010,
		MIDDLEDOWN = 0x0020,
		MIDDLEUP = 0x0040,
		VIRTUALDESK = 0x4000,
		WHEEL = 0x0800,
		XDOWN = 0x0080,
		XUP = 0x0100
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct KEYBDINPUT {
		internal VirtualKeyShort wVk;
		internal ScanCodeShort wScan;
		internal KEYEVENTF dwFlags;
		internal int time;
		internal UIntPtr dwExtraInfo;
	}

	[Flags]
	public enum KEYEVENTF : uint {
		EXTENDEDKEY = 0x0001,
		KEYUP = 0x0002,
		SCANCODE = 0x0008,
		UNICODE = 0x0004
	}

	public enum VirtualKeyShort : short {
		///<summary>
		///Left mouse button
		///</summary>
		LBUTTON = 0x01,
		///<summary>
		///Right mouse button
		///</summary>
		RBUTTON = 0x02,
		///<summary>
		///Control-break processing
		///</summary>
		CANCEL = 0x03,
		///<summary>
		///Middle mouse button (three-button mouse)
		///</summary>
		MBUTTON = 0x04,
		///<summary>
		///Windows 2000/XP: X1 mouse button
		///</summary>
		XBUTTON1 = 0x05,
		///<summary>
		///Windows 2000/XP: X2 mouse button
		///</summary>
		XBUTTON2 = 0x06,
		///<summary>
		///BACKSPACE key
		///</summary>
		BACK = 0x08,
		///<summary>
		///TAB key
		///</summary>
		TAB = 0x09,
		///<summary>
		///CLEAR key
		///</summary>
		CLEAR = 0x0C,
		///<summary>
		///ENTER key
		///</summary>
		RETURN = 0x0D,
		///<summary>
		///SHIFT key
		///</summary>
		SHIFT = 0x10,
		///<summary>
		///CTRL key
		///</summary>
		CONTROL = 0x11,
		///<summary>
		///ALT key
		///</summary>
		MENU = 0x12,
		///<summary>
		///PAUSE key
		///</summary>
		PAUSE = 0x13,
		///<summary>
		///CAPS LOCK key
		///</summary>
		CAPITAL = 0x14,
		///<summary>
		///Input Method Editor (IME) Kana mode
		///</summary>
		KANA = 0x15,
		///<summary>
		///IME Hangul mode
		///</summary>
		HANGUL = 0x15,
		///<summary>
		///IME Junja mode
		///</summary>
		JUNJA = 0x17,
		///<summary>
		///IME final mode
		///</summary>
		FINAL = 0x18,
		///<summary>
		///IME Hanja mode
		///</summary>
		HANJA = 0x19,
		///<summary>
		///IME Kanji mode
		///</summary>
		KANJI = 0x19,
		///<summary>
		///ESC key
		///</summary>
		ESCAPE = 0x1B,
		///<summary>
		///IME convert
		///</summary>
		CONVERT = 0x1C,
		///<summary>
		///IME nonconvert
		///</summary>
		NONCONVERT = 0x1D,
		///<summary>
		///IME accept
		///</summary>
		ACCEPT = 0x1E,
		///<summary>
		///IME mode change request
		///</summary>
		MODECHANGE = 0x1F,
		///<summary>
		///SPACEBAR
		///</summary>
		SPACE = 0x20,
		///<summary>
		///PAGE UP key
		///</summary>
		PRIOR = 0x21,
		///<summary>
		///PAGE DOWN key
		///</summary>
		NEXT = 0x22,
		///<summary>
		///END key
		///</summary>
		END = 0x23,
		///<summary>
		///HOME key
		///</summary>
		HOME = 0x24,
		///<summary>
		///LEFT ARROW key
		///</summary>
		LEFT = 0x25,
		///<summary>
		///UP ARROW key
		///</summary>
		UP = 0x26,
		///<summary>
		///RIGHT ARROW key
		///</summary>
		RIGHT = 0x27,
		///<summary>
		///DOWN ARROW key
		///</summary>
		DOWN = 0x28,
		///<summary>
		///SELECT key
		///</summary>
		SELECT = 0x29,
		///<summary>
		///PRINT key
		///</summary>
		PRINT = 0x2A,
		///<summary>
		///EXECUTE key
		///</summary>
		EXECUTE = 0x2B,
		///<summary>
		///PRINT SCREEN key
		///</summary>
		SNAPSHOT = 0x2C,
		///<summary>
		///INS key
		///</summary>
		INSERT = 0x2D,
		///<summary>
		///DEL key
		///</summary>
		DELETE = 0x2E,
		///<summary>
		///HELP key
		///</summary>
		HELP = 0x2F,
		///<summary>
		///0 key
		///</summary>
		KEY_0 = 0x30,
		///<summary>
		///1 key
		///</summary>
		KEY_1 = 0x31,
		///<summary>
		///2 key
		///</summary>
		KEY_2 = 0x32,
		///<summary>
		///3 key
		///</summary>
		KEY_3 = 0x33,
		///<summary>
		///4 key
		///</summary>
		KEY_4 = 0x34,
		///<summary>
		///5 key
		///</summary>
		KEY_5 = 0x35,
		///<summary>
		///6 key
		///</summary>
		KEY_6 = 0x36,
		///<summary>
		///7 key
		///</summary>
		KEY_7 = 0x37,
		///<summary>
		///8 key
		///</summary>
		KEY_8 = 0x38,
		///<summary>
		///9 key
		///</summary>
		KEY_9 = 0x39,
		///<summary>
		///A key
		///</summary>
		KEY_A = 0x41,
		///<summary>
		///B key
		///</summary>
		KEY_B = 0x42,
		///<summary>
		///C key
		///</summary>
		KEY_C = 0x43,
		///<summary>
		///D key
		///</summary>
		KEY_D = 0x44,
		///<summary>
		///E key
		///</summary>
		KEY_E = 0x45,
		///<summary>
		///F key
		///</summary>
		KEY_F = 0x46,
		///<summary>
		///G key
		///</summary>
		KEY_G = 0x47,
		///<summary>
		///H key
		///</summary>
		KEY_H = 0x48,
		///<summary>
		///I key
		///</summary>
		KEY_I = 0x49,
		///<summary>
		///J key
		///</summary>
		KEY_J = 0x4A,
		///<summary>
		///K key
		///</summary>
		KEY_K = 0x4B,
		///<summary>
		///L key
		///</summary>
		KEY_L = 0x4C,
		///<summary>
		///M key
		///</summary>
		KEY_M = 0x4D,
		///<summary>
		///N key
		///</summary>
		KEY_N = 0x4E,
		///<summary>
		///O key
		///</summary>
		KEY_O = 0x4F,
		///<summary>
		///P key
		///</summary>
		KEY_P = 0x50,
		///<summary>
		///Q key
		///</summary>
		KEY_Q = 0x51,
		///<summary>
		///R key
		///</summary>
		KEY_R = 0x52,
		///<summary>
		///S key
		///</summary>
		KEY_S = 0x53,
		///<summary>
		///T key
		///</summary>
		KEY_T = 0x54,
		///<summary>
		///U key
		///</summary>
		KEY_U = 0x55,
		///<summary>
		///V key
		///</summary>
		KEY_V = 0x56,
		///<summary>
		///W key
		///</summary>
		KEY_W = 0x57,
		///<summary>
		///X key
		///</summary>
		KEY_X = 0x58,
		///<summary>
		///Y key
		///</summary>
		KEY_Y = 0x59,
		///<summary>
		///Z key
		///</summary>
		KEY_Z = 0x5A,
		///<summary>
		///Left Windows key (Microsoft Natural keyboard) 
		///</summary>
		LWIN = 0x5B,
		///<summary>
		///Right Windows key (Natural keyboard)
		///</summary>
		RWIN = 0x5C,
		///<summary>
		///Applications key (Natural keyboard)
		///</summary>
		APPS = 0x5D,
		///<summary>
		///Computer Sleep key
		///</summary>
		SLEEP = 0x5F,
		///<summary>
		///Numeric keypad 0 key
		///</summary>
		NUMPAD0 = 0x60,
		///<summary>
		///Numeric keypad 1 key
		///</summary>
		NUMPAD1 = 0x61,
		///<summary>
		///Numeric keypad 2 key
		///</summary>
		NUMPAD2 = 0x62,
		///<summary>
		///Numeric keypad 3 key
		///</summary>
		NUMPAD3 = 0x63,
		///<summary>
		///Numeric keypad 4 key
		///</summary>
		NUMPAD4 = 0x64,
		///<summary>
		///Numeric keypad 5 key
		///</summary>
		NUMPAD5 = 0x65,
		///<summary>
		///Numeric keypad 6 key
		///</summary>
		NUMPAD6 = 0x66,
		///<summary>
		///Numeric keypad 7 key
		///</summary>
		NUMPAD7 = 0x67,
		///<summary>
		///Numeric keypad 8 key
		///</summary>
		NUMPAD8 = 0x68,
		///<summary>
		///Numeric keypad 9 key
		///</summary>
		NUMPAD9 = 0x69,
		///<summary>
		///Multiply key
		///</summary>
		MULTIPLY = 0x6A,
		///<summary>
		///Add key
		///</summary>
		ADD = 0x6B,
		///<summary>
		///Separator key
		///</summary>
		SEPARATOR = 0x6C,
		///<summary>
		///Subtract key
		///</summary>
		SUBTRACT = 0x6D,
		///<summary>
		///Decimal key
		///</summary>
		DECIMAL = 0x6E,
		///<summary>
		///Divide key
		///</summary>
		DIVIDE = 0x6F,
		///<summary>
		///F1 key
		///</summary>
		F1 = 0x70,
		///<summary>
		///F2 key
		///</summary>
		F2 = 0x71,
		///<summary>
		///F3 key
		///</summary>
		F3 = 0x72,
		///<summary>
		///F4 key
		///</summary>
		F4 = 0x73,
		///<summary>
		///F5 key
		///</summary>
		F5 = 0x74,
		///<summary>
		///F6 key
		///</summary>
		F6 = 0x75,
		///<summary>
		///F7 key
		///</summary>
		F7 = 0x76,
		///<summary>
		///F8 key
		///</summary>
		F8 = 0x77,
		///<summary>
		///F9 key
		///</summary>
		F9 = 0x78,
		///<summary>
		///F10 key
		///</summary>
		F10 = 0x79,
		///<summary>
		///F11 key
		///</summary>
		F11 = 0x7A,
		///<summary>
		///F12 key
		///</summary>
		F12 = 0x7B,
		///<summary>
		///F13 key
		///</summary>
		F13 = 0x7C,
		///<summary>
		///F14 key
		///</summary>
		F14 = 0x7D,
		///<summary>
		///F15 key
		///</summary>
		F15 = 0x7E,
		///<summary>
		///F16 key
		///</summary>
		F16 = 0x7F,
		///<summary>
		///F17 key  
		///</summary>
		F17 = 0x80,
		///<summary>
		///F18 key  
		///</summary>
		F18 = 0x81,
		///<summary>
		///F19 key  
		///</summary>
		F19 = 0x82,
		///<summary>
		///F20 key  
		///</summary>
		F20 = 0x83,
		///<summary>
		///F21 key  
		///</summary>
		F21 = 0x84,
		///<summary>
		///F22 key, (PPC only) Key used to lock device.
		///</summary>
		F22 = 0x85,
		///<summary>
		///F23 key  
		///</summary>
		F23 = 0x86,
		///<summary>
		///F24 key  
		///</summary>
		F24 = 0x87,
		///<summary>
		///NUM LOCK key
		///</summary>
		NUMLOCK = 0x90,
		///<summary>
		///SCROLL LOCK key
		///</summary>
		SCROLL = 0x91,
		///<summary>
		///Left SHIFT key
		///</summary>
		LSHIFT = 0xA0,
		///<summary>
		///Right SHIFT key
		///</summary>
		RSHIFT = 0xA1,
		///<summary>
		///Left CONTROL key
		///</summary>
		LCONTROL = 0xA2,
		///<summary>
		///Right CONTROL key
		///</summary>
		RCONTROL = 0xA3,
		///<summary>
		///Left MENU key
		///</summary>
		LMENU = 0xA4,
		///<summary>
		///Right MENU key
		///</summary>
		RMENU = 0xA5,
		///<summary>
		///Windows 2000/XP: Browser Back key
		///</summary>
		BROWSER_BACK = 0xA6,
		///<summary>
		///Windows 2000/XP: Browser Forward key
		///</summary>
		BROWSER_FORWARD = 0xA7,
		///<summary>
		///Windows 2000/XP: Browser Refresh key
		///</summary>
		BROWSER_REFRESH = 0xA8,
		///<summary>
		///Windows 2000/XP: Browser Stop key
		///</summary>
		BROWSER_STOP = 0xA9,
		///<summary>
		///Windows 2000/XP: Browser Search key 
		///</summary>
		BROWSER_SEARCH = 0xAA,
		///<summary>
		///Windows 2000/XP: Browser Favorites key
		///</summary>
		BROWSER_FAVORITES = 0xAB,
		///<summary>
		///Windows 2000/XP: Browser Start and Home key
		///</summary>
		BROWSER_HOME = 0xAC,
		///<summary>
		///Windows 2000/XP: Volume Mute key
		///</summary>
		VOLUME_MUTE = 0xAD,
		///<summary>
		///Windows 2000/XP: Volume Down key
		///</summary>
		VOLUME_DOWN = 0xAE,
		///<summary>
		///Windows 2000/XP: Volume Up key
		///</summary>
		VOLUME_UP = 0xAF,
		///<summary>
		///Windows 2000/XP: Next Track key
		///</summary>
		MEDIA_NEXT_TRACK = 0xB0,
		///<summary>
		///Windows 2000/XP: Previous Track key
		///</summary>
		MEDIA_PREV_TRACK = 0xB1,
		///<summary>
		///Windows 2000/XP: Stop Media key
		///</summary>
		MEDIA_STOP = 0xB2,
		///<summary>
		///Windows 2000/XP: Play/Pause Media key
		///</summary>
		MEDIA_PLAY_PAUSE = 0xB3,
		///<summary>
		///Windows 2000/XP: Start Mail key
		///</summary>
		LAUNCH_MAIL = 0xB4,
		///<summary>
		///Windows 2000/XP: Select Media key
		///</summary>
		LAUNCH_MEDIA_SELECT = 0xB5,
		///<summary>
		///Windows 2000/XP: Start Application 1 key
		///</summary>
		LAUNCH_APP1 = 0xB6,
		///<summary>
		///Windows 2000/XP: Start Application 2 key
		///</summary>
		LAUNCH_APP2 = 0xB7,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard.
		///</summary>
		OEM_1 = 0xBA,
		///<summary>
		///Windows 2000/XP: For any country/region, the '+' key
		///</summary>
		OEM_PLUS = 0xBB,
		///<summary>
		///Windows 2000/XP: For any country/region, the ',' key
		///</summary>
		OEM_COMMA = 0xBC,
		///<summary>
		///Windows 2000/XP: For any country/region, the '-' key
		///</summary>
		OEM_MINUS = 0xBD,
		///<summary>
		///Windows 2000/XP: For any country/region, the '.' key
		///</summary>
		OEM_PERIOD = 0xBE,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard.
		///</summary>
		OEM_2 = 0xBF,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard. 
		///</summary>
		OEM_3 = 0xC0,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard. 
		///</summary>
		OEM_4 = 0xDB,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard. 
		///</summary>
		OEM_5 = 0xDC,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard. 
		///</summary>
		OEM_6 = 0xDD,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard. 
		///</summary>
		OEM_7 = 0xDE,
		///<summary>
		///Used for miscellaneous characters; it can vary by keyboard.
		///</summary>
		OEM_8 = 0xDF,
		///<summary>
		///Windows 2000/XP: Either the angle bracket key or the backslash key on the RT 102-key keyboard
		///</summary>
		OEM_102 = 0xE2,
		///<summary>
		///Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME PROCESS key
		///</summary>
		PROCESSKEY = 0xE5,
		///<summary>
		///Windows 2000/XP: Used to pass Unicode characters as if they were keystrokes.
		///The VK_PACKET key is the low word of a 32-bit Virtual Key value used for non-keyboard input methods. For more information,
		///see Remark in KEYBDINPUT, SendInput, WM_KEYDOWN, and WM_KEYUP
		///</summary>
		PACKET = 0xE7,
		///<summary>
		///Attn key
		///</summary>
		ATTN = 0xF6,
		///<summary>
		///CrSel key
		///</summary>
		CRSEL = 0xF7,
		///<summary>
		///ExSel key
		///</summary>
		EXSEL = 0xF8,
		///<summary>
		///Erase EOF key
		///</summary>
		EREOF = 0xF9,
		///<summary>
		///Play key
		///</summary>
		PLAY = 0xFA,
		///<summary>
		///Zoom key
		///</summary>
		ZOOM = 0xFB,
		///<summary>
		///Reserved 
		///</summary>
		NONAME = 0xFC,
		///<summary>
		///PA1 key
		///</summary>
		PA1 = 0xFD,
		///<summary>
		///Clear key
		///</summary>
		OEM_CLEAR = 0xFE
	}

	public enum ScanCodeShort : short {
		LBUTTON = 0,
		RBUTTON = 0,
		CANCEL = 70,
		MBUTTON = 0,
		XBUTTON1 = 0,
		XBUTTON2 = 0,
		BACK = 14,
		TAB = 15,
		CLEAR = 76,
		RETURN = 28,
		SHIFT = 42,
		CONTROL = 29,
		MENU = 56,
		PAUSE = 0,
		CAPITAL = 58,
		KANA = 0,
		HANGUL = 0,
		JUNJA = 0,
		FINAL = 0,
		HANJA = 0,
		KANJI = 0,
		ESCAPE = 1,
		CONVERT = 0,
		NONCONVERT = 0,
		ACCEPT = 0,
		MODECHANGE = 0,
		SPACE = 57,
		PRIOR = 73,
		NEXT = 81,
		END = 79,
		HOME = 71,
		LEFT = 75,
		UP = 72,
		RIGHT = 77,
		DOWN = 80,
		SELECT = 0,
		PRINT = 0,
		EXECUTE = 0,
		SNAPSHOT = 84,
		INSERT = 82,
		DELETE = 83,
		HELP = 99,
		KEY_0 = 11,
		KEY_1 = 2,
		KEY_2 = 3,
		KEY_3 = 4,
		KEY_4 = 5,
		KEY_5 = 6,
		KEY_6 = 7,
		KEY_7 = 8,
		KEY_8 = 9,
		KEY_9 = 10,
		KEY_A = 30,
		KEY_B = 48,
		KEY_C = 46,
		KEY_D = 32,
		KEY_E = 18,
		KEY_F = 33,
		KEY_G = 34,
		KEY_H = 35,
		KEY_I = 23,
		KEY_J = 36,
		KEY_K = 37,
		KEY_L = 38,
		KEY_M = 50,
		KEY_N = 49,
		KEY_O = 24,
		KEY_P = 25,
		KEY_Q = 16,
		KEY_R = 19,
		KEY_S = 31,
		KEY_T = 20,
		KEY_U = 22,
		KEY_V = 47,
		KEY_W = 17,
		KEY_X = 45,
		KEY_Y = 21,
		KEY_Z = 44,
		LWIN = 91,
		RWIN = 92,
		APPS = 93,
		SLEEP = 95,
		NUMPAD0 = 82,
		NUMPAD1 = 79,
		NUMPAD2 = 80,
		NUMPAD3 = 81,
		NUMPAD4 = 75,
		NUMPAD5 = 76,
		NUMPAD6 = 77,
		NUMPAD7 = 71,
		NUMPAD8 = 72,
		NUMPAD9 = 73,
		MULTIPLY = 55,
		ADD = 78,
		SEPARATOR = 0,
		SUBTRACT = 74,
		DECIMAL = 83,
		DIVIDE = 53,
		F1 = 59,
		F2 = 60,
		F3 = 61,
		F4 = 62,
		F5 = 63,
		F6 = 64,
		F7 = 65,
		F8 = 66,
		F9 = 67,
		F10 = 68,
		F11 = 87,
		F12 = 88,
		F13 = 100,
		F14 = 101,
		F15 = 102,
		F16 = 103,
		F17 = 104,
		F18 = 105,
		F19 = 106,
		F20 = 107,
		F21 = 108,
		F22 = 109,
		F23 = 110,
		F24 = 118,
		NUMLOCK = 69,
		SCROLL = 70,
		LSHIFT = 42,
		RSHIFT = 54,
		LCONTROL = 29,
		RCONTROL = 29,
		LMENU = 56,
		RMENU = 56,
		BROWSER_BACK = 106,
		BROWSER_FORWARD = 105,
		BROWSER_REFRESH = 103,
		BROWSER_STOP = 104,
		BROWSER_SEARCH = 101,
		BROWSER_FAVORITES = 102,
		BROWSER_HOME = 50,
		VOLUME_MUTE = 32,
		VOLUME_DOWN = 46,
		VOLUME_UP = 48,
		MEDIA_NEXT_TRACK = 25,
		MEDIA_PREV_TRACK = 16,
		MEDIA_STOP = 36,
		MEDIA_PLAY_PAUSE = 34,
		LAUNCH_MAIL = 108,
		LAUNCH_MEDIA_SELECT = 109,
		LAUNCH_APP1 = 107,
		LAUNCH_APP2 = 33,
		OEM_1 = 39,
		OEM_PLUS = 13,
		OEM_COMMA = 51,
		OEM_MINUS = 12,
		OEM_PERIOD = 52,
		OEM_2 = 53,
		OEM_3 = 41,
		OEM_4 = 26,
		OEM_5 = 43,
		OEM_6 = 27,
		OEM_7 = 40,
		OEM_8 = 0,
		OEM_102 = 86,
		PROCESSKEY = 0,
		PACKET = 0,
		ATTN = 0,
		CRSEL = 0,
		EXSEL = 0,
		EREOF = 93,
		PLAY = 0,
		ZOOM = 98,
		NONAME = 0,
		PA1 = 0,
		OEM_CLEAR = 0,
	}

	/// <summary>
	/// Define HARDWAREINPUT struct
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct HARDWAREINPUT {
		internal int uMsg;
		internal short wParamL;
		internal short wParamH;
	}

}

