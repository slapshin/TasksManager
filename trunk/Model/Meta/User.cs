using Iesi.Collections.Generic;
using Model.Common;

namespace Model
{
    public partial class User : IdEntity<User>
    {
        #region fields
        private ISet<IdentityProvider> identityProviders = new HashedSet<IdentityProvider>();
        #endregion

        #region properties
        public virtual string Login { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Patronymic { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual UserRole Roles { get; set; }
        #endregion

        #region collections
        public virtual ISet<IdentityProvider> IdentityProviders
        {
            get { return identityProviders; }
            set { identityProviders = value; }
        }
        #endregion
    }
}
