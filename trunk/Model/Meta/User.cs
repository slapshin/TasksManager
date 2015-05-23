using Model.Common;
using System.Collections.Generic;

namespace Model
{
    public partial class User : IdEntity<User>
    {
        #region fields

        private ISet<IdentityProvider> identityProviders = new HashSet<IdentityProvider>();

        #endregion fields

        #region properties

        public virtual string Login { get; set; }

        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Patronymic { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual UserRole Roles { get; set; }

        #endregion properties

        #region collections

        public virtual ISet<IdentityProvider> IdentityProviders
        {
            get { return identityProviders; }
            set { identityProviders = value; }
        }

        #endregion collections
    }
}