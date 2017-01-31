using GTIWebAPI.Models.Context;
using GTIWebAPI.Models.Dictionary;
using GTIWebAPI.Models.Personnel;
using System.Collections.Generic;
using System.Linq;

namespace GTIWebAPI.Models.Organizations
{
    public class OrganizationList
    {
        public IEnumerable<ContactTypeDTO> ContactTypes { get; set; }

        public IEnumerable<OrganizationAddressTypeDTO> OrganizationAddressTypes { get; set; }

        public IEnumerable<OrganizationTaxAddressTypeDTO> OrganizationTaxAddressTypes { get; set; }

        public IEnumerable<OrganizationPropertyTypeDTO> OrganizationPropertyTypes { get; set; }

        public IEnumerable<OrganizationLegalFormDTO> OrganizationLegalForms { get; set; }

        public AddressList AddressList { get; set; }

        public OrganizationList()
        { }

        public static OrganizationList CreateOrganizationList(IDbContextOrganization db)
        {
            OrganizationList list = new OrganizationList();

            list.AddressList = AddressList.CreateAddressList(db);

            List<ContactType> types = db.ContactTypes.ToList();
            list.ContactTypes = types.Select(c => c.ToDTO()).ToList();

            List<OrganizationAddressType> aTypes = db.OrganizationAddressTypes.ToList();
            list.OrganizationAddressTypes = aTypes.Select(a => a.ToDTO()).ToList();

            List<OrganizationTaxAddressType> taTypes = db.OrganizationTaxAddressTypes.ToList();
            list.OrganizationTaxAddressTypes = taTypes.Select(a => a.ToDTO()).ToList();

            List<OrganizationLegalForm> forms = db.OrganizationLegalForms.ToList();
            list.OrganizationLegalForms = forms.Select(a => a.ToDTO()).ToList();

            List<OrganizationPropertyType> pTypes = db.OrganizationPropertyTypes.ToList();
            list.OrganizationPropertyTypes = pTypes.Select(a => a.ToDTO()).ToList();

            return list;
        }


    }
}
