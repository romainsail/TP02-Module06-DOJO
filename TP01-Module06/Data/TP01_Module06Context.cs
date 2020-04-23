using BO;
using Module06_BO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TP01_Module06.Data
{
    public class TP01_Module06Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public TP01_Module06Context() : base("name=TP01_Module06Context")
        {
        }

        public System.Data.Entity.DbSet<BO.Samourai> Samourais { get; set; }

        public System.Data.Entity.DbSet<BO.Arme> Armes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //une Arme ne peut appartenir qu’à un seul samouraï.
            modelBuilder.Entity<Samourai>().HasOptional(s => s.Arme).WithOptionalPrincipal();

            //Un art martial peut être associé à zéro ou plusieurs samouraïs.
            //Un samourai peut avoir 0 ou plusieurs art martials & un art martial peut etre associé à 0 ou +sieurs samourais
            modelBuilder.Entity<Samourai>().HasMany(s => s.ArtMartials).WithMany();

            modelBuilder.Entity<Samourai>().Ignore(s => s.Potentiel); //Ignore en BDD la propriete Potentiel, juste pour de l'affichage

            base.OnModelCreating(modelBuilder);

        }

        public System.Data.Entity.DbSet<Module06_BO.ArtMartial> ArtMartials { get; set; }
    }
}
