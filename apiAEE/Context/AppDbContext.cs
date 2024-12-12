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

        public DbSet<Cadastrar> Cadastras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração da tabela associativa "Pertence"
            modelBuilder.Entity<Pertence>()
                .HasKey(p => new { p.CodEquipe, p.ID});

            modelBuilder.Entity<Pertence>()
                .HasOne(p => p.Equipe)
                .WithMany(e => e.Pertences)
                .HasForeignKey(p => p.CodEquipe)
                .OnDelete(DeleteBehavior.Restrict); // Evita cascatas desnecessárias

            modelBuilder.Entity<Pertence>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.Pertences)
                .HasForeignKey(p => p.ID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configurar a tabela intermediária "Cadastrar"
            modelBuilder.Entity<Cadastrar>()
                .HasKey(c => new { c.CodEquipe, c.CodEvento }); // Definir chave composta

            modelBuilder.Entity<Cadastrar>()
                .HasOne(c => c.Equipe) // Relacionamento com "Equipe"
                .WithMany(e => e.Cadastrar) // Uma equipe pode estar em várias inscrições
                .HasForeignKey(c => c.CodEquipe) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Comportamento de exclusão em cascata

            modelBuilder.Entity<Cadastrar>()
                .HasOne(c => c.Evento) // Relacionamento com "Evento"
                .WithMany(e => e.Cadastrar) // Um evento pode ter várias equipes inscritas
                .HasForeignKey(c => c.CodEvento) // Chave estrangeira
                .OnDelete(DeleteBehavior.Cascade); // Comportamento de exclusão em cascat

        modelBuilder.Entity<Usuario>().ToTable("Usuarios");


            base.OnModelCreating(modelBuilder);
        }
    }
}
