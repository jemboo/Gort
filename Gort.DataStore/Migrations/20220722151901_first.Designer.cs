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
    [Migration("20220722151901_first")]
    partial class first
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Gort.DataStore.DataModel.Cause", b =>
                {
                    b.Property<int>("CauseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseStatus")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasColumnType("longtext");

                    b.Property<string>("Genus")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("WorkspaceId")
                        .HasColumnType("char(36)");

                    b.HasKey("CauseId");

                    b.HasIndex("WorkspaceId", "Index")
                        .IsUnique();

                    b.ToTable("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseParam", b =>
                {
                    b.Property<int>("CauseParamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("ParamId")
                        .HasColumnType("int");

                    b.HasKey("CauseParamId");

                    b.HasIndex("CauseId");

                    b.HasIndex("ParamId");

                    b.ToTable("CauseParam");
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

            modelBuilder.Entity("Gort.DataStore.DataModel.RandGen", b =>
                {
                    b.Property<int>("RandGenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("RandGenId");

                    b.HasIndex("CauseId");

                    b.ToTable("RandGen");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Sortable", b =>
                {
                    b.Property<int>("SortableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SortableId");

                    b.HasIndex("CauseId");

                    b.ToTable("Sortable");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableGen", b =>
                {
                    b.Property<int>("SortableGenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SortableGenId");

                    b.HasIndex("CauseId");

                    b.ToTable("SortableGen");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableSet", b =>
                {
                    b.Property<int>("SortableSetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SortableSetId");

                    b.HasIndex("CauseId");

                    b.ToTable("SortableSet");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Sorter", b =>
                {
                    b.Property<int>("SorterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterId");

                    b.HasIndex("CauseId");

                    b.ToTable("Sorter");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterGen", b =>
                {
                    b.Property<int>("SorterGenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterGenId");

                    b.HasIndex("CauseId");

                    b.ToTable("SorterGen");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterMutator", b =>
                {
                    b.Property<int>("SorterMutatorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterMutatorId");

                    b.HasIndex("CauseId");

                    b.ToTable("SorterMutator");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterPerf", b =>
                {
                    b.Property<int>("SorterPerfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("SorterSetId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterPerfId");

                    b.HasIndex("CauseId");

                    b.HasIndex("SorterSetId");

                    b.ToTable("SorterPerf");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSet", b =>
                {
                    b.Property<int>("SorterSetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterSetId");

                    b.HasIndex("CauseId");

                    b.ToTable("SorterSet");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetPerf", b =>
                {
                    b.Property<int>("SorterSetPerfId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CauseId")
                        .HasColumnType("int");

                    b.Property<string>("CausePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Cereal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<int>("SorterSetId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("SorterSetPerfId");

                    b.HasIndex("CauseId");

                    b.HasIndex("SorterSetId");

                    b.ToTable("SorterSetPerf");
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

            modelBuilder.Entity("Gort.DataStore.DataModel.Cause", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Workspace", "Workspace")
                        .WithMany("Causes")
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.CauseParam", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany("CauseParams")
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gort.DataStore.DataModel.Param", "Param")
                        .WithMany()
                        .HasForeignKey("ParamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");

                    b.Navigation("Param");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.RandGen", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Sortable", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableGen", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SortableSet", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Sorter", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterGen", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterMutator", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterPerf", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gort.DataStore.DataModel.SorterSet", "SorterSet")
                        .WithMany()
                        .HasForeignKey("SorterSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");

                    b.Navigation("SorterSet");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSet", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.SorterSetPerf", b =>
                {
                    b.HasOne("Gort.DataStore.DataModel.Cause", "Cause")
                        .WithMany()
                        .HasForeignKey("CauseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Gort.DataStore.DataModel.SorterSet", "SorterSet")
                        .WithMany()
                        .HasForeignKey("SorterSetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cause");

                    b.Navigation("SorterSet");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Cause", b =>
                {
                    b.Navigation("CauseParams");
                });

            modelBuilder.Entity("Gort.DataStore.DataModel.Workspace", b =>
                {
                    b.Navigation("Causes");
                });
#pragma warning restore 612, 618
        }
    }
}
