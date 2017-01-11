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
using System.Data;

namespace GTIWebAPI.Models.Context
{
    public class DbClient : DbContext
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

        public virtual DbSet<OrganizationType> OrganizationType { get; set; }

        public virtual DbSet<ClientSigner> ClientSigner { get; set; }

        public virtual DbSet<ClientTaxInfo> ClientTaxInfo { get; set; }

        public virtual DbSet<SignerPosition> SignerPosition { get; set; }

        public virtual DbSet<ClientView> ClientView { get; set; }

        public IEnumerable<ClientViewDTO> ClientFilter(string myFilter)
        {
            if (myFilter == null)
                myFilter = "";
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@filter",
                IsNullable = true,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Size = 1000,
                Value = myFilter
            };
            List<ClientViewDTO> clientList = new List<ClientViewDTO>();
            try
            {
                var result = Database.SqlQuery<ClientViewDTO>("exec ClientFilter @filter", parameter).ToList();
                clientList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return clientList;
        }


        public IEnumerable<ClientGTIClientView> ClientGTIClientList(int clientId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@ClientId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = clientId
            };

            IEnumerable<ClientGTIClientView> clientList = new List<ClientGTIClientView>();
            try
            {
                var result = Database.SqlQuery<ClientGTIClientView>("exec ClientGTIClientList @ClientId", parameter).ToList();
                clientList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return clientList;
        }

        public IEnumerable<DealShortViewDTO> DealList(int clientId, DateTime dateBegin, DateTime dateEnd)
        {
            SqlParameter parClient = new SqlParameter
            {
                ParameterName = "@ClientId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = clientId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBegin
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEnd
            };

            IEnumerable<DealShortViewDTO> dealList = new List<DealShortViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealShortViewDTO>("exec ClientGTIClientList @ClientId, @DateBegin, @DateEnd", parClient, parBegin, parEnd).ToList();
                dealList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dealList;
        }


        public string ClientUserNameGenerator()
        {
            string result = "";
            try
            {
                var sqlResult = Database.SqlQuery<int>("exec ClientUserNameGenerator").FirstOrDefault();
                result = "customer_" + sqlResult;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return result;
        }

        public DealFullViewDTO DealCardInfo(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            DealFullViewDTO dto = new DealFullViewDTO();
            try
            {
                var result = Database.SqlQuery<DealFullViewDTO>(
                @"SELECT FullNumber,
                        Number,
                        CreateDate,
                        Direction,
                        DirectionId,
                        Cargo,
                        ShippingLineId,
                        ShippingLine,
                        POO,
                        POL,
                        POD,
                        FD,
                        Office,
                        OfficeId,
                        ClientGTIId,
                        ClientId,
                        ForwarderGTIId,
                        ForwarderId,
                        ManagerGTIId,
                        ManagerId,
                        MaganerPicture,
                        AssistantGTIId,
                        AssistantId,
                        AssistantPicture,
                        StatusChar,
                        StatusId,
                        DealType,
                        DealTypeId,
                        Incoterms,
                        IncotermsId,
                        FeederVessel,
                        FeederVesselId,
                         OceanVessel,
                        OceanVesselId,
                        POT,
                        Id from dbo.DealView where Id = @DealId", parameter).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        public IEnumerable<DealContainerViewDTO> ContainersByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            IEnumerable<DealContainerViewDTO> dto = new List<DealContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealContainerViewDTO>(
                @"SELECT Container, 
                    Type, 
                    ReadinessDate, 
                    ETSPOL, 
                    ETAPOT, 
                    Terminal, 
                    ETAPOD, 
                    DocumentNo, 
                    RegistrationDate, 
                    TerminalHandlingDate, 
                    ClearanceDate, 
                    GateOutFullDate, 
                    PlatformTruck, 
                    PolicyNo, 
                    PolicyDate, 
                    DeliveryDate, 
                    GateInEmptyDate, 
                    Seal,
                    MRN,
                    Weight,
                    Id,
                    DealId
                from ContainerView where DealId = @DealId", parameter).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }


        public IEnumerable<InvoiceViewDTO> InvoicesByDeal(Guid dealId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@DealId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Guid,
                Value = dealId
            };

            IEnumerable<InvoiceViewDTO> dto = new List<InvoiceViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceViewDTO>(
                @"SELECT DealId,
                        CurrencyName, 
                        CurrencyId, 
                        PaymentMode, 
                        PaymentModeId, 
                        Number,
                        OfficialNumber,
                        InvoiceDate,
                        PayerName, 
                        ClientGTIId, 
                        ClientId, 
                        RE,
                        Voyage, 
                        USD,
                        CurrencySum,
                        Status,
                        DebtUSD,
                        DebtCurrency,
                        DebtVat,
                        TotalUsdIncome, 
                        DebtVatUsd, 
                        InvoiceId, 
                        Id,
                        Cancelled,
                        Paid,
                        PaidDate
               from Invoices where DealId = @DealId", parameter).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }




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