using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Insomnia
{
	[Flags]
	public enum EXECUTION_STATE : uint
	{
		ES_AWAYMODE_REQUIRED = 0x00000040,
		ES_CONTINUOUS = 0x80000000,
		ES_DISPLAY_REQUIRED = 0x00000002,
		ES_SYSTEM_REQUIRED = 0x00000001
	}

	public class MyCustomApplicationContext : ApplicationContext
	{
		private NotifyIcon icon;

		public MyCustomApplicationContext()
		{
			var menu = new ContextMenu();
			var exitItem = new MenuItem
			{
				Text = "Exit"
			};
			exitItem.Click += ExitItem_Click;
			menu.MenuItems.Add(exitItem);

			icon = new NotifyIcon
			{
				Icon = Resource1.TinyNinja,
				Visible = true,
				ContextMenu = menu
			};

			SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
		}

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

		private void ExitItem_Click(object sender, EventArgs e)
		{
			SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
			icon.Visible = false;
			icon.Dispose();
			Application.Exit();
		}
	}

	internal static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MyCustomApplicationContext());
		}
	}
}