using Microsoft.EntityFrameworkCore;
using SyrusVoluntariado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Database {
    public class DatabaseContext : DbContext {

        //public DbSet<Palavra> Palavras { get; set; }
        public DbSet<Vaga> Vagas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){
            //EF - Garantir que o banco seja criado pelo EF - Code First
            Database.EnsureCreated();

        }
    }
}
