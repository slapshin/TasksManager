using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace Model.Mapping
{
    class IdConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.GeneratedBy.GuidComb();
        }
    }
}
