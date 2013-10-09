using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class ClaimMap : ClassMap<Claim>
    {
        public ClaimMap()
        {
            Id(x => x.Id);
            References(x => x.Customer).Not.Nullable();
            Map(x => x.Created);
            Map(x => x.Title);
            Map(x => x.Comment).Length(10000);
            Map(x => x.InArchive);
            Map(x => x.Priority).CustomType<TaskPriority>();
            References(x => x.Project);
            Map(x => x.Type).CustomType<TaskType>();
            References(x => x.Call);
        }
    }
}