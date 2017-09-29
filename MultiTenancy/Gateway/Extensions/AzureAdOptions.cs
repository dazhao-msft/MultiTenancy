namespace Microsoft.AspNetCore.Authentication
{
    public class AzureAdOptions
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string Instance { get; set; }
        public string Domain { get; set; }
        public string TenantId { get; set; }
    }
}
