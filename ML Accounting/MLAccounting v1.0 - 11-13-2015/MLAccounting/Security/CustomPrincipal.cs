using System.Linq;
using System.Security.Principal;

namespace MLAccountingWeb.Security
{
    public class CustomPrincipal : IPrincipal
    {
        #region Properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
        public string task { get; set; }
        #endregion

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (roles.Any(m => role.Contains(m)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}