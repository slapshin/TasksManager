using System.Configuration;

namespace Web.Common.Repository
{
    public class DatabaseConfiguration : ConfigurationSection
    {
        private const string SectionName = "databaseConfig";
        private const string ServerSettingName = "server";
        private const string DatabaseSettingName = "database";
        private const string UserSettingName = "user";
        private const string PasswordSettingName = "password";
        private const string IntegratedSettingName = "integrated";
        private const string TypeSettingName = "type";

        [ConfigurationProperty(ServerSettingName, IsRequired = true)]
        public string Server
        {
            get { return this[ServerSettingName] as string; }
            set { this[ServerSettingName] = value; }
        }

        [ConfigurationProperty(DatabaseSettingName, IsRequired = false)]
        public string Database
        {
            get { return this[DatabaseSettingName] as string; }
            set { this[DatabaseSettingName] = value; }
        }

        [ConfigurationProperty(UserSettingName, IsRequired = false)]
        public string User
        {
            get { return this[UserSettingName] as string; }
            set { this[UserSettingName] = value; }
        }

        [ConfigurationProperty(PasswordSettingName, IsRequired = false)]
        public string Password
        {
            get { return this[PasswordSettingName] as string; }
            set { this[PasswordSettingName] = value; }
        }

        public static DatabaseConfiguration GetConfiguration()
        {
            return (DatabaseConfiguration)ConfigurationManager.GetSection(SectionName);
        }
    }
}