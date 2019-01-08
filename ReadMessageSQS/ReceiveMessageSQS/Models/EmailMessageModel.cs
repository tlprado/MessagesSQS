namespace ReceiveMessageSQS.Models
{
    public class EmailMessageModel
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
