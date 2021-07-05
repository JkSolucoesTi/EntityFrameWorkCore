using Microsoft.EntityFrameworkCore;
using System;

namespace DevIo.Entity.FW
{
    internal class ProgramBase
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
            MapearPropriedades(modelBuilder);
        }

        internal void OnModelCreating(ModelBuilder modelBuilder)
        {
            throw new NotImplementedException();
        }
    }
}