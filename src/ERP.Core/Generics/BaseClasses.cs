using Abp.Application.Services.Dto;
using System;

namespace ERP.Generics
{
    public class SimpleEntityBase : ERPProjBaseEntity
    {
        public string Name { get; set; }
    }

    public class SimpleDtoBase : EntityDto<long>
    {
        public string Name { get; set; }
    }

    public class SimpleSearchDtoBase : BaseFiltersDto
    {
        public string Name { get; set; }
    }

    public class BaseFiltersDto : PagedResultRequestDto
    {
        public string Id { get; set; }
    }

    public class BaseDocumentDto : EntityDto<long>
    {
        public DateTime IssueDate { get; set; }
        public string Remarks { get; set; }
    }

    public class BaseDocumentGetAllDto : BaseDocumentDto
    {
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
    }

    public class BaseDocumentFiltersDto : BaseFiltersDto
    {
        public DateTime? IssueDate { get; set; }
        public string VoucherNumber { get; set; }
        public string Status { get; set; }
    }
}
