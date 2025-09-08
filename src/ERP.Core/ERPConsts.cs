using ERP.Configuration;
using Newtonsoft.Json;
using System.IO;

namespace ERP;

public class ERPConsts
{
    public const string LocalizationSourceName = "ERP";

    public const string ConnectionStringName = "Default";

    public const bool MultiTenancyEnabled = true;

    public static readonly string UNIVERSAL_PERMISSION = "LookUps.{0}.{1}";

    public static readonly string CREATE_PERMISSION = "LookUps.{0}.Create";

    public static readonly string UPDATE_PERMISSION = "LookUps.{0}.Update";

    public static readonly string DELETE_PERMISSION = "LookUps.{0}.Delete";

    public static readonly string APPROVE_DOCUMENT_PERMISSION = "LookUps.{0}.ApproveDocument";

    private static string connection_string;

    public static string GetConnectionString()
    {
        if (connection_string != null)
            return connection_string;

        var app_settings = LoadAppSettings();
        connection_string = app_settings.ConnectionStrings.Default;
        return connection_string;
    }

    private static AppSettings LoadAppSettings()
    {
        var json_content = File.ReadAllText("appsettings.json");
        return JsonConvert.DeserializeObject<AppSettings>(json_content);
    }

    public static readonly string DefaultPassPhrase =
        ConfigurationHelper.IsRelease ? "be6c8e4e866443ae8f938a62b21fb04e" : "gsKxGZ012HLL3MI5";
}
