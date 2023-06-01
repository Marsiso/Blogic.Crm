﻿// <auto-generated />
using System;
using Blogic.Crm.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Blogic.Crm.Web.Data.Migrations.DataSeed
{
    [DbContext(typeof(DataContext))]
    [Migration("20230601042726_DataSeed")]
    partial class DataSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Blogic.Crm.Domain.Data.Entities.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BirthNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("birth_number");

                    b.Property<byte[]>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("concurrency_stamp");

                    b.Property<DateTime>("DateBorn")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_born");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("email");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("family_name");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("given_name");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit")
                        .HasColumnName("is_email_confirmed");

                    b.Property<bool>("IsPhoneConfirmed")
                        .HasColumnType("bit")
                        .HasColumnName("is_phone_confirmed");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password_hash");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("phone");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("security_stamp");

                    b.HasKey("Id");

                    b.HasIndex("BirthNumber")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("clients", "dbo");
                });

            modelBuilder.Entity("Blogic.Crm.Domain.Data.Entities.Consultant", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BirthNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("birth_number");

                    b.Property<byte[]>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion")
                        .HasColumnName("concurrency_stamp");

                    b.Property<DateTime>("DateBorn")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_born");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("email");

                    b.Property<string>("FamilyName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("family_name");

                    b.Property<string>("GivenName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("given_name");

                    b.Property<bool>("IsEmailConfirmed")
                        .HasColumnType("bit")
                        .HasColumnName("is_email_confirmed");

                    b.Property<bool>("IsPhoneConfirmed")
                        .HasColumnType("bit")
                        .HasColumnName("is_phone_confirmed");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("password_hash");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("phone");

                    b.Property<string>("SecurityStamp")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)")
                        .HasColumnName("security_stamp");

                    b.HasKey("Id");

                    b.HasIndex("BirthNumber")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("consultants", "dbo");
                });

            modelBuilder.Entity("Blogic.Crm.Domain.Data.Entities.Contract", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ClientId")
                        .HasColumnType("bigint")
                        .HasColumnName("client_id");

                    b.Property<DateTime>("DateConcluded")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_concluded");

                    b.Property<DateTime>("DateExpired")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_expired");

                    b.Property<DateTime>("DateValid")
                        .HasColumnType("datetime2")
                        .HasColumnName("date_valid");

                    b.Property<string>("Institution")
                        .IsRequired()
                        .HasMaxLength(256)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(256)")
                        .HasColumnName("institution");

                    b.Property<long?>("ManagerId")
                        .HasColumnType("bigint")
                        .HasColumnName("manager_id");

                    b.Property<string>("RegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)")
                        .HasColumnName("registration_number");

                    b.HasKey("Id");

                    b.HasIndex("RegistrationNumber")
                        .IsUnique();

                    b.ToTable("contracts", "dbo");
                });

            modelBuilder.Entity("Blogic.Crm.Domain.Data.Entities.ContractConsultant", b =>
                {
                    b.Property<long>("ContractId")
                        .HasColumnType("bigint");

                    b.Property<long>("ConsultantId")
                        .HasColumnType("bigint");

                    b.HasKey("ContractId", "ConsultantId");

                    b.ToTable("contract_consultants", "dbo");
                });
#pragma warning restore 612, 618
        }
    }
}
