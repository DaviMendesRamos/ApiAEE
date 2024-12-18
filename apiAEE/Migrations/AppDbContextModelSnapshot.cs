﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using apiAEE.Context;

#nullable disable

namespace apiAEE.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Senha")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefone")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("UrlImagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("apiAEE.Entities.Amistoso", b =>
                {
                    b.Property<int>("CodEquipe")
                        .HasColumnType("int");

                    b.Property<int>("CodPartida")
                        .HasColumnType("int");

                    b.HasKey("CodEquipe", "CodPartida");

                    b.HasIndex("CodPartida");

                    b.ToTable("Amistososos");
                });

            modelBuilder.Entity("apiAEE.Entities.Cadastrar", b =>
                {
                    b.Property<int>("CodEquipe")
                        .HasColumnType("int");

                    b.Property<int>("CodEvento")
                        .HasColumnType("int");

                    b.HasKey("CodEquipe", "CodEvento");

                    b.HasIndex("CodEvento");

                    b.ToTable("Cadastras");
                });

            modelBuilder.Entity("apiAEE.Entities.Equipe", b =>
                {
                    b.Property<int>("CodEquipe")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodEquipe"));

                    b.Property<string>("Modalidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeEquipe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlImagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodEquipe");

                    b.ToTable("Equipes");
                });

            modelBuilder.Entity("apiAEE.Entities.Evento", b =>
                {
                    b.Property<int>("CodEvento")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodEvento"));

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocalEvento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeEvento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlImagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodEvento");

                    b.ToTable("Eventos");
                });

            modelBuilder.Entity("apiAEE.Entities.Midia", b =>
                {
                    b.Property<int>("CodMidia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodMidia"));

                    b.Property<int?>("CodEvento")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UrlImagem")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CodMidia");

                    b.HasIndex("CodEvento");

                    b.ToTable("Midias");
                });

            modelBuilder.Entity("apiAEE.Entities.Participar", b =>
                {
                    b.Property<int>("CodEquipe")
                        .HasColumnType("int");

                    b.Property<int>("CodEvento")
                        .HasColumnType("int");

                    b.Property<int>("CodPartida")
                        .HasColumnType("int");

                    b.HasKey("CodEquipe", "CodEvento", "CodPartida");

                    b.HasIndex("CodPartida");

                    b.ToTable("Participas");
                });

            modelBuilder.Entity("apiAEE.Entities.Partida", b =>
                {
                    b.Property<int>("CodPartida")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodPartida"));

                    b.Property<DateTime>("HoratioPartida")
                        .HasColumnType("datetime2");

                    b.Property<int>("Pontuacao")
                        .HasColumnType("int");

                    b.Property<int>("QuantMinJogadores")
                        .HasColumnType("int");

                    b.HasKey("CodPartida");

                    b.ToTable("Partidas");
                });

            modelBuilder.Entity("apiAEE.Entities.Pertence", b =>
                {
                    b.Property<int>("CodEquipe")
                        .HasColumnType("int");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.HasKey("CodEquipe", "ID");

                    b.HasIndex("ID");

                    b.ToTable("Pertences");
                });

            modelBuilder.Entity("apiAEE.Entities.Amistoso", b =>
                {
                    b.HasOne("apiAEE.Entities.Equipe", "Equipe")
                        .WithMany("Amistoso")
                        .HasForeignKey("CodEquipe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apiAEE.Entities.Partida", "Partida")
                        .WithMany("Amistoso")
                        .HasForeignKey("CodPartida")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipe");

                    b.Navigation("Partida");
                });

            modelBuilder.Entity("apiAEE.Entities.Cadastrar", b =>
                {
                    b.HasOne("apiAEE.Entities.Equipe", "Equipe")
                        .WithMany("Cadastrar")
                        .HasForeignKey("CodEquipe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apiAEE.Entities.Evento", "Evento")
                        .WithMany("Cadastrar")
                        .HasForeignKey("CodEvento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipe");

                    b.Navigation("Evento");
                });

            modelBuilder.Entity("apiAEE.Entities.Midia", b =>
                {
                    b.HasOne("apiAEE.Entities.Evento", "Evento")
                        .WithMany("Midias")
                        .HasForeignKey("CodEvento")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Evento");
                });

            modelBuilder.Entity("apiAEE.Entities.Participar", b =>
                {
                    b.HasOne("apiAEE.Entities.Partida", "Partida")
                        .WithMany("Participar")
                        .HasForeignKey("CodPartida")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("apiAEE.Entities.Cadastrar", "Cadastrar")
                        .WithMany("Participar")
                        .HasForeignKey("CodEquipe", "CodEvento")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cadastrar");

                    b.Navigation("Partida");
                });

            modelBuilder.Entity("apiAEE.Entities.Pertence", b =>
                {
                    b.HasOne("apiAEE.Entities.Equipe", "Equipe")
                        .WithMany("Pertences")
                        .HasForeignKey("CodEquipe")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Usuario", "Usuario")
                        .WithMany("Pertences")
                        .HasForeignKey("ID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Equipe");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("Usuario", b =>
                {
                    b.Navigation("Pertences");
                });

            modelBuilder.Entity("apiAEE.Entities.Cadastrar", b =>
                {
                    b.Navigation("Participar");
                });

            modelBuilder.Entity("apiAEE.Entities.Equipe", b =>
                {
                    b.Navigation("Amistoso");

                    b.Navigation("Cadastrar");

                    b.Navigation("Pertences");
                });

            modelBuilder.Entity("apiAEE.Entities.Evento", b =>
                {
                    b.Navigation("Cadastrar");

                    b.Navigation("Midias");
                });

            modelBuilder.Entity("apiAEE.Entities.Partida", b =>
                {
                    b.Navigation("Amistoso");

                    b.Navigation("Participar");
                });
#pragma warning restore 612, 618
        }
    }
}
