﻿// <auto-generated />
using System;
using BingoPelc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BingoPelc.Migrations
{
    [DbContext(typeof(DomainContextDb))]
    [Migration("20230406193120_Created admin user")]
    partial class Createdadminuser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BingoPelc.Entities.DailyBingo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValue(new DateTime(2023, 4, 6, 21, 31, 20, 333, DateTimeKind.Local).AddTicks(3910));

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("daily_bingo", (string)null);
                });

            modelBuilder.Entity("BingoPelc.Entities.DailyQuestion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Checked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid>("DailyBingoId")
                        .HasColumnType("uuid");

                    b.Property<int>("Index")
                        .HasColumnType("integer");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DailyBingoId");

                    b.HasIndex("QuestionId");

                    b.ToTable("daily_questions", (string)null);
                });

            modelBuilder.Entity("BingoPelc.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("question", (string)null);
                });

            modelBuilder.Entity("BingoPelc.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("77318190-c9cf-4568-aa0e-374bfa958823"),
                            Email = "kamilpietrak123@gmail.com",
                            HashedPassword = "AQAAAAIAAYagAAAAEDkMuHLJn58ej1h3jD/A8Q4CHD3pc2fOi647e2fEn+FsDIp8LjqNbUFlgPPZ9U+vLg==",
                            Nickname = "SWETRAK"
                        });
                });

            modelBuilder.Entity("BingoPelc.Entities.DailyBingo", b =>
                {
                    b.HasOne("BingoPelc.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BingoPelc.Entities.DailyQuestion", b =>
                {
                    b.HasOne("BingoPelc.Entities.DailyBingo", "DailyBingo")
                        .WithMany("DailyQuestions")
                        .HasForeignKey("DailyBingoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BingoPelc.Entities.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DailyBingo");

                    b.Navigation("Question");
                });

            modelBuilder.Entity("BingoPelc.Entities.DailyBingo", b =>
                {
                    b.Navigation("DailyQuestions");
                });
#pragma warning restore 612, 618
        }
    }
}
