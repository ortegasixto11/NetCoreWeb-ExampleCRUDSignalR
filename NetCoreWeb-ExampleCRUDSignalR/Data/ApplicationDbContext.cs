using Microsoft.EntityFrameworkCore;
using NetCoreWeb_ExampleCRUDSignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWeb_ExampleCRUDSignalR.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}
