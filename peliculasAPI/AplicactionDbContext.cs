﻿using Microsoft.EntityFrameworkCore;
using peliculasAPI.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace peliculasAPI
{
    public class AplicactionDbContext : DbContext
    {
        public AplicactionDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }

}
