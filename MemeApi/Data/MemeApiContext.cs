using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MemeApi.Models
{
    public class MemeApiContext : DbContext
    {
        public MemeApiContext (DbContextOptions<MemeApiContext> options)
            : base(options)
        {
        }

        public DbSet<MemeApi.Models.MemeItem> MemeItem { get; set; }
    }
}
