using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Comment).Length(10000);
            Map(x => x.Priority).CustomType<TaskPriority>();
            Map(x => x.Status).CustomType<TaskStatus>();
            Map(x => x.Type).CustomType<TaskType>();
            Map(x => x.Created);
            Map(x => x.Executing);
            Map(x => x.Completed);
            Map(x => x.Deadline);
            Map(x => x.Returned);
            Map(x => x.Checked);
            Map(x => x.InArchive);
            References(x => x.Call);
            References(x => x.Executor);
            HasMany(x => x.Files).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}