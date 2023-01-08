using Entites;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog_DataAccessLayer.EntityFrameworkSQL
{
    public class BlogContext : DbContext 
    {
        public DbSet<BlogUser> BlogUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public BlogContext()
        {
            Database.SetInitializer(new MyDbInitializer());
        }
    }
}
