﻿// <auto-generated />
using System;
using FileService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FileService.Infrastructure.Migrations
{
    [DbContext(typeof(FileServiceDbContext))]
    [Migration("20240306125927_AddFileEntity")]
    partial class AddFileEntity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("FileService.Domain.FileUploadRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BackupUrl")
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<long?>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<long>("FileByteSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileHash")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("RemoteUrl")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("varchar(300)");

                    b.HasKey("Id");

                    b.HasIndex("FileByteSize");

                    b.HasIndex("FileHash");

                    b.ToTable("FileUploadRecord", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
