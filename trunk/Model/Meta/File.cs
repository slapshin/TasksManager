using Model.Common;

namespace Model
{
    public class File : IdEntity<File>
    {
        #region properties
        public virtual string Title { get; set; }
        public virtual FileType Type { get; set; }
        public virtual byte[] Data { get; set; }
        #endregion
    }
}
