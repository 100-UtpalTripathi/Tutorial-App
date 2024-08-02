﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TutorialApp.Contexts;

#nullable disable

namespace TutorialApp.Migrations
{
    [DbContext(typeof(TutorialAppContext))]
    [Migration("20240802043417_Updated course categories")]
    partial class Updatedcoursecategories
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TutorialApp.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("CartId");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("TutorialApp.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("TutorialApp.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CourseURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstructorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TutorialApp.Models.Enrollment", b =>
                {
                    b.Property<int>("EnrollmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EnrollmentId"));

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("EnrollmentId");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Enrollments");
                });

            modelBuilder.Entity("TutorialApp.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionId"));

                    b.Property<string>("CorrectAnswer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionC")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OptionD")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionId");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TutorialApp.Models.Quiz", b =>
                {
                    b.Property<int>("QuizId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuizId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuizId");

                    b.HasIndex("CourseId");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("TutorialApp.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageURI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Email");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TutorialApp.Models.UserCredential", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Email");

                    b.ToTable("UserCredentials");
                });

            modelBuilder.Entity("TutorialApp.Models.Wishlist", b =>
                {
                    b.Property<int>("WishlistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WishlistId"));

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("WishlistId");

                    b.HasIndex("CourseId");

                    b.HasIndex("UserEmail");

                    b.ToTable("Wishlists");
                });

            modelBuilder.Entity("TutorialApp.Models.Cart", b =>
                {
                    b.HasOne("TutorialApp.Models.Course", "Course")
                        .WithMany("Carts")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutorialApp.Models.User", "User")
                        .WithMany("Carts")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutorialApp.Models.Course", b =>
                {
                    b.HasOne("TutorialApp.Models.Category", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("TutorialApp.Models.Enrollment", b =>
                {
                    b.HasOne("TutorialApp.Models.Course", "Course")
                        .WithMany("Enrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutorialApp.Models.User", "User")
                        .WithMany("Enrollments")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutorialApp.Models.Question", b =>
                {
                    b.HasOne("TutorialApp.Models.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("TutorialApp.Models.Quiz", b =>
                {
                    b.HasOne("TutorialApp.Models.Course", "Course")
                        .WithMany("Quizzes")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("TutorialApp.Models.UserCredential", b =>
                {
                    b.HasOne("TutorialApp.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutorialApp.Models.Wishlist", b =>
                {
                    b.HasOne("TutorialApp.Models.Course", "Course")
                        .WithMany("Wishlists")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TutorialApp.Models.User", "User")
                        .WithMany("Wishlists")
                        .HasForeignKey("UserEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TutorialApp.Models.Category", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("TutorialApp.Models.Course", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Enrollments");

                    b.Navigation("Quizzes");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("TutorialApp.Models.Quiz", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("TutorialApp.Models.User", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Enrollments");

                    b.Navigation("Wishlists");
                });
#pragma warning restore 612, 618
        }
    }
}
