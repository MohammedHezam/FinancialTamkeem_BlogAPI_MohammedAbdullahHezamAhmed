﻿using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogManagementSystem.Models
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
            //try
            //{
            //    var databaseCreater = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            //    if (databaseCreater != null)
            //    {
            //        //if (!databaseCreater.CanConnect())
            //        //{
            //        //    //databaseCreater.Create();
            //        //    databaseCreater.EnsureCreated();
            //        //}
            //        //if (!databaseCreater.HasTables())
            //        //{
            //        //    databaseCreater.CreateTables();
            //        //}

            //        //var createScript = databaseCreater.GenerateCreateScript();

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

          

        }



        public DbSet<BlogPost>? BlogPost { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}