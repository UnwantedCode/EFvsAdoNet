using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using EFvsAdoNet.Models;

namespace EFvsAdoNet
{
    internal class DatabaseEFContext : DbContext
    {
        public DatabaseEFContext()
        {
        }

        public DbSet<Pracownik> Pracownicy { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Tu możesz dodać konfigurację
        }

    }
}
