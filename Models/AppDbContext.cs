using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace CREDAJAX.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<studint> Studints { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        
        
        }

       


      
    }
}

   

