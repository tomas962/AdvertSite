﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvertSite.Models
{
    public partial class advert_siteContext : IdentityDbContext<ApplicationUser>
    {
        public advert_siteContext()
        {
        }

        public advert_siteContext(DbContextOptions<advert_siteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ListingPictures> ListingPictures { get; set; }
        public virtual DbSet<Listings> Listings { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }
        public virtual DbSet<UsersHasMessages> UsersHasMessages { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=advert_site;Trusted_Connection=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });



            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Listingid).HasColumnName("listingid");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .IsUnicode(false);

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Listingid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Comments_Listings11");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Comments_Users11");

            });

            modelBuilder.Entity<ListingPictures>(entity =>
            {
                entity.HasKey(e => new { e.PictureId, e.ListingId });

                entity.ToTable("Listing_pictures");

                entity.Property(e => e.PictureId).HasColumnName("picture_id");

                entity.Property(e => e.ListingId).HasColumnName("Listing_id");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.ListingPictures)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Listing_pictures_Listings1");
            });

            modelBuilder.Entity<Listings>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Display).HasColumnName("display");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Subcategoryid).HasColumnName("subcategoryid");

                entity.Property(e => e.Userid)
                    .IsRequired()
                    .HasColumnName("userid")
                    .HasMaxLength(450);

                entity.Property(e => e.Verified).HasColumnName("verified");

                entity.HasOne(d => d.Subcategory)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.Subcategoryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Listings_Subcategory1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Listings_Users1");
            });

            modelBuilder.Entity<Messages>(entity =>
            {
                entity.HasKey(e => new { e.Id });

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();


                entity.Property(e => e.Subject)
                    .HasColumnName("subject")
                    .IsUnicode(false);

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .IsUnicode(false);


                entity.Property(e => e.DateSent)
                .HasColumnName("dateSent")
                .HasColumnType("datetime2(0)");

            });

            modelBuilder.Entity<Reviews>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Buyerid)
                    .IsRequired()
                    .HasColumnName("buyerid")
                    .HasMaxLength(450);

                entity.Property(e => e.Comment)
                    .HasColumnName("comment")
                    .IsUnicode(false);

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime2(0)");

                entity.Property(e => e.Evaluation).HasColumnName("evaluation");

                entity.Property(e => e.Sellerid)
                    .IsRequired()
                    .HasColumnName("sellerid")
                    .HasMaxLength(450);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.ReviewsBuyer)
                    .HasForeignKey(d => d.Buyerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id");

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ReviewsSeller)
                    .HasForeignKey(d => d.Sellerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("id1");
            });

            modelBuilder.Entity<Subcategory>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Subcategory)
                    .HasForeignKey(d => d.Categoryid)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_Subcategory_Category1");
            });

            modelBuilder.Entity<UsersHasMessages>(entity =>
            {
                entity.HasKey(e => new { e.RecipientId, e.MessagesId, e.SenderId });

                entity.ToTable("Users_has_Messages");

                entity.Property(e => e.RecipientId).HasColumnName("recipient_id");

                entity.Property(e => e.MessagesId).HasColumnName("Messages_id");

                entity.Property(e => e.SenderId).HasColumnName("Messages_sender_id");

                entity.Property(e => e.AlreadyRead).HasColumnName("alreadyRead");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsAdminMessage).HasColumnName("isAdminMessage");

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.ReceivedMessages)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Users_has_Messages_Users1");

                entity.HasOne(d => d.Messages)
                    .WithMany(p => p.UsersHasMessages)
                    .HasForeignKey(d => new { d.MessagesId })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Users_has_Messages_Messages1");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.SentMessages)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Users_has_Messages_Users2");
            });
        }

    }
}
