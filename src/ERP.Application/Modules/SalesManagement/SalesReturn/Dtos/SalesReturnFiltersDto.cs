using ERP.Generics;

namespace ERP.Modules.SalesManagement.SalesReturn
{
    public class SalesReturnFiltersDto : BaseDocumentFiltersDto
    {
        public string CustomerCOALevel04Id { get; set; }
        public string WarehouseId { get; set; }
    }
}
