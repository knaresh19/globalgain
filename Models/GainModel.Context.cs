﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GAIN.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GainEntities : DbContext
    {
        public GainEntities()
            : base("name=GainEntities")
        {
        }

        public GainEntities(string connectionstring):base(connectionstring)
        {

        }
    
          
        public virtual DbSet<logtable> logtables { get; set; }
        public virtual DbSet<maccess_menu> maccess_menu { get; set; }
        public virtual DbSet<mactiontype> mactiontypes { get; set; }
        public virtual DbSet<mbrand> mbrands { get; set; }
        public virtual DbSet<mbrandcountry> mbrandcountries { get; set; }
        public virtual DbSet<mcluster> mclusters { get; set; }
        public virtual DbSet<mcostcontrolsite> mcostcontrolsites { get; set; }
        public virtual DbSet<mcosttype> mcosttypes { get; set; }
        public virtual DbSet<mcountry> mcountries { get; set; }
        public virtual DbSet<mlegalentity> mlegalentities { get; set; }
        public virtual DbSet<mmenu> mmenus { get; set; }
        public virtual DbSet<mport> mports { get; set; }
        public virtual DbSet<mregion> mregions { get; set; }
        public virtual DbSet<mregional_office> mregional_office { get; set; }
        public virtual DbSet<mrole> mroles { get; set; }
        public virtual DbSet<msavingtype> msavingtypes { get; set; }
        public virtual DbSet<msourcecategory> msourcecategories { get; set; }
        public virtual DbSet<mstatu> mstatus { get; set; }
        public virtual DbSet<msubcost> msubcosts { get; set; }
        public virtual DbSet<msubcountry> msubcountries { get; set; }
        public virtual DbSet<msubregion> msubregions { get; set; }
        public virtual DbSet<msynimpact> msynimpacts { get; set; }
        public virtual DbSet<myear> myears { get; set; }
        public virtual DbSet<t_cost_actiontype> t_cost_actiontype { get; set; }
        public virtual DbSet<t_initiative> t_initiative { get; set; }
        public virtual DbSet<t_subcostactiontype> t_subcostactiontype { get; set; }
        public virtual DbSet<t_subcostbrand> t_subcostbrand { get; set; }
        public virtual DbSet<t_subcostinitiative> t_subcostinitiative { get; set; }
        public virtual DbSet<t_subctry_costcntrlsite> t_subctry_costcntrlsite { get; set; }
        public virtual DbSet<user_list> user_list { get; set; }

        public virtual DbSet<t_initiative_calcs> t_initiative_calcs { get; set; }
        public virtual DbSet<mcpi> mcpi { get; set; }

        public virtual DbSet<vwagency> vwagencies { get; set; }
        public virtual DbSet<vwheaderinitiative> vwheaderinitiatives { get; set; }
        public virtual DbSet<vwsummarydashboard> vwsummarydashboards { get; set; }
        public virtual DbSet<vwsummarydashboarddetail> vwsummarydashboarddetails { get; set; }
        public virtual DbSet<vwtopcategory> vwtopcategories { get; set; }
        public virtual DbSet<vwtypeofsaving> vwtypeofsavings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Legal Entity Refrence
            modelBuilder.Entity<mbrand>()
               .HasOptional(j => j.mlegalentities)
               .WithMany()
               .HasForeignKey(j=>j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<mcostcontrolsite>()
               .HasOptional(j => j.mlegalentities)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<mcountry>()
               .HasOptional(j => j.mlegalentities)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubcountry>()
               .HasOptional(j => j.mlegalentities)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            
            //For mCountry
            modelBuilder.Entity<mregion>()
               .HasOptional(j => j.mcountries)
               .WithMany()
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubregion>()
              .HasOptional(j => j.mcountries)
              .WithMany()
              .WillCascadeOnDelete(true);
            
            //For mSubCountry
            modelBuilder.Entity<mcountry>()
               .HasOptional(j => j.msubcountries)
               .WithMany()
               .WillCascadeOnDelete(true);
            
            //For mSubRegion 
            modelBuilder.Entity<mregion>()
              .HasOptional(j => j.msubregions)
              .WithMany()
              .WillCascadeOnDelete(true);

            //For Regional Office
            modelBuilder.Entity<mbrand>()
               .HasOptional(j => j.mregional_office)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<mregion>()
               .HasOptional(j => j.mregional_office)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<mcountry>()
               .HasOptional(j => j.mregional_office)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubcountry>()
               .HasOptional(j => j.mregional_office)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);

            //FOr mBrandCountry
            modelBuilder.Entity<mcountry>()
               .HasOptional(j => j.mbrandcountries)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<mbrand>()
               .HasOptional(j => j.mbrandcountries)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubcountry>()
               .HasOptional(j => j.mbrandcountries)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);

            //mCluster Table
            modelBuilder.Entity<mregion>()
               .HasOptional(j => j.mclusters)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubregion>()
               .HasOptional(j => j.mclusters)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);

            //t_initative
            modelBuilder.Entity<mcountry>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mbrand>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mcluster>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mcostcontrolsite>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mlegalentity>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mport>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mregion>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mstatu>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubcountry>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<msubregion>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);
            modelBuilder.Entity<mregional_office>()
              .HasOptional(j => j.t_initiative)
              .WithMany()
              .HasForeignKey(j => j.id)
              .WillCascadeOnDelete(true);

            //modelBuilder.Entity<t_initiative>()
            //  .HasOptional(j => j.t_initiative_calcs)
            //  .WithMany()
            //  .HasForeignKey(j => j.id)
            //  .WillCascadeOnDelete(true);

            //FOr mCPI
            modelBuilder.Entity<mcountry>()
               .HasOptional(j => j.mcpi)
               .WithMany()
               .HasForeignKey(j => j.id)
               .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);

            //throw new UnintentionalCodeFirstException();
        }
    }
}
