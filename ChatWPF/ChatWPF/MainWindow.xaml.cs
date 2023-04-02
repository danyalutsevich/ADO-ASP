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
			Chat.ItemsSource = localMessages;
			context = new();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			context.Chats.Load();
			context.Messages.Load();
			currentChat = new Chat(context.Chats.Local.Count);
			context.Chats.Add(currentChat);
			context.SaveChanges();
			History.ItemsSource = context.Chats.Local.ToObservableCollection();
		}

		public OpenAIClient client;
		public ChatPrompt system;
		public Chat currentChat;
		public ObservableCollection<string> chat = new();
		public ObservableCollection<Entity.Message> localMessages = new();
		public Context context;


		private void UpdateMessage(string message, int index)
		{
			Dispatcher.Invoke(() =>
			{
				localMessages[index].message = message;
				Chat.Items.Refresh();
			});
		}

		private List<ChatPrompt> GetChatPrompts()
		{
			var prompts = localMessages.Select(m => new ChatPrompt(m.from, m.message)).ToList();
			prompts.Insert(0, system);
			return prompts;
		}

		private void Send_Click(object sender, RoutedEventArgs e)
		{

			// add user's message and clear input
			var usersMessage = new Entity.Message(currentChat.Id, localMessages.Count, "user", InputPrompt.Text);
			// we need two storages of messages because we cant set the DbSet as an itemSource 
			// so the localMessages is for view and context.Messages is for storing the messages
			localMessages.Add(usersMessage);
			context.Messages.Add(usersMessage);
			Chat.Items.Refresh();
			InputPrompt.Text = "";

			var chatPrompts = GetChatPrompts();
			var chatRequest = new ChatRequest(chatPrompts);

			// add response that will be updated as responses will come
			StringBuilder response = new();
			int index = localMessages.Count;
			var message = new Entity.Message(currentChat.Id, index, "assistant", response.ToString());
			localMessages.Add(message);

			client.ChatEndpoint.StreamCompletionAsync(chatRequest, (result) =>
			{
				response.Append(result.FirstChoice);
				UpdateMessage(response.ToString(), index);
			})
			.ContinueWith((res) =>
			{
				// when the message is complete adding it to the store (database)
				context.Messages.Add(message);
				context.SaveChanges();
			});

		}

		private void History_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var chat = e.AddedItems[0] as Chat;
			localMessages.Clear();
			foreach (var m in context.Messages.Where(m => m.ChatId == chat.Id))
			{
				localMessages.Add(m);
			}
			currentChat = chat;
			Chat.Items.Refresh();

		}
	}
}
