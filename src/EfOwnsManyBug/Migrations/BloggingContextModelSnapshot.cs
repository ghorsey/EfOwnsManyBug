﻿// <auto-generated />
using System;
using EfOwnsManyBug;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EfOwnsManyBug.Migrations
{
    [DbContext(typeof(BloggingContext))]
    partial class BloggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EfOwnsManyBug.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EfOwnsManyBug.Models.PostSummary", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("PostSummary");
                });

            modelBuilder.Entity("EfOwnsManyBug.Models.Post", b =>
                {
                    b.OwnsOne("EfOwnsManyBug.Models.PostBody", "Body", b1 =>
                        {
                            b1.Property<Guid>("PostId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Slices")
                                .IsRequired()
                                .HasMaxLength(8000)
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(8000)
                                .HasColumnType("nvarchar(max)")
                                .HasDefaultValue("");

                            b1.HasKey("PostId");

                            b1.ToTable("Posts");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.Navigation("Body")
                        .IsRequired();
                });

            modelBuilder.Entity("EfOwnsManyBug.Models.PostSummary", b =>
                {
                    b.HasOne("EfOwnsManyBug.Models.Post", null)
                        .WithOne()
                        .HasForeignKey("EfOwnsManyBug.Models.PostSummary", "Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsMany("EfOwnsManyBug.Models.PostSummaryTag", "Tags", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("PostId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Tag")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(200)
                                .HasColumnType("nvarchar(200)")
                                .HasDefaultValue("");

                            b1.HasKey("Id");

                            b1.HasIndex("PostId", "Tag");

                            b1.ToTable("PostSummaryTag");

                            b1.WithOwner()
                                .HasForeignKey("PostId");
                        });

                    b.OwnsOne("EfOwnsManyBug.Models.PostBody", "Body", b1 =>
                        {
                            b1.Property<Guid>("PostSummaryId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Slices")
                                .IsRequired()
                                .HasMaxLength(8000)
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .ValueGeneratedOnAdd()
                                .HasMaxLength(8000)
                                .HasColumnType("nvarchar(max)")
                                .HasDefaultValue("");

                            b1.HasKey("PostSummaryId");

                            b1.ToTable("PostSummary");

                            b1.WithOwner()
                                .HasForeignKey("PostSummaryId");
                        });

                    b.Navigation("Body")
                        .IsRequired();

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
