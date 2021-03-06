﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations.Schema;
using GTIWebAPI.Models.Security;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using GTIWebAPI.Models.Dictionary;
using System.Linq;
using GTIWebAPI.Models.Service;
using System.Net.Mail;
using System;
using System.Net.Mime;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using GTIWebAPI.Models.Context;
using GTIWebAPI.NovelleDirectory;
using System.Security.Principal;

namespace GTIWebAPI.Models.Account
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// User of application
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() : base()
        {
            UserRights = new List<UserRight>();
        }

        //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        //    // Add custom user claims here
        //    userIdentity.AddClaim(new Claim("GroupWiseSessionId", this.GroupWiseSessionId));

        //    return userIdentity;
        //}




        public string TableName { get; set; }

        public int TableId { get; set; }

        [NotMapped]
        public string GroupWiseSessionId { get; set; }

        [NotMapped]
        public string PostOfficeAddress { get; set; }

        public virtual ICollection<UserRight> UserRights { get; set; }

        public List<UserRightDTO> GetUserRightsDTO()
        {
            List<UserRightDTO> dtos = new List<UserRightDTO>();
            try
            {
                if (UserRights != null)
                {
                    if (UserRights.Count != 0)
                    {
                        var result = UserRights.Select(r => r.OfficeSecurity).Distinct().ToList();
                        if (result != null)
                        {
                            foreach (var item in result)
                            {
                                UserRightDTO dto = new UserRightDTO();
                                dto.OfficeId = item.Id;
                                dto.OfficeName = item.ShortName;

                                List<ControllerBoxDTO> boxesList = new List<ControllerBoxDTO>();
                                var bList = UserRights
                                    .Where(d => d.OfficeId == item.Id)
                                    .Select(d => d.Action.Controller.ControllerBox)
                                    .Distinct()
                                    .ToList();

                                if (bList != null)
                                {
                                    foreach (var box in bList)
                                    {

                                        ControllerBoxDTO boxDTO = new ControllerBoxDTO();
                                        boxDTO.Name = box.Name;
                                        boxDTO.Id = box.Id;

                                        List<ControllerDTO> controllerList = new List<ControllerDTO>();
                                        var cList = UserRights.Where(r => r.OfficeId == item.Id && r.Action.Controller.BoxId == box.Id).Select(r => r.Action.Controller).Distinct().ToList();

                                        if (cList != null)
                                        {
                                            foreach (var c in cList)
                                            {
                                                ControllerDTO cDto = new ControllerDTO();
                                                cDto.ControllerName = c.Name;
                                                cDto.Id = c.Id;

                                                List<ActionDTO> actionList = new List<ActionDTO>();
                                                var aList = UserRights.Where(r => r.OfficeId == item.Id && r.Action.ControllerId == c.Id).Select(r => r.Action).Distinct().ToList();
                                                if (aList != null)
                                                {
                                                    foreach (var a in aList)
                                                    {
                                                        ActionDTO aDto = new ActionDTO();
                                                        aDto.Id = a.Id;
                                                        aDto.ActionLongName = a.LongName == null ? "" : a.LongName;
                                                        aDto.ActionName = a.Name == null ? "" : a.Name;
                                                        actionList.Add(aDto);
                                                    }
                                                }
                                                cDto.Actions = actionList;
                                                controllerList.Add(cDto);
                                            }
                                        }

                                        boxDTO.Controllers = controllerList;
                                        boxesList.Add(boxDTO);
                                    }
                                }
                                dto.Boxes = boxesList;
                                dtos.Add(dto);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                string m = e.Message;
            }
            return dtos;
        }

        /// <summary>
        /// method that creates a new Claims Identity 
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="authenticationType"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            if (this.GroupWiseSessionId != null)
            {
                userIdentity.AddClaim(new Claim("GroupWiseSessionId", this.GroupWiseSessionId));
            }
            if (this.PostOfficeAddress != null)
            {
                userIdentity.AddClaim(new Claim("PostOfficeAddress", this.PostOfficeAddress));
            }
            //if (this.PostOfficeAddress != null)
            //{
            //    userIdentity.AddClaim(new Claim("Id", this.Id));
            //}
            return userIdentity;
        }
    }

    public static class IdentityExtensions
    {
        public static string GetSessionId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("GroupWiseSessionId");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetPostOfficeAddress(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("PostOfficeAddress");
            return (claim != null) ? claim.Value : string.Empty;
        }

        //public static string GetUserId(this IIdentity identity)
        //{
        //    return identity.GetUserId();
        //}
    }


    public interface IApplicationDbContext : IDisposable
    {

        DbSet<Controller> Controllers { get; set; }

        DbSet<Security.Action> Actions { get; set; }

        bool CreateHoldingUser(string email, string password);

        bool CreateOrganization(string email, string password);

        string GetFullUserName(string userId);

        bool GrantRightsToOrganization(string userId);

        bool GrantStandardRightsToPersonnel(string userId);
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext()
            : base("name=DbUsers", throwIfV1Schema: false)
        {
            Database.SetInitializer<MainDbContext>(null);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IEnumerable<T> ExecuteStoredProcedure<T>(string query, params object[] parameters)
        {
            return this.Database.SqlQuery<T>(query, parameters).ToList();
        }

        public DbSet<Controller> Controllers { get; set; }

        public DbSet<Security.Action> Actions { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Security.Action>()
                .HasRequired<Controller>(s => s.Controller)
                .WithMany(s => s.Actions);


            modelBuilder.Entity<UserRight>()
                .HasRequired<GTIWebAPI.Models.Security.Action>(s => s.Action)
                .WithMany(s => s.UserRights);

            modelBuilder.Entity<UserRight>()
                   .HasRequired<Service.OfficeSecurity>(s => s.OfficeSecurity)
                   .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                   .HasRequired<ApplicationUser>(s => s.ApplicationUser)
                   .WithMany(s => s.UserRights);

            modelBuilder.Entity<Controller>()
                .HasRequired<ControllerBox>(s => s.ControllerBox)
                .WithMany(d => d.Controllers);
        }

        public bool CreateHoldingUser(string email, string password)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Username",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = email
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec CreateDatabaseHoldingUser @Username ",
                    pEmail
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }


        public bool CreateOrganization(string email, string password)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Username",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = email
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec CreateDatabaseExternalUser @Username ",
                    pEmail
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public string GetFullUserName(string userId)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@AspNetUserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };

            string methodResult = "";

            try
            {
                var result = Database.SqlQuery<string>("exec GetFullAspNetUserName @AspNetUserId",
                    pEmail
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public bool GrantRightsToOrganization(string userId)
        {
            SqlParameter pUserId = new SqlParameter
            {
                ParameterName = "@AspNetUserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec GrantAspNetUserRightsForOrganization @AspNetUserId ",
                    pUserId
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

        public bool GrantStandardRightsToPersonnel(string userId)
        {
            SqlParameter pUserId = new SqlParameter
            {
                ParameterName = "@AspNetUserId",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = userId
            };

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec GrantAspNetUserStandardRightsForPersonnel @AspNetUserId ",
                    pUserId
                    ).FirstOrDefault();
                methodResult = result;
            }
            catch (Exception e)
            {
                string error = e.ToString();
            }
            return methodResult;
        }

    }

    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var email =
               new MailMessage(new MailAddress(Properties.Settings.Default.SMTPEmailAddress, "(do not reply)"),
               new MailAddress(message.Destination))
               {
                   Subject = message.Subject,
                   Body = message.Body,
                   IsBodyHtml = true
               };
            try
            {
                using (var client = new SmtpClient(Properties.Settings.Default.SMTPIPAddress, 25)) // SmtpClient configuration comes from config file
                {
                    client.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.SMTPEmailAddress, Properties.Settings.Default.SMTPEmailPassword);
                    await client.SendMailAsync(email);
                }
            }
            catch (Exception e)
            {
                string m = e.Message;
            }
        }
    }
}