using Model.Common;

namespace Model
{
    public abstract class IdentityProvider : IdEntity<File>
    {
        #region properties
        public User User { get; set; }
        #endregion
    }
}
