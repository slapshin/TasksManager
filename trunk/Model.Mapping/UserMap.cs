using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(x => x.Id);
            Map(x => x.Login);
            Map(x => x.Name);
            Map(x => x.Surname);
            Map(x => x.Patronymic);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Roles);
        }
    }
}