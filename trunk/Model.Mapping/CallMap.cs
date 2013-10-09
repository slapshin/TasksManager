using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class CallMap : ClassMap<Call>
    {
        public CallMap()
        {
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Title);
            Map(x => x.Comment).Length(10000);
            References(x => x.Project).Not.Nullable();
            Map(x => x.Created);
            Map(x => x.Completed);
            Map(x => x.Returned);
            Map(x => x.Checked);
            Map(x => x.Priority).CustomType<TaskPriority>(); ;
            Map(x => x.Status).CustomType<CallStatus>();
            Map(x => x.InArchive);
            References(x => x.Claim);
            HasMany(x => x.Tasks).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}