﻿// <auto-generated />
using System;
using Agendamentos.Shared.Dados.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AgendamentosAPI.Shared.Dados.Migrations
{
    [DbContext(typeof(AgendamentosContext))]
    [Migration("20240615022936_AddColumnToken")]
    partial class AddColumnToken
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Agendamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AulasId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Data")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("EquipamentoId")
                        .HasColumnType("integer");

                    b.Property<string>("ProfessorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AulasId");

                    b.HasIndex("EquipamentoId");

                    b.HasIndex("ProfessorId");

                    b.ToTable("Agendamentos");
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Aulas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Aula")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("Duracao")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.ToTable("Aulas");
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Equipamentos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantidade")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Equipamentos");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.AgendamentoAula", b =>
                {
                    b.Property<int>("AgendamentoId")
                        .HasColumnType("integer");

                    b.Property<int>("AulaId")
                        .HasColumnType("integer");

                    b.HasKey("AgendamentoId", "AulaId");

                    b.HasIndex("AulaId");

                    b.ToTable("AgendamentoAulas");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.NivelAcesso", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("TipoAcesso")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("NivelAcessos");

                    b.HasData(
                        new
                        {
                            Id = "918b0a58-4334-4af7-92a5-1e5d39064f88",
                            TipoAcesso = "Gestor"
                        });
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AcessoId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Senha")
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AcessoId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = "718dcaf6-1cb1-41e6-b747-8466f6a05134",
                            AcessoId = "918b0a58-4334-4af7-92a5-1e5d39064f88",
                            Email = "root@gmail.com",
                            Senha = "Soeuseisoeusei",
                            UserName = "root"
                        });
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Agendamento", b =>
                {
                    b.HasOne("Agendamentos.Shared.Modelos.Modelos.Aulas", null)
                        .WithMany("Agendamentos")
                        .HasForeignKey("AulasId");

                    b.HasOne("Agendamentos.Shared.Modelos.Modelos.Equipamentos", "Equipamento")
                        .WithMany("Agendamentos")
                        .HasForeignKey("EquipamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AgendamentosAPI.Shared.Models.Modelos.User", "Professor")
                        .WithMany("Agendamentos")
                        .HasForeignKey("ProfessorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipamento");

                    b.Navigation("Professor");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.AgendamentoAula", b =>
                {
                    b.HasOne("Agendamentos.Shared.Modelos.Modelos.Agendamento", "Agendamento")
                        .WithMany("AgendamentoAulas")
                        .HasForeignKey("AgendamentoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Agendamentos.Shared.Modelos.Modelos.Aulas", "Aula")
                        .WithMany("AgendamentoAulas")
                        .HasForeignKey("AulaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agendamento");

                    b.Navigation("Aula");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.User", b =>
                {
                    b.HasOne("AgendamentosAPI.Shared.Models.Modelos.NivelAcesso", "NivelAcesso")
                        .WithMany("Users")
                        .HasForeignKey("AcessoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_User_Acesso");

                    b.Navigation("NivelAcesso");
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Agendamento", b =>
                {
                    b.Navigation("AgendamentoAulas");
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Aulas", b =>
                {
                    b.Navigation("AgendamentoAulas");

                    b.Navigation("Agendamentos");
                });

            modelBuilder.Entity("Agendamentos.Shared.Modelos.Modelos.Equipamentos", b =>
                {
                    b.Navigation("Agendamentos");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.NivelAcesso", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("AgendamentosAPI.Shared.Models.Modelos.User", b =>
                {
                    b.Navigation("Agendamentos");
                });
#pragma warning restore 612, 618
        }
    }
}
