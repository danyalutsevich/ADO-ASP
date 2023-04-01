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
using OpenAI.Chat;

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
			string? ORGANIZATION_ID = Environment.GetEnvironmentVariable("ORGANIZATION_ID");
			if (OPENAI_API_KEY == null || ORGANIZATION_ID == null)
			{
				MessageBox.Show("Please set your OPENAI_API_KEY and ORGANIZATION_ID in the .env file");
				return;
			}
			OpenAIAuthentication auth = new(OPENAI_API_KEY, ORGANIZATION_ID);
			client = new(auth);
			system = new ChatPrompt("system", "You are a Donald Trump");
		}
		
		public OpenAIClient client;
		public ChatPrompt system;

		private void Send_Click(object sender, RoutedEventArgs e)
		{
			var chatPrompts = new List<ChatPrompt>
			{
				system,
				new ChatPrompt("user",InputPrompt.Text),
			};
			var chatRequest = new ChatRequest(chatPrompts);
			var result = client.ChatEndpoint.GetCompletionAsync(chatRequest).Result;
			MessageBox.Show(result.FirstChoice);
		}
	}
}
