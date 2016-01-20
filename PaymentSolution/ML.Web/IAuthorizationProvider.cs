
namespace ML.Web
{
    public interface IAuthorizationProvider
    {
        void SetAuthCookie(string userName, bool createPersistentCookie);

        void SetAuthCookie(string userName, bool createPersistentCookie, string cookiePath);

        void SignOut();
    }
}
