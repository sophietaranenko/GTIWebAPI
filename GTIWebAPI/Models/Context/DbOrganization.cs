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
    /// <summary>
    /// Db set for organization users (contains info about organizations and what they need to read to show them) 
    /// Rights to add and update deal document scans, show all other information 
    /// </summary>
    public class DbOrganization : DbContext
    {

        public DbOrganization()
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345")
        {
        }

        public static DbOrganization Create()
        {
            return new DbOrganization();
        }

        public virtual DbSet<Address> Addresses { get; set; }

        public virtual DbSet<OrganizationGTI> GTIOrganizations { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<OrganizationBank> OrganizationBanks { get; set; }

        public virtual DbSet<OrganizationAgreement> OrganizationAgreements { get; set; }

        public virtual DbSet<OrganizationContact> OrganizationContacts { get; set; }

        public virtual DbSet<OrganizationFile> OrganizationFiles { get; set; }

        public virtual DbSet<OrganizationGTILink> OrganizationGTILinks { get; set; }

        public virtual DbSet<ShippingLine> ShippingLines { get; set; }

        public virtual DbSet<EmployeeGTI> GTIEmployees { get; set; }

        public virtual DbSet<Vessel> Vessels { get; set; }

        public virtual DbSet<Incoterms> Incoterms { get; set; }

        public virtual DbSet<Deal> Deals { get; set; }

        public virtual DbSet<DealType> DealTypes { get; set; }

        public virtual DbSet<Office> Offices { get; set; }

        public virtual DbSet<Terminal> Terminals { get; set; }

        public virtual DbSet<OrganizationPhoto> OrganizationPhotos { get; set; }

        public virtual DbSet<OrganizationType> OrganizationTypes { get; set; }

        public virtual DbSet<OrganizationSigner> OrganizationSigners { get; set; }

        public virtual DbSet<OrganizationTaxInfo> OrganizationTaxInfos { get; set; }

        public virtual DbSet<SignerPosition> SignerPositions { get; set; }

        public virtual DbSet<OrganizationStatistic> OrganizationStatistics { get; set; }

        /// <summary>
        /// Stored procedure call that filters organization view by text filter (filter parameters separated by space symbols) 
        /// </summary>
        /// <param name="myFilter"></param>
        /// <returns></returns>
        public IEnumerable<OrganizationViewDTO> OrganizationsFilter(string myFilter)
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
            List<OrganizationViewDTO> organizationList = new List<OrganizationViewDTO>();
            try
            {
                var result = Database.SqlQuery<OrganizationViewDTO>("exec OrganizationsFilter @filter", parameter).ToList();
                organizationList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return organizationList;
        }

        /// <summary>
        /// Stored procedure call to return GTI VFP Organizations linked to current WEB API Organization 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns>List of linked GTI VFP Organization (klient table)</returns>
        public IEnumerable<OrganizationGTILinkView> OrganizationGTILinkList(int organizationId)
        {

            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            IEnumerable<OrganizationGTILinkView> organizationList = new List<OrganizationGTILinkView>();
            try
            {
                var result = Database.SqlQuery<OrganizationGTILinkView>("exec OrganizationGTILinkList @ClientId", parameter).ToList();
                organizationList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return organizationList;
        }

        /// <summary>
        /// Deals linked to currency WEB API Organization, selected by their creation date 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="dateBegin">Begin date, if not seleted - 01/01/1900</param>
        /// <param name="dateEnd">End date, if not</param>
        /// <returns></returns>
        public IEnumerable<DealShortViewDTO> DealList(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime dateBeginParameter = dateBegin.GetValueOrDefault();
            DateTime dateEndParameter = dateEnd.GetValueOrDefault();

            SqlParameter parOrganization = new SqlParameter
            {
                ParameterName = "@OrganizationId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBeginParameter
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEndParameter
            };

            IEnumerable<DealShortViewDTO> dealList = new List<DealShortViewDTO>();
            try
            {
                var result = Database.SqlQuery<DealShortViewDTO>("exec DealsFilter @OrganizationId, @DateBegin, @DateEnd", parOrganization, parBegin, parEnd).ToList();
                dealList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dealList;
        }

        /// <summary>
        /// Stored procedure call to return invoice list selected by organization and dates 
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns>List of invoices</returns>
        public IEnumerable<InvoiceViewDTO> InvoicesList(int organizationId, DateTime? dateBegin, DateTime? dateEnd)
        {
            if (dateBegin == null)
            {
                dateBegin = new DateTime(1900, 1, 1);
            }
            if (dateEnd == null)
            {
                dateEnd = new DateTime(2200, 1, 1);
            }
            DateTime dateBeginParameter = dateBegin.GetValueOrDefault();
            DateTime dateEndParameter = dateEnd.GetValueOrDefault();

            SqlParameter parClient = new SqlParameter
            {
                ParameterName = "@ClientId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = organizationId
            };

            SqlParameter parBegin = new SqlParameter
            {
                ParameterName = "@DateBegin",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateBeginParameter
            };

            SqlParameter parEnd = new SqlParameter
            {
                ParameterName = "@DateEnd",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.DateTime,
                Value = dateEndParameter
            };

            IEnumerable<InvoiceViewDTO> dealList = new List<InvoiceViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceViewDTO>("exec InvoicesList @OrganizationId, @DateBegin, @DateEnd", parClient, parBegin, parEnd).ToList();
                dealList = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dealList;
        }


        /// <summary>
        /// Stored procedure call to return current deal data
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>Deal info</returns>
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
                var result = Database.SqlQuery<DealFullViewDTO>("exec DealInfo @DealId", parameter).FirstOrDefault();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        /// <summary>
        /// Selects containers by current deal 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>List of containers</returns>
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
                var result = Database.SqlQuery<DealContainerViewDTO>("exec DealContainersList @DealId", parameter).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        /// <summary>
        /// Selects invoices by current deal using 
        /// </summary>
        /// <param name="dealId"></param>
        /// <returns>list of invoices</returns>
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
                @"exec DealInvoicesList @DealId", parameter).OrderBy(d => d.InvoiceDate).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        /// <summary>
        /// Select view InvoiceLines by invoiceId
        /// </summary>
        /// <param name="invoiceId">Invoice Id</param>
        /// <returns>List of invoice lines</returns>
        public IEnumerable<InvoiceLineViewDTO> InvoiceLinesByInvoice(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            IEnumerable<InvoiceLineViewDTO> dto = new List<InvoiceLineViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceLineViewDTO>(
                @"SELECT ServiceType, 
                ServiceName, 
                ServiceSum, 
                UsdSum, 
                CurrencySum, 
                VatId, 
                VatInPersent,  
                VatSum, 
                ServiceTypeId, 
                Id,
                LinePosition,  
                InvoiceId,  
               from InvoiceLines where InvoiceId = @InvoiceId", parameter).OrderBy(l => l.LinePosition).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        public IEnumerable<InvoiceContainerViewDTO> ContainersByInvoice(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            IEnumerable<InvoiceContainerViewDTO> dto = new List<InvoiceContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceContainerViewDTO>(
                @"select c.container as Name, c.type as Type, 
                    m.bl as BL, c.ves_brutto as BruttoWeight,
                    c.bl as BLId, c.rc as Id, c.account as InvoiceId 
                    from cntr c
                    left join manifest m 
                    on m.kod = c.bl
                    where c.pr_delete = 0 and c.act='INV ' and c.account = @InvoiceId
                    order by c.container", parameter).OrderBy(l => l.Name).ToList();
                dto = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return dto;
        }

        public IEnumerable<InvoiceContainerViewDTO> ContainersByInvoiceIdView(int invoiceId)
        {
            SqlParameter parameter = new SqlParameter
            {
                ParameterName = "@InvoiceId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.Int32,
                Value = invoiceId
            };
            IEnumerable<InvoiceContainerViewDTO> dto = new List<InvoiceContainerViewDTO>();
            try
            {
                var result = Database.SqlQuery<InvoiceContainerViewDTO>(
                @"select Name,Type, 
                    BL, BruttoWeight,
                    BLId, Id, InvoiceId 
                    from InvoiceContainers 
                    where InvoiceId = @InvoiceId", parameter).OrderBy(l => l.Name).ToList();
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

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Name)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Type)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Remark)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Platform)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Weight)
            //    .HasPrecision(12, 3);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.TerminalId)
            //    .IsFixedLength()
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.Seal)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.MRNCode)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.PolicyNo)
            //    .IsUnicode(false);

            //modelBuilder.Entity<ClientContainer>()
            //    .Property(e => e.DocumentNo)
            //    .IsUnicode(false);

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