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
	}
}
