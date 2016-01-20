using System.Web.Security;

namespace ML.Web
{
    public class AuthorizationProvider : IAuthorizationProvider
    {         
        public void SetAuthCookie(string userName, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SetAuthCookie(string userName, bool createPersistentCookie, string cookiePath)
        {
            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie, cookiePath);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}