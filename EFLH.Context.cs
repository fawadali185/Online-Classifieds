﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ListHell
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class LH_newEntities : DbContext
    {
        public LH_newEntities()
            : base("name=LH_newEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<CustomRole> CustomRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<log> logs { get; set; }
        public virtual DbSet<ad> ads { get; set; }
    
        public virtual int test321()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("test321");
        }
    
        public virtual ObjectResult<test32121_Result> test32121()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<test32121_Result>("test32121");
        }
    
        public virtual int tes786()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("tes786");
        }
    
        public virtual ObjectResult<Nullable<int>> tes786_1()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("tes786_1");
        }
    
        public virtual ObjectResult<tes786_2_3333> tes786_2()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<tes786_2_3333>("tes786_2");
        }
    }
}