using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenAI;
using DotNetEnv;

namespace ChatWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			DotNetEnv.Env.Load();
			string? OPENAI_API_KEY = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
			string? ORGANIZATION_ID = Environment.GetEnvironmentVariable("ORGANIZATIdON_ID");
			if (OPENAI_API_KEY == null || ORGANIZATION_ID == null)
			{
				MessageBox.Show("Please set your OPENAI_API_KEY and ORGANIZATION_ID in the .env file");
				return;
			}
			MessageBox.Show(ORGANIZATION_ID + OPENAI_API_KEY);
			//OpenAIAuthentication auth = new(OPENAI_API_KEY,ORGANIZATION_ID); 
		}

		private void Send_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}
