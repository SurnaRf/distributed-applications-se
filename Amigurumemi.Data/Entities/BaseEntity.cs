using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Amigurumemi.Data.Entities
{
	public class BaseEntity
	{
		[Key]
		public int Id { get; set; }

		public Guid CreatedBy { get; set; }

		public DateTime CreatedOn { get; set; }

		public Guid UpdateBy { get; set; }

		public DateTime UpdatedOn { get; set; }
	}
}
