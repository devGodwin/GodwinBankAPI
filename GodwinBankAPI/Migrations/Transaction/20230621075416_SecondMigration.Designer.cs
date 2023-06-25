﻿// <auto-generated />
using System;
using GodwinBankAPI.Data.TransactionData;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GodwinBankAPI.Migrations.Transaction
{
    [DbContext(typeof(TransactionContext))]
    [Migration("20230621075416_SecondMigration")]
    partial class SecondMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GodwinBankAPI.Data.TransactionData.Transaction", b =>
                {
                    b.Property<string>("TransactionId")
                        .HasColumnType("text");

                    b.Property<decimal>("AccountBalance")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DestinationAccountNumber")
                        .HasColumnType("text");

                    b.Property<string>("SourceAccountNumber")
                        .HasColumnType("text");

                    b.Property<string>("TransactionStatus")
                        .HasColumnType("text");

                    b.Property<string>("TransactionType")
                        .HasColumnType("text");

                    b.HasKey("TransactionId");

                    b.ToTable("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
