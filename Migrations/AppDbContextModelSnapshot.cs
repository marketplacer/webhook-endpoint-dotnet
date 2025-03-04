﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace webhook_endpoint_dotnet.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("WebhookEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("Sequence")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WebhookEventType")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebhookId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WebhookObjectId")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebhookObjectType")
                        .HasColumnType("TEXT");

                    b.Property<string>("WebhookPayload")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WebhookEvents");
                });
#pragma warning restore 612, 618
        }
    }
}
