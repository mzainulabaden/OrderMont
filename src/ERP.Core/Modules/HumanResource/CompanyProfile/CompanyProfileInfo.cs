using ERP.Generics;

namespace ERP.Modules.HumanResource.CompanyProfile
{
    public class CompanyProfileInfo: SimpleEntityBase
    {
        public string Logo { get; set; }
        public string NTN { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
}
