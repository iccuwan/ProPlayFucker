using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProPlayFucker
{
	class FuckerContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=db.db");
	}
}
