using SQLite;

namespace DocSearcher.Model
{
    public class Extension
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public bool Checked { get; set; }
    }
}