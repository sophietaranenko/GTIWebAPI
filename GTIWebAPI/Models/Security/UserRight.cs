using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Security
{
    [Table("AspNetUserRights")]
    public class UserRight : GTITable
    {
        [Key]
        public Guid Id { get; set; }

        public string AspNetUserId { get; set; }

        public Int32 OfficeId { get; set; }

        public Int32 ActionId { get; set; }

        [ForeignKey("ActionId")]
        public virtual Models.Security.Action Action { get; set; }

        [ForeignKey("OfficeId")]
        public virtual OfficeSecurity OfficeSecurity { get; set; }

        [ForeignKey("AspNetUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        protected override string TableName
        {
            get
            {
                return "AspNetUserRights";
            }
        }
    }


    [Table("AspNetUserRights")]
    public class UserRightOff : GTITable
    {
        [Key]
        public Guid Id { get; set; }

        public string AspNetUserId { get; set; }

        public Int32 OfficeId { get; set; }

        public Int32 ActionId { get; set; }

        [ForeignKey("ActionId")]
        public virtual Models.Security.RightControllerAction Action { get; set; }

        [ForeignKey("OfficeId")]
        public virtual Office Office { get; set; }

        protected override string TableName
        {
            get
            {
                return "AspNetUserRights";
            }
        }
    }

    public class UserRightOfficeDTO
    {
        public Guid? Id { get; set; }

        public string UserId { get; set; }

        public int? OfficeId { get; set; }

        public string OfficeShortName { get; set; }

        public int? ActionId { get; set; }

        public string ActionName { get; set; }

        public string ActionLongName { get; set; }

        public int? ControllerId { get; set; }

        public string ControllerName { get; set; }

        public string ControllerLongName { get; set; }

        public int? BoxId { get; set; }

        public string BoxName { get; set; }

        public string BoxLongName { get; set; }

        public bool? Value { get; set; }
    }


    public class UserRightTreeView : IEquatable<UserRightTreeView>
    {

        public string UserId { get; set; }

        public int? OfficeId { get; set; }

        public OfficeDTO Office { get; set; }

        public IEnumerable<RightControllerBoxDTO> Boxes { get; set; }

        public bool Equals(UserRightTreeView other)
        {
            return this.UserId == other.UserId;
        }

        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }
    }


    public class UserRightDTO
    {
        [Key]
        public Int32 OfficeId { get; set; }

        public string OfficeName { get; set; }

        public List<ControllerBoxDTO> Boxes { get; set; }

        public static List<UserRightDTO> TransferToDTO(List<UserRight> UserRights)
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
    }

    public class UserRightEditDTO
    {
        public Int32 OfficeId { get; set; }

        public Int32 ControllerId { get; set; }

        public Int32 ActionId { get; set; }
    }

}