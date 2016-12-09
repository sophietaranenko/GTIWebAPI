using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GTIWebAPI.Models.Service;
using GTIWebAPI.Models.Employees;
using GTIWebAPI.Models.Clients;
using GTIWebAPI.Models.Accounting;
using GTIWebAPI.Models.Dictionary;

namespace GTIWebAPI.Models.Context 
{
    public class DbClient: DbContext
    {
        public DbClient()
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345")
        {
        }

        public static DbClient Create()
        {
            return new DbClient();
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<ClientGTI> ClientGTI { get; set; }
        public virtual DbSet<ClientAccount> ClientAccount { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<ClientBank> ClientBank { get; set; }
        public virtual DbSet<ClientAgreement> ClientAgreement { get; set; }
        public virtual DbSet<ClientContact> ClientContact { get; set; }
        public virtual DbSet<ClientFile> ClientFile { get; set; }
        public virtual DbSet<ClientGTIClient> ClientGTIClient { get; set; }
        public virtual DbSet<ClientContainer> ClientContainer { get; set; }
        public virtual DbSet<ShippingLine> ShippingLine { get; set; }
        public virtual DbSet<EmployeeGTI> EmployeeGTI { get; set; }
        public virtual DbSet<Vessel> Vessel { get; set; }
        public virtual DbSet<Incoterms> Incoterms { get; set; }
        public virtual DbSet<Deal> Deal { get; set; }
        public virtual DbSet<DealType> DealType { get; set; }
        public virtual DbSet<Office> Office { get; set; }
        public virtual DbSet<Terminal> Terminal { get; set; }
        public virtual DbSet<ClientPhoto> ClientPhoto { get; set; }

        public virtual DbSet<ClientView> ClientView { get; set; }

        //public virtual int NewId(string tableName)
        //{
        //    SqlParameter table = new SqlParameter("@table_name", tableName);
        //    int result = this.Database.SqlQuery<int>("exec table_id @table_name", table).FirstOrDefault();
        //    return result;
        //}
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Terminal>()
                .Property(e => e.Id)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Terminal>()
                .Property(e => e.Name)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DealType>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Name)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Type)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Remark)
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Platform)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Weight)
                .HasPrecision(12, 3);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.TerminalId)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.Seal)
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.MRNCode)
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.PolicyNo)
                .IsUnicode(false);

            modelBuilder.Entity<ClientContainer>()
                .Property(e => e.DocumentNo)
                .IsUnicode(false);

            modelBuilder.Entity<ShippingLine>()
                .Property(e => e.Name)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EmployeeGTI>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<EmployeeGTI>()
                .Property(e => e.NativeName)
                .IsFixedLength();

            modelBuilder.Entity<Vessel>()
                .Property(e => e.VesselName)
                .IsFixedLength();

            modelBuilder.Entity<Vessel>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Incoterms>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Deal>()
              .Property(e => e.Cargo)
              .IsUnicode(false);

            modelBuilder.Entity<Deal>()
                .Property(e => e.PortOfOrigin)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Deal>()
                .Property(e => e.PortOfLoading)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Deal>()
                .Property(e => e.PortOfDestination)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Deal>()
                .Property(e => e.FinalDestination)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}