using FluentNHibernate.Mapping;

namespace Model.Mapping
{
    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Type);
            Map(x => x.Data);
        }
    }
}
