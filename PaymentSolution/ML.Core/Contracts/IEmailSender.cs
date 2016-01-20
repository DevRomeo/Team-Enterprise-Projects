namespace ML.Core
{
    public interface IEmailSender
    {
        void SendMail(string from, string to, string subject, string message);
    }
}
