using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UIToolTrealet.Models;

namespace UIToolTrealet.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UIToolTrealet.Models.Image> Image { get; set; }
        public DbSet<UIToolTrealet.Models.Video> Video { get; set; }
        public DbSet<UIToolTrealet.Models.Interaction> Interaction { get; set; }
        public DbSet<UIToolTrealet.Models.Item> Item { get; set; }
        public DbSet<UIToolTrealet.Models.Info> Info { get; set; }
        public DbSet<UIToolTrealet.Models.Page> Page { get; set; }
    }
}
