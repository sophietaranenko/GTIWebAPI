using GTIWebAPI.Models.Account;
using GTIWebAPI.Models.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GTIWebAPI.Models.Repository.Identity
{
    public interface IAccountRepository
    {
        bool IsEmployeeInformationFilled(int tableId);

        int GetOrganizationIdOfPerson(int organizationContactPersonId);

        string SaveFile(HttpPostedFile postedFile);

        string GetProfilePicturePathByUserId(string userId);

        UserImage AddNewProfilePicture(UserImage image);

        UserImage SetAsProfilePicture(int pictureId);

        bool CreateOrganization(string email, string password);

        bool CreateHoldingUser(string email, string pasword);

        OrganizationContactPersonView FindPerson(int id);


    }
}
