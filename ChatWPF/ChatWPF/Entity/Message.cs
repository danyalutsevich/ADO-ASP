using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF.Entity
{
	[Table("Message")]
	public class Message
	{
		[Key]
		public Guid Id { get; set; }
		public Guid ChatId { get; set; }
		public int index { get; set; }
		public string? from { get; set; }
		public string? message { get; set; }

		public Message(Guid ChatId, int index, string? from, string? message)
		{
			this.Id = Guid.NewGuid();
			this.ChatId = ChatId;
			this.index = index;
			this.from = from;
			this.message = message;
		}

		public override string ToString()
		{
			return message;
		}

		public Chat Chat { get; set; }

	}
}
