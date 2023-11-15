using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KeyboardClicker {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private readonly Clicker _keyboardClicker;

		public Clicker KeyboardClicker { get { return _keyboardClicker; } }

		public App() {
			_keyboardClicker = new Clicker();
		}

		protected override void OnStartup(StartupEventArgs e) {
			_keyboardClicker.Start();
		}

		protected override void OnExit(ExitEventArgs e) {
			base.OnExit(e);
			_keyboardClicker.Stop();
		}
	}

}
