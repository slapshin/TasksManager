using System.Configuration;

namespace Web.Common.Repository
{
    public class DatabaseConfiguration : ConfigurationSection
    {
        private const string SECTION_NAME = "databaseConfig";
        private const string SERVER = "server";
        private const string DATABASE = "database";
        private const string USER = "user";
        private const string PASSWORD = "password";
        private const string INTEGRATED = "integrated";
        private const string TYPE = "type";

        [ConfigurationProperty(SERVER, IsRequired = true)]
        public string Server
        {
            get { return this[SERVER] as string; }
            set { this[SERVER] = value; }
        }

        [ConfigurationProperty(DATABASE, IsRequired = false)]
        public string Database
        {
            get { return this[DATABASE] as string; }
            set { this[DATABASE] = value; }
        }

        [ConfigurationProperty(USER, IsRequired = false)]
        public string User
        {
            get { return this[USER] as string; }
            set { this[USER] = value; }
        }

        [ConfigurationProperty(PASSWORD, IsRequired = false)]
        public string Password
        {
            get { return this[PASSWORD] as string; }
            set { this[PASSWORD] = value; }
        }

        public static DatabaseConfiguration GetConfiguration()
        {
            return (DatabaseConfiguration)ConfigurationManager.GetSection(SECTION_NAME);
        }
    }
}