using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheCarHub.Models;

namespace TheCarHub.Data
{
    public class CarHubContext : DbContext
    {
        public CarHubContext(DbContextOptions<CarHubContext> options): base(options)
        {
        }
        public DbSet<Car> Cars { get; set; }
    }
}
