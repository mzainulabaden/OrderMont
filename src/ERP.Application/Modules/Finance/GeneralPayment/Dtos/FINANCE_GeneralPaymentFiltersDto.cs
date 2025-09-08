using ERP.Enums;
using ERP.Generics;

namespace ERP.Modules.Finance.GeneralPayment
{
    public class FINANCE_GeneralPaymentFiltersDto : BaseDocumentFiltersDto
    {
        public string BankCOALevel04Id { get; set; }
        public GeneralPaymentLinkedDocument? LinkedDocument { get; set; }
    }
}
