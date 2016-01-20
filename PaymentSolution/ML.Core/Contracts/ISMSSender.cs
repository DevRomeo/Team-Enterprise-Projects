namespace ML.Core
{
    public interface ISMSSender
    {
        void SendSMS(string cellNumber, string message);
    }
}
