using apiAEE.Entities;
using Microsoft.EntityFrameworkCore;

namespace apiAEE.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Pertence> Pertences { get; set; }
        public DbSet<Evento> Eventos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da tabela associativa "Pertence"
            modelBuilder.Entity<Pertence>()
                .HasKey(p => new { p.CodEquipe, p.CodUsuario });

            modelBuilder.Entity<Pertence>()
                .HasOne(p => p.Equipe)
                .WithMany(e => e.Pertences)
                .HasForeignKey(p => p.CodEquipe)
                .OnDelete(DeleteBehavior.Restrict); // Evita cascatas desnecessárias

            modelBuilder.Entity<Pertence>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pertences)
                .HasForeignKey(p => p.CodUsuario)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação entre Equipe e Usuario (CriadorId)
            modelBuilder.Entity<Equipe>()
                .HasOne(e => e.Criador)
                .WithMany() // Não cria coleção em Usuario para evitar confusão
                .HasForeignKey(e => e.CriadorId)
                .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

            base.OnModelCreating(modelBuilder);
        }
    }
}
