namespace APIProject.Infraestructure.Messaging
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTime? Ocurred { get; set; }
        public DateTime? Processed { get; set; }
        public string? Error { get; set; }
    }
}
