﻿// <auto-generated />
using System;
using AdvertSite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AdvertSite.Migrations
{
    [DbContext(typeof(advert_siteContext))]
    partial class advert_siteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AdvertSite.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("HomeAdress");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserLevel");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("AdvertSite.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(45)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("AdvertSite.Models.Comments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<int>("Listingid")
                        .HasColumnName("listingid");

                    b.Property<string>("Text")
                        .HasColumnName("text")
                        .IsUnicode(false);

                    b.Property<string>("Userid")
                        .HasColumnName("userid");

                    b.HasKey("Id");

                    b.HasIndex("Listingid");

                    b.HasIndex("Userid");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("AdvertSite.Models.ListingPictures", b =>
                {
                    b.Property<int>("PictureId")
                        .HasColumnName("picture_id");

                    b.Property<int>("ListingId")
                        .HasColumnName("Listing_id");

                    b.HasKey("PictureId", "ListingId");

                    b.HasIndex("ListingId");

                    b.ToTable("Listing_pictures");
                });

            modelBuilder.Entity("AdvertSite.Models.Listings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnName("description")
                        .IsUnicode(false);

                    b.Property<short?>("Display")
                        .HasColumnName("display");

                    b.Property<double?>("GoogleLatitude");

                    b.Property<double?>("GoogleLongitude");

                    b.Property<double?>("GoogleRadius");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("name")
                        .HasMaxLength(45)
                        .IsUnicode(false);

                    b.Property<double>("Price")
                        .HasColumnName("price");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("quantity")
                        .HasDefaultValueSql("((1))");

                    b.Property<int>("Subcategoryid")
                        .HasColumnName("subcategoryid");

                    b.Property<string>("Userid")
                        .IsRequired()
                        .HasColumnName("userid")
                        .HasMaxLength(450);

                    b.Property<short?>("Verified")
                        .HasColumnName("verified");

                    b.HasKey("Id");

                    b.HasIndex("Subcategoryid");

                    b.HasIndex("Userid");

                    b.ToTable("Listings");
                });

            modelBuilder.Entity("AdvertSite.Models.Messages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateSent")
                        .HasColumnName("dateSent")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnName("subject")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnName("text")
                        .HasMaxLength(1000)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("AdvertSite.Models.Reviews", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Buyerid")
                        .IsRequired()
                        .HasColumnName("buyerid")
                        .HasMaxLength(450);

                    b.Property<string>("Comment")
                        .HasColumnName("comment")
                        .IsUnicode(false);

                    b.Property<DateTime?>("Date")
                        .HasColumnName("date")
                        .HasColumnType("datetime2(0)");

                    b.Property<short?>("Evaluation")
                        .HasColumnName("evaluation");

                    b.Property<string>("Sellerid")
                        .IsRequired()
                        .HasColumnName("sellerid")
                        .HasMaxLength(450);

                    b.HasKey("Id");

                    b.HasIndex("Buyerid");

                    b.HasIndex("Sellerid");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("AdvertSite.Models.Subcategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Categoryid")
                        .HasColumnName("categoryid");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasMaxLength(45)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("Categoryid");

                    b.ToTable("Subcategory");
                });

            modelBuilder.Entity("AdvertSite.Models.UsersHasMessages", b =>
                {
                    b.Property<string>("RecipientId")
                        .HasColumnName("recipient_id");

                    b.Property<int>("MessagesId")
                        .HasColumnName("Messages_id");

                    b.Property<string>("SenderId")
                        .HasColumnName("Messages_sender_id");

                    b.Property<short?>("AlreadyRead")
                        .HasColumnName("alreadyRead");

                    b.Property<short?>("IsAdminMessage")
                        .HasColumnName("isAdminMessage");

                    b.Property<short?>("IsDeleted")
                        .HasColumnName("isDeleted");

                    b.HasKey("RecipientId", "MessagesId", "SenderId");

                    b.HasIndex("MessagesId");

                    b.HasIndex("SenderId");

                    b.ToTable("Users_has_Messages");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AdvertSite.Models.Comments", b =>
                {
                    b.HasOne("AdvertSite.Models.Listings", "Listing")
                        .WithMany("Comments")
                        .HasForeignKey("Listingid")
                        .HasConstraintName("fk_Comments_Listings11")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AdvertSite.Models.ApplicationUser", "User")
                        .WithMany("Comments")
                        .HasForeignKey("Userid")
                        .HasConstraintName("fk_Comments_Users11")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdvertSite.Models.ListingPictures", b =>
                {
                    b.HasOne("AdvertSite.Models.Listings", "Listing")
                        .WithMany("ListingPictures")
                        .HasForeignKey("ListingId")
                        .HasConstraintName("fk_Listing_pictures_Listings1");
                });

            modelBuilder.Entity("AdvertSite.Models.Listings", b =>
                {
                    b.HasOne("AdvertSite.Models.Subcategory", "Subcategory")
                        .WithMany("Listings")
                        .HasForeignKey("Subcategoryid")
                        .HasConstraintName("fk_Listings_Subcategory1");

                    b.HasOne("AdvertSite.Models.ApplicationUser", "User")
                        .WithMany("Listings")
                        .HasForeignKey("Userid")
                        .HasConstraintName("fk_Listings_Users1");
                });

            modelBuilder.Entity("AdvertSite.Models.Reviews", b =>
                {
                    b.HasOne("AdvertSite.Models.ApplicationUser", "Buyer")
                        .WithMany("ReviewsBuyer")
                        .HasForeignKey("Buyerid")
                        .HasConstraintName("id");

                    b.HasOne("AdvertSite.Models.ApplicationUser", "Seller")
                        .WithMany("ReviewsSeller")
                        .HasForeignKey("Sellerid")
                        .HasConstraintName("id1");
                });

            modelBuilder.Entity("AdvertSite.Models.Subcategory", b =>
                {
                    b.HasOne("AdvertSite.Models.Category", "Category")
                        .WithMany("Subcategory")
                        .HasForeignKey("Categoryid")
                        .HasConstraintName("fk_Subcategory_Category1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AdvertSite.Models.UsersHasMessages", b =>
                {
                    b.HasOne("AdvertSite.Models.Messages", "Messages")
                        .WithMany("UsersHasMessages")
                        .HasForeignKey("MessagesId")
                        .HasConstraintName("fk_Users_has_Messages_Messages1");

                    b.HasOne("AdvertSite.Models.ApplicationUser", "Recipient")
                        .WithMany("ReceivedMessages")
                        .HasForeignKey("RecipientId")
                        .HasConstraintName("fk_Users_has_Messages_Users1");

                    b.HasOne("AdvertSite.Models.ApplicationUser", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .HasConstraintName("fk_Users_has_Messages_Users2");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AdvertSite.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AdvertSite.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AdvertSite.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AdvertSite.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
