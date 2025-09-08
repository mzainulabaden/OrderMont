using Abp.Domain.Entities;
using ERP.Enums;
using ERP.Generics;
using ERP.Modules.Finance.GeneralPayments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.Finance.ProfitLoseNote
{
    public class ProfitLoseNoteInfo : SimpleEntityBase
    {
        public string NoteNumber { get; set; }
        public AccountType AccountType { get; set; }
        public List<ProfitLoseNoteDetailsInfo> ProfitLoseNoteDetails { get; set; }
    }
    public class ProfitLoseNoteDetailsInfo : Entity<long>
    {
        public long COALevel03Id { get; set; }
        public string COAlevel03Name { get; set; }  

    }
}
