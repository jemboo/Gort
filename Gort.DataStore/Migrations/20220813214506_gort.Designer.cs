﻿// <auto-generated />
using System;
using Gort.DataStore.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Gort.DataStore.Migrations
{
    [DbContext(typeof(GortContext2))]
    [Migration("20220813214506_gort")]
    partial class gort
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Gort.DataStore.DataModel.BitPackR", b =>
                {
                    b.Property<int>("BitPackRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BitsPerSymbol")
                        .HasColumnType("int");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("SymbolCount")
                        .HasColumnType("int");

                    b.HasKey("BitPackRId");

                    b.ToTable("BitPackR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseParamR", b =>
                {
                    b.Property<int>("CauseParamRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ParamId")
                        .HasColumnType("int");

                    b.HasKey("CauseParamRId");

                    b.HasIndex("CauseRId");

                    b.HasIndex("ParamId");

                    b.ToTable("CauseParamR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseR", b =>
                {
                    b.Property<int>("CauseRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseStatus")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("longtext");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("char(36)");

                    b.HasKey("CauseRId");

                    b.HasIndex("WorkspaceId", "Index")
                        .IsUnique();

                    b.ToTable("CauseR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Param", b =>
                {
                    b.Property<int>("ParamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("ParamDataType")
                        .HasColumnType("int");

                    b.Property<byte[]>("Value")
                        .HasColumnType("longblob");

                    b.HasKey("ParamId");

                    b.ToTable("Param");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.RandGenR", b =>
                {
                    b.Property<int>("RandGenRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("RandGenRId");

                    b.HasIndex("CauseRId");

                    b.ToTable("RandGenR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableSetR", b =>
                {
                    b.Property<int>("SortableSetRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BitPackRId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SortableSetRId");

                    b.HasIndex("BitPackRId");

                    b.HasIndex("CauseRId");

                    b.ToTable("SortableSetR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterR", b =>
                {
                    b.Property<int>("SorterRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BitPackRId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterRId");

                    b.HasIndex("BitPackRId");

                    b.HasIndex("CauseRId");

                    b.ToTable("SorterR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetPerfR", b =>
                {
                    b.Property<int>("SorterSetPerfRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BitPackRId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("SorterSetRId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterSetPerfRId");

                    b.HasIndex("BitPackRId");

                    b.HasIndex("CauseRId");

                    b.HasIndex("SorterSetRId");

                    b.ToTable("SorterSetPerfR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetR", b =>
                {
                    b.Property<int>("SorterSetRId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("BitPackRId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("CauseRId")
                        .HasColumnType("int");

                    b.Property<string>("Json")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterSetRId");

                    b.HasIndex("BitPackRId");

                    b.HasIndex("CauseRId");

                    b.ToTable("SorterSetR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Workspace", b =>
                {
                    b.Property<Guid>("WorkspaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("WorkspaceId");

                    b.ToTable("Workspace");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseParamR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany("CauseParamRs")
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gort.DataStore.DataModel.Param", "Param")
                        .WithMany()
                        .HasForeignKey("ParamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CauseR");

                    b.Navigation("Param");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Workspace", "Workspace")
                        .WithMany("Causes")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.RandGenR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany()
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CauseR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableSetR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.BitPackR", "BitPackR")
                        .WithMany()
                        .HasForeignKey("BitPackRId");

                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany()
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BitPackR");

                    b.Navigation("CauseR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.BitPackR", "BitPackR")
                        .WithMany()
                        .HasForeignKey("BitPackRId");

                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany()
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BitPackR");

                    b.Navigation("CauseR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetPerfR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.BitPackR", "BitPackR")
                        .WithMany()
                        .HasForeignKey("BitPackRId");

                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany()
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gort.DataStore.DataModel.SorterSetR", "SorterSetR")
                        .WithMany()
                        .HasForeignKey("SorterSetRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BitPackR");

                    b.Navigation("CauseR");

                    b.Navigation("SorterSetR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetR", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.BitPackR", "BitPackR")
                        .WithMany()
                        .HasForeignKey("BitPackRId");

                    b.HasOne("Gort.DataStore.DataModel.CauseR", "CauseR")
                        .WithMany()
                        .HasForeignKey("CauseRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BitPackR");

                    b.Navigation("CauseR");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseR", b =>
                {
                    b.Navigation("CauseParamRs");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Workspace", b =>
                {
                    b.Navigation("Causes");
                });
#pragma warning restore 612, 618
        }
    }
}