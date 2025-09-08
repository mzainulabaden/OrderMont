using Abp.Application.Services;
using Abp.UI;
using ERP.Enums;
using ERP.Modules.Reporting.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ERP.Modules.Reporting
{
    public class ReportingAppService : ApplicationService
    {
        private readonly IConfiguration _appConfiguration;

        public ReportingAppService(IConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        [HttpPost]
        public async Task<IActionResult> DownloadReport(string ReportName, string ReportUrl, [FromQuery] ReportFormat format, [FromBody] List<ReportParameterDto> parameters = null)
        {
            string format_string = format.ToString().ToLower();
            string base_url = _appConfiguration["ReportServer:Settings:BaseUrl"];

            var query_params = parameters != null
                ? string.Join("&", parameters
                    .Where(p => !string.IsNullOrEmpty(p.ParameterName))
                    .Select(p => $"{Uri.EscapeDataString(p.ParameterName)}={Uri.EscapeDataString(p.ParameterValue)}"))
                : "";

            string full_url = string.Empty;
            if (query_params.Equals(string.Empty))
            {
                full_url = $"{base_url}?{ReportUrl}&rs:Format={format_string}";
            }
            else
            {
                full_url = $"{base_url}?{ReportUrl}&{query_params}&rs:Format={format_string}";
            }

            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(
                    _appConfiguration["ReportServer:Credentials:Username"],
                    _appConfiguration["ReportServer:Credentials:Password"]
                ),
                UseDefaultCredentials = false,
                PreAuthenticate = true,
                ClientCertificateOptions = ClientCertificateOption.Manual
            };

            using var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

            HttpResponseMessage response = await client.GetAsync(full_url);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                throw new UserFriendlyException((int)response.StatusCode, $"Error fetching report: {errorContent}");
            }

            var report_bytes = await response.Content.ReadAsByteArrayAsync();
            var content_type = GetContentType(format);
            var file_name = $"{ReportName.Trim()}.{GetFileExtension(format)}";

            return new FileContentResult(report_bytes, content_type)
            {
                FileDownloadName = file_name
            };
        }

        private static string GetFileExtension(ReportFormat format)
        {
            return format switch
            {
                ReportFormat.PDF => "pdf",
                ReportFormat.EXCEL => "xls",
                ReportFormat.WORD => "doc",
                ReportFormat.PPTX => "pptx",
                ReportFormat.CSV => "csv",
                ReportFormat.XML => "xml",
                ReportFormat.MHTML => "mhtml",
                ReportFormat.IMAGE => "tif",
                _ => throw new ArgumentOutOfRangeException(nameof(format), $"Unsupported format: {format}")
            };
        }

        private string GetContentType(ReportFormat format)
        {
            return format switch
            {
                ReportFormat.PDF => "application/pdf",
                ReportFormat.EXCEL => "application/vnd.ms-excel",
                ReportFormat.WORD => "application/msword",
                ReportFormat.PPTX => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                ReportFormat.CSV => "text/csv",
                ReportFormat.XML => "application/xml",
                ReportFormat.MHTML => "message/rfc822",
                ReportFormat.IMAGE => "image/tiff",
                _ => "application/octet-stream"
            };
        }
    }
}
