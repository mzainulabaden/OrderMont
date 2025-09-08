using Abp.Domain.Entities;
using ERP.Generics;
using System;
using System.Collections.Generic;

namespace ERP.Modules.InventoryManagement.DayBook
{
    public class DayBookInfo : SimpleEntityBase
    {
        public DateTime IssueDate { get; set; }
        public decimal Total { get; set; }
        public List<DayBookDetailsInfo> DayBookDetails {  get; set; }
    }
    public class DayBookDetailsInfo : Entity<long>
    {
        public string COAName { get; set; }
        public long COAlevel04Id { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        
    }
}
