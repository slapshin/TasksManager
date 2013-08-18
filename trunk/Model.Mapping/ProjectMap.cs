using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Comment);
            Map(x => x.Priority).CustomType<ProjectPriority>();
            References(x => x.Master).Not.Nullable();
            HasMany(x => x.Claims).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Calls).Inverse().Cascade.AllDeleteOrphan();
            HasManyToMany<User>(x => x.Executors).Table("ProjectExecutors");
            HasManyToMany<User>(x => x.Observers).Table("ProjectObservers");
        }
    }
}