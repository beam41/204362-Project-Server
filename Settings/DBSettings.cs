namespace MheanMaa.Settings
{
    public class DBSettings : IDBSettings
    {
        public string ConnectionString { get; set; }
        public string DBName { get; set; }
        public string DogsColName { get; set; }
        public string DonatesColName { get; set; }
        public string UsersColName { get; set; }
        public string NewsColName { get; set; }
        public string ReportsColName { get; set; }


    }

    public interface IDBSettings
    {
        public string ConnectionString { get; set; }
        public string DBName { get; set; }
        public string DogsColName { get; set; }
        public string DonatesColName { get; set; }
        public string UsersColName { get; set; }
        public string NewsColName { get; set; }
        public string ReportsColName { get; set; }
    }
}
