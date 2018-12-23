using System;
using System.Collections.Generic;
using System.Text;
using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdvertSite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsersHasMessages>().HasKey(item => new { item.RecipientId, item.MessagesSenderId, item.MessagesId });
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<ListingPictures> ListingPictures { get; set; }
        public virtual DbSet<Listings> Listings { get; set; }
        public virtual DbSet<Messages> Messages { get; set; }
        public virtual DbSet<Reviews> Reviews { get; set; }
        public virtual DbSet<Subcategory> Subcategory { get; set; }
        public virtual DbSet<IdentityUser> Users { get; set; }
        public virtual DbSet<UsersHasMessages> UsersHasMessages { get; set; }
    }
}
