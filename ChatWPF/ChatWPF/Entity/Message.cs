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
	}
}
