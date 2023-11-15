using KeyboardClicker.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using static KeyboardClicker.Common.Win32;

namespace KeyboardClicker;

public class Clicker {
	public static readonly HashSet<int> skillScanCodes = new();

	private IntPtr HookPtr = IntPtr.Zero;
	private LowLevelWindowsHookProc KeyboardProc;

	private int previousScanCode = -1;
	private Thread repeaterThread;
	private object keepRepeaterLock = new();
	private bool _keepRepeater = true;
	private bool KeepRepeater {
		get {
			lock (keepRepeaterLock) {
				return _keepRepeater;
			}
		}

		set {
			lock (keepRepeaterLock) {
				_keepRepeater = value;
			}
		}
	}

	private object keyPressedLock = new();
	private Dictionary<int, DateTime?> keyPressed = new();

	static Clicker() {
		skillScanCodes.Add((int) ScanCodeShort.KEY_Q);
		skillScanCodes.Add((int) ScanCodeShort.KEY_W);
		skillScanCodes.Add((int) ScanCodeShort.KEY_E);
		skillScanCodes.Add((int) ScanCodeShort.KEY_R);
		skillScanCodes.Add((int) ScanCodeShort.KEY_D);
		skillScanCodes.Add((int) ScanCodeShort.KEY_F);
	}

	public Clicker() {


		foreach (var scanCode in skillScanCodes) {
			keyPressed[scanCode] = null;
		}
	}

	public void Start() {
		Hook();
	}

	public void Stop() {
		Unhook();
	}


	private void Hook() {
		KeyboardProc = new Win32.LowLevelWindowsHookProc(KeyboardHookProc);
		// Win32.LoadLibrary("user32")
		HookPtr = Win32.SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardProc, IntPtr.Zero, 0);
		if (HookPtr == IntPtr.Zero) {
			throw new System.ComponentModel.Win32Exception();
		}
		repeaterThread = new Thread(Repeater);
		//repeaterThread.Priority = ThreadPriority.Highest;
		repeaterThread.Start();
	}

	private void Unhook() {
		if (HookPtr != IntPtr.Zero) {
			KeepRepeater = false;
			repeaterThread.Join();
			bool result = UnhookWindowsHookEx(HookPtr);
		}
	}

	private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
		Win32.KbdLLHookStruct kbStruct = (Win32.KbdLLHookStruct) Marshal.PtrToStructure(lParam, typeof(Win32.KbdLLHookStruct));
		if (nCode >= 0 && (kbStruct.flags & Win32.LLKHF_INJECTED) != Win32.LLKHF_INJECTED) {
			bool handled = OnKeyboardClick(kbStruct);
			if (handled)
				return (IntPtr) 1;

		}
		return Win32.CallNextHookEx(HookPtr, nCode, wParam, lParam);
	}

	private bool OnKeyboardClick(KbdLLHookStruct kbStruct) {
		// 81 - q
		// 87 - w
		// 69 - e
		// 82 - r
		// 68 - d
		// 70 - f

		//if (!app.GameWindow.IsForeground()) {
		//	return false;
		//}
		lock (skillScanCodes) {
			if (!skillScanCodes.Contains(kbStruct.scanCode))
				return false;
		}

	var isDown = (kbStruct.flags >> 7 & 1) != 1;
		if (previousScanCode == kbStruct.scanCode && isDown)
			return true;
		if (isDown) {
			lock (keyPressedLock) {
				keyPressed[kbStruct.scanCode] = DateTime.Now;
			}
			SendKeyboard((ScanCodeShort) kbStruct.scanCode);
			previousScanCode = kbStruct.scanCode;
			return true;
		} else {
			lock (keyPressedLock) {
				keyPressed[kbStruct.scanCode] = null;
			}
			previousScanCode = -1;
		}
		return true;
	}

	private void SendKeyboardDown(ScanCodeShort a) {
		INPUT[] Inputs = new INPUT[1];
		INPUT Input = new INPUT();
		Input.type = 1; // 1 = Keyboard Input
		Input.U.ki.wScan = a;
		Input.U.ki.dwFlags = KEYEVENTF.SCANCODE;
		Inputs[0] = Input;
		SendInput(1, Inputs, INPUT.Size);
	}

	private void SendKeyboardUp(ScanCodeShort a) {
		INPUT[] Inputs = new INPUT[1];
		INPUT Input = new INPUT();
		Input.type = 1; // 1 = Keyboard Input
		Input.U.ki.wScan = a;
		Input.U.ki.dwFlags = KEYEVENTF.KEYUP | KEYEVENTF.SCANCODE;
		Inputs[0] = Input;
		SendInput(1, Inputs, INPUT.Size);
	}

	private void Repeater() {
		while (KeepRepeater) {
			List<int> keys = new();
			lock (keyPressedLock) {
				foreach (KeyValuePair<int, DateTime?> entry in keyPressed) {
					if (entry.Value.HasValue && (DateTime.Now - entry.Value.Value).Milliseconds >= 200) {
						keys.Add(entry.Key);
					}
				}
			}
			foreach (var scanCode in keys) {
				SendKeyboard((ScanCodeShort) scanCode);
			}
			Thread.Sleep(30);
		}
	}

	private void SendKeyboard(ScanCodeShort scanCode) {
		SendKeyboardDown(scanCode);
		SendKeyboardUp(scanCode);
	}
}

public enum Skill {
	Q, W, E, R, D, F
}