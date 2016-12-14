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

        /// <summary>
        /// Profile picture
        /// </summary>
        public virtual UserImage Image { get; set; }
        /// <summary>
        /// Collection of rights
        /// </summary>
        public virtual ICollection<UserRight> UserRights { get; set; }

        /// <summary>
        /// Collection of rights in another form of representation
        /// </summary>
        public List<UserRightDTO> UserRightsDto
        {
            get
            {
                List<UserRightDTO> dtos = new List<UserRightDTO>();
                if (UserRights != null)
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
                                            aDto.ActionName = a.Name == null? "" : a.Name;
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
                return dtos;
            }
            

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
            : base("Data Source=192.168.0.229;Initial Catalog=Crew;User ID=sa;Password=12345", throwIfV1Schema: false)
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

        public DbSet<Action> Actions { get; set; }

        public DbSet<OfficeSecurity> OfficeSecurity { get; set; }

        public object Address { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserRight>()
                .HasRequired<Controller>(s => s.Controller)
                .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                .HasRequired<Action>(s => s.Action)
                .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                   .HasRequired<Service.OfficeSecurity>(s => s.OfficeSecurity)
                   .WithMany(s => s.UserRights);


            modelBuilder.Entity<UserRight>()
                   .HasRequired<ApplicationUser>(s => s.ApplicationUser)
                   .WithMany(s => s.UserRights);


            modelBuilder.Entity<ApplicationUser>()
                            .HasOptional(s => s.Image) 
                            .WithRequired(ad => ad.ApplicationUser);
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
}