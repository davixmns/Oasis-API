﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OasisAPI.Context;

#nullable disable

namespace OasisAPI.Migrations
{
    [DbContext(typeof(OasisDbContext))]
    [Migration("20240515191444_rename_UserId")]
    partial class rename_UserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("OasisAPI.Models.OasisChat", b =>
                {
                    b.Property<int>("OasisChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("OasisChatId"));

                    b.Property<string>("ChatGptThreadId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("GeminiThreadId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("OasisUserId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("OasisChatId");

                    b.HasIndex("OasisUserId");

                    b.ToTable("oasis_chats");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisMessage", b =>
                {
                    b.Property<int>("OasisMessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("OasisMessageId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FromMessageId")
                        .HasColumnType("longtext");

                    b.Property<string>("FromThreadId")
                        .HasColumnType("longtext");

                    b.Property<bool?>("IsSaved")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("OasisChatId")
                        .HasColumnType("int");

                    b.HasKey("OasisMessageId");

                    b.HasIndex("OasisChatId");

                    b.ToTable("oasis_messages");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisUser", b =>
                {
                    b.Property<int>("OasisUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("OasisUserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<DateTime>("RefreshTokenExpiryDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("OasisUserId");

                    b.ToTable("oasis_users");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisChat", b =>
                {
                    b.HasOne("OasisAPI.Models.OasisUser", "User")
                        .WithMany("Chats")
                        .HasForeignKey("OasisUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisMessage", b =>
                {
                    b.HasOne("OasisAPI.Models.OasisChat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("OasisChatId");

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisChat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("OasisAPI.Models.OasisUser", b =>
                {
                    b.Navigation("Chats");
                });
#pragma warning restore 612, 618
        }
    }
}
