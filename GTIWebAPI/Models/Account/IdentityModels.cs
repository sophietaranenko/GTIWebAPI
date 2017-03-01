using System.Security.Claims;
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

namespace GTIWebAPI.Models.Account
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    /// <summary>
    /// User of application
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ApplicationUser() : base()
        {
            UserRights = new List<UserRight>();
        }

        [Column("TableName")]
        /// <summary>
        /// Is it Employee or Client 
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Id in table TableName
        /// </summary>
        public int TableId { get; set; }

        /// <summary>
        /// Profile picture
        /// </summary>
        public virtual UserImage Image { get; set; }
        /// <summary>
        /// Collection of rights
        /// </summary>
        public virtual ICollection<UserRight> UserRights { get; set; }
        public List<UserRightDTO>  GetUserRightsDTO()
        {
            List<UserRightDTO> dtos = new List<UserRightDTO>();
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

                            List<ControllerDTO> controllerList = new List<ControllerDTO>();
                            var cList = UserRights.Where(r => r.OfficeId == item.Id).Select(r => r.Controller).Distinct().ToList();

                            if (cList != null)
                            {
                                foreach (var c in cList)
                                {
                                    ControllerDTO cDto = new ControllerDTO();
                                    cDto.ControllerName = c.Name;
                                    cDto.Id = c.Id;

                                    List<ActionDTO> actionList = new List<ActionDTO>();
                                    var aList = UserRights.Where(r => r.OfficeId == item.Id && r.ControllerId == c.Id).Select(r => r.Action).Distinct().ToList();
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
                            dto.Controllers = controllerList;
                            dtos.Add(dto);
                        }
                    }
                }
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
            return userIdentity;
        }
    }

    /// <summary>
    /// Context for ApplicationUser
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        public ApplicationDbContext()
            : base("Data Source=192.168.0.229;Initial Catalog=GTIWeb_DEV;User ID=sa;Password=12345", throwIfV1Schema: false)
        {
        }

        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserRight> UserRights { get; set; }

        public DbSet<UserImage> UserImage { get; set; }

        public DbSet<Controller> Controllers { get; set; }

        public DbSet<GTIWebAPI.Models.Security.Action> Actions { get; set; }

        public DbSet<OfficeSecurity> OfficeSecurity { get; set; }

        public object Address { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRight>()
                .HasRequired<Controller>(s => s.Controller)
                .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                .HasRequired<GTIWebAPI.Models.Security.Action>(s => s.Action)
                .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                   .HasRequired<Service.OfficeSecurity>(s => s.OfficeSecurity)
                   .WithMany(s => s.UserRights);

            //modelBuilder.Entity<OfficeSecurity>()
            //    .HasKey()



            modelBuilder.Entity<UserRight>()
                   .HasRequired<ApplicationUser>(s => s.ApplicationUser)
                   .WithMany(s => s.UserRights);


            modelBuilder.Entity<ApplicationUser>()
                            .HasOptional(s => s.Image)
                            .WithRequired(ad => ad.ApplicationUser);
        }

        public virtual int FileNameUnique()
        {
            string tableName = "FileNameUnique";
            SqlParameter table = new SqlParameter("@TableName", tableName);
            int result = this.Database.SqlQuery<int>("exec NewTableId @TableName", table).FirstOrDefault();
            return result;
        }

        public bool CreateOrganization(string email, string password)
        {
            SqlParameter pEmail = new SqlParameter
            {
                ParameterName = "@Email",
                IsNullable = false,
                Direction = ParameterDirection.Input,
                DbType = DbType.String,
                Value = email
            };

            //SqlParameter pPassword = new SqlParameter
            //{
            //    ParameterName = "@Password",
            //    IsNullable = false,
            //    Direction = ParameterDirection.Input,
            //    DbType = DbType.String,
            //    Value = password
            //};

            bool methodResult = false;

            try
            {
                var result = Database.SqlQuery<bool>("exec CreateDatabaseExternalUser @Email ",
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
    }


    /// <summary>
    /// Class for user profile picture
    /// </summary>
    [Table("AspNetUserImages")]
    public class UserImage
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [ForeignKey("ApplicationUser")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        /// <summary>
        /// image name
        /// </summary>
        public string ImageName { get; set; }

        /// <summary>
        /// image byte array 
        /// </summary>
        public byte[] ImageData { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }



    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var email =
               new MailMessage(new MailAddress("gtidonotreply@gtinvest.com", "(do not reply)"),
               new MailAddress(message.Destination))
               {
                   Subject = message.Subject,
                   Body = message.Body,
                   IsBodyHtml = true
               };
            try
            {
                using (var client = new SmtpClient("192.168.0.9", 25)) // SmtpClient configuration comes from config file
                {
                    client.Credentials = new System.Net.NetworkCredential("gtidonotreply@gtinvest.com", "aluqdong");
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