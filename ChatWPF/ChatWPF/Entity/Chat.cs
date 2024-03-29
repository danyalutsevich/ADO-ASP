﻿using Sprache;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF.Entity
{
	[Table("Chat")]
	public class Chat
	{
		[Key]
		public Guid Id { get; set; }
		public int index { get; set; }

		public Chat(int index)
		{
			Id = Guid.NewGuid();
			this.index = index;
		}

		public override string ToString()
		{
			int maxCharacters = 40;
			var message = Messages?.Last()?.ToString();
			return (message?.Substring(0, Math.Min(maxCharacters, message.Length)) ?? "No messages") + "...";
		}

		public List<Message> Messages { get; set; }

	}
}
