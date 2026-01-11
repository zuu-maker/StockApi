namespace StockApi.Modals
{
    public class Session
    {
        public int Id { get; set; }
        public string SessionDate { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public string ExportedAt { get; set; } = string.Empty;
        public bool Locked { get; set; } = false;
    }
};
