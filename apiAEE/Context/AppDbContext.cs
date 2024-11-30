using ApiAEE.Entities;
using Microsoft.EntityFrameworkCore;

namespace apiAEE.Context
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ }
		public DbSet<Usuario> Usuarios { get; set; }
	}
}
