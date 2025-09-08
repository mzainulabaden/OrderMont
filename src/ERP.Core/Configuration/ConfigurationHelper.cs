namespace ERP.Configuration;

public static class ConfigurationHelper
{
    public static bool IsRelease
    {
        get
        {
#pragma warning disable
#if RELEASE
            return true;
#endif
            return false;
#pragma warning restore
        }
    }
}