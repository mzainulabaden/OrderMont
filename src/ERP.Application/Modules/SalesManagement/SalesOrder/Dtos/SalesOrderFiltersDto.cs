using ERP.Generics;

namespace ERP.Modules.SalesManagement.SalesOrder
{
    public class SalesOrderFiltersDto : BaseDocumentFiltersDto
    {
        public string PaymentModeId { get; set; }
        public string CustomerCOALevel04Id { get; set; }
        public string WarehouseId { get; set; }
        public string EmployeeId { get; set; }
    }
}
