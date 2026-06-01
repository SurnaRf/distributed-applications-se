using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Amigurumemi.Data.Entities;

namespace Amigurumemi.Data
{
	public class AmigurumemiDbContext : DbContext, IDbContext
	{
		public AmigurumemiDbContext()
		{
		}
		public AmigurumemiDbContext(DbContextOptions<AmigurumemiDbContext> options) : base(options)
		{
		}

		public DbSet<Project> Projects { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Pattern> Patterns { get; set; }

		public DbSet<Yarn> Yarns { get; set; }

		public DbSet<ProjectYarn> ProjectYarns { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Project>()
				.HasOne<User>()
				.WithMany()
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Project>()
				.HasOne<Pattern>()
				.WithMany()
				.HasForeignKey(p => p.PatternId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<ProjectYarn>()
				.HasKey(py => new { py.ProjectId, py.YarnId }); 

			modelBuilder.Entity<Pattern>()
						.Property(p => p.Price)
						.HasPrecision(18, 2);

			modelBuilder.Entity<Project>()
				.Property(p => p.EstimatedCost)
				.HasPrecision(18, 2);

			modelBuilder.Entity<Yarn>()
				.Property(y => y.Price)
				.HasPrecision(18, 2);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Server=DESKTOP-F3IKLD2;Database=AmigurumemiDb;Trusted_Connection=True;TrustServerCertificate=True");
			}
		}
	}
}
