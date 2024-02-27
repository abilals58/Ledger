﻿// <auto-generated />
using System;
using Ledger.Ledger.Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ledger.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20240214130458_BudgetCheck")]
    partial class BudgetCheck
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ledger.Ledger.Web.Models.BuyOrder", b =>
                {
                    b.Property<int>("BuyOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BuyOrderId"));

                    b.Property<double>("BidPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("BidSize")
                        .HasColumnType("integer");

                    b.Property<int>("CurrentBidSize")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("BuyOrderId");

                    b.ToTable("BuyOrders");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.BuyOrderMatch", b =>
                {
                    b.Property<int>("BuyOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("SellOrderId")
                        .HasColumnType("integer");

                    b.HasKey("BuyOrderId", "SellOrderId");

                    b.ToTable("BuyOrderMatches");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.BuyOrderProcess", b =>
                {
                    b.Property<int>("BuyOrderProcessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("BuyOrderProcessId"));

                    b.Property<double>("BidPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("BuyOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("BuyOrderProcessId");

                    b.ToTable("BuyOrderJobs");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.DailyStock", b =>
                {
                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("StockValue")
                        .HasColumnType("double precision");

                    b.HasKey("StockId", "Date");

                    b.ToTable("DailyStocks");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.SellOrder", b =>
                {
                    b.Property<int>("SellOrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SellOrderId"));

                    b.Property<double>("AskPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("AskSize")
                        .HasColumnType("integer");

                    b.Property<int>("CurrentAskSize")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("SellOrderId");

                    b.ToTable("SellOrders");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.SellOrderMatch", b =>
                {
                    b.Property<int>("SellOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("BuyOrderId")
                        .HasColumnType("integer");

                    b.HasKey("SellOrderId", "BuyOrderId");

                    b.ToTable("SellOrderMatches");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.SellOrderProcess", b =>
                {
                    b.Property<int>("SellOrderProcessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("SellOrderProcessId"));

                    b.Property<double>("AskPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("OrderNum")
                        .HasColumnType("integer");

                    b.Property<int>("SellOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.HasKey("SellOrderProcessId");

                    b.ToTable("SellOrderJobs");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.Stock", b =>
                {
                    b.Property<int>("StockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StockId"));

                    b.Property<double>("CurrentPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("CurrentStock")
                        .HasColumnType("integer");

                    b.Property<double>("HighestPrice")
                        .HasColumnType("double precision");

                    b.Property<double>("InitialPrice")
                        .HasColumnType("double precision");

                    b.Property<int>("InitialStock")
                        .HasColumnType("integer");

                    b.Property<double>("LowestPrice")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("StockName")
                        .HasColumnType("text");

                    b.HasKey("StockId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.StocksOfUser", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("NumOfStocks")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "StockId");

                    b.ToTable("StocksOfUser");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.Transaction", b =>
                {
                    b.Property<int>("Tid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Tid"));

                    b.Property<int>("BuyOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("BuyerId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("SellOrderId")
                        .HasColumnType("integer");

                    b.Property<int>("SellerId")
                        .HasColumnType("integer");

                    b.Property<int>("StockId")
                        .HasColumnType("integer");

                    b.Property<int>("StockNum")
                        .HasColumnType("integer");

                    b.HasKey("Tid");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Ledger.Ledger.Web.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<double>("Budget")
                        .HasColumnType("double precision");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
