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
using OpenAI.Chat;
using System.Collections.ObjectModel;
using ChatWPF.Entity;
using Microsoft.EntityFrameworkCore;

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
			system = new ChatPrompt("system", "You are Donald Trump");
			Chat.ItemsSource = chat;
			context = new();
			context.History.Load();
			context.Messages.Load();
			context.Messages.Local.ToObservableCollection();
		}

		public OpenAIClient client;
		public ChatPrompt system;
		public ObservableCollection<string> chat = new();
		public Context context;

		private void UpdateChat(string message, int index = -1)
		{
			Dispatcher.Invoke(() =>
			{
				if (index == -1)
				{
					chat.Add(message);
				}
				else
				{
					chat[index] = message;
					Chat.Items.Refresh();
				}
			});
		}

		private void Send_Click(object sender, RoutedEventArgs e)
		{

			var chatPrompts = new List<ChatPrompt>
			{
				system,
				new ChatPrompt("user",InputPrompt.Text),
			};
			var chatRequest = new ChatRequest(chatPrompts);

			chat.Add(InputPrompt.Text);
			Chat.Items.Refresh();
			InputPrompt.Text = "";

			StringBuilder response = new();
			chat.Add(response.ToString());

			int index = chat.Count-1;
			client.ChatEndpoint.StreamCompletionAsync(chatRequest, (result) =>
			{
				response.Append(result.FirstChoice);
				UpdateChat(response.ToString(), index);
			});

		}
	}
}
