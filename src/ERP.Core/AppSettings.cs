namespace ERP
{
    public class ConnectionStrings
    {
        public string Default { get; set; }
    }

    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
    }
}
