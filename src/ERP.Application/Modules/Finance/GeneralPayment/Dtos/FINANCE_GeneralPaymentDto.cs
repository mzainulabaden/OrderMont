using Abp.AutoMapper;
using Abp.Domain.Entities;
using ERP.Enums;
using ERP.Modules.Finance.GeneralPayments;
using System;
using System.Collections.Generic;

namespace ERP.Modules.Finance.GeneralPayment
{
    [AutoMap(typeof(GeneralPaymentInfo))]
    public class FINANCE_GeneralPaymentDto : Entity<long>
    {
        public long BankCOALevel04Id { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime ReferenceDate { get; set; }
        public bool IsCheque { get; set; }
        public bool IsCrossedCheque { get; set; }
        public string ChequeTitle { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? MaturityDate { get; set; }
        public decimal TotalAmount { get; set; }
        public GeneralPaymentLinkedDocument LinkedDocument { get; set; }
        public List<GeneralPaymentDetailsDto> GeneralPaymentDetails { get; set; }
       
    }

    [AutoMap(typeof(GeneralPaymentDetailsInfo))]
    public class GeneralPaymentDetailsDto : Entity<long>
    {
        public long COALevel04Id { get; set; }
        public decimal GrossAmount { get; set; }
        public long? TaxCOALevel04Id { get; set; }
        public decimal? TaxAmount { get; set; }
        public long? OtherTaxCOALevel04Id { get; set; }
        public decimal? OtherTaxAmount { get; set; }
        public decimal? NetAmount { get; set; }
        public string Remarks { get; set; }
        public string VoucherNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public long? GeneralLedgerId { get; set; }
        public long GeneralPaymentInfoId { get; set; }
        public string COALevel04Name { get; set; }
    }
}
