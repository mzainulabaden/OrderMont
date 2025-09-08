using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Modules.SalesManagement.SalesInvoice.Dtos
{
    public class DocumentUploadResultDto
    {
        public List<string> ImagePaths { get; set; }
        public string Message { get; set; }
    }
}
