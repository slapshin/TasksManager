using Model;

namespace Web.Common.Auth
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}