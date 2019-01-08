using System.Threading.Tasks;

namespace ReceiveMessageSQS.Interfaces
{
    public interface ISendEmailService
    {
        Task SendEmail(string body);
    }
}
