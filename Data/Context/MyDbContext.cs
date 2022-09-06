using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class MyDbContext : DbContext
    {
        public MyDbContext( DbContextOptions<MyDbContext> options ) : base( options )
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
