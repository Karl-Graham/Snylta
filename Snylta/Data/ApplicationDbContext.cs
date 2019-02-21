using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Snylta.Models;

namespace Snylta.Data
{
    public class ApplicationDbContext : IdentityDbContext<User,Roles,string,IdentityUserClaim<string>, UserRoles, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GroupThings>()
                .HasKey(groupThings => new { groupThings.GroupId, groupThings.ThingId });

            modelBuilder.Entity<GroupUsers>()
                .HasKey(groupUsers => new { groupUsers.GroupId, groupUsers.UserId });

            modelBuilder.Entity<UserRoles>()
                .HasKey(userRoles => new { userRoles.GroupId, userRoles.RoleId, userRoles.UserId });
        }

        public DbSet<Snylta.Models.Group> Group { get; set; }

        public DbSet<Snylta.Models.Thing> Thing { get; set; }

        public DbSet<Snylta.Models.Snyltning> Snyltning { get; set; }

        public DbSet<Snylta.Models.ThingPic> ThingPic { get; set; }
    }
}
