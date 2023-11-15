using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using static KeyboardClicker.Common.Win32;

namespace KeyboardClicker {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {

		private readonly Clicker _keyboardClicker;

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool AllocConsole();


		public MainWindow() {
			_keyboardClicker = ((App) Application.Current).KeyboardClicker;
			//AllocConsole();
			InitializeComponent();
		}

		public void EnableShortcut(ScanCodeShort code) {
			lock (Clicker.skillScanCodes) {
				Console.WriteLine($"{code} enable");
				Clicker.skillScanCodes.Add((int) code);
			}
		}

		public void DisableShortcut(ScanCodeShort code) {
			lock (Clicker.skillScanCodes) {
				Console.WriteLine($"{code} disable");
				Clicker.skillScanCodes.Remove((int) code);
			}
		}

		public void ToggleSkillCheckBox(CheckBox c, ScanCodeShort code) {
			if (c.IsChecked.HasValue && c.IsChecked.Value) {
				EnableShortcut(code);
			} else {
				DisableShortcut(code);
			}
		}

		private void QCheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_Q);
		}

		private void WCheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_W);
		}

		private void ECheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_E);
		}

		private void RCheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_R);
		}

		private void DCheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_D);
		}

		private void FCheckBox_Checked(object sender, RoutedEventArgs e) {
			EnableShortcut(ScanCodeShort.KEY_F);
		}

		private void QCheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_Q);
		}

		private void WCheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_W);
		}

		private void ECheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_E);
		}

		private void RCheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_R);
		}

		private void DCheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_D);
		}

		private void FCheckBox_Unchecked(object sender, RoutedEventArgs e) {
			DisableShortcut(ScanCodeShort.KEY_F);
		}
	}
}
