using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using System.IO;
using System.Reflection;
using Beste.Databases;
using Beste.Databases.Connector;

namespace Beste.Databases.Tests
{
    [TestClass]
    public class IntegrationTest
    {
        readonly Assembly[] Assemblies =
            {
                Assembly.GetAssembly(typeof(User.UserMap))
            };

        [TestMethod]
        public void TestDatabaseConnection()
        {
            ActivateTestSchema();
            SessionFactory.Assemblies = Assemblies;
            using (NHibernate.ISession session = SessionFactory.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                
            }
        }

        [TestMethod]
        public void ReGenerateTestTables()
        {
            ActivateTestSchema();
            SessionFactory.GenerateTables();
        }

        [TestMethod]
        public void WriteInTestTable_User()
        {
            ActivateTestSchema();
            User.User user = null;
            using (NHibernate.ISession session = SessionFactory.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                user = new User.User
                {
                    Firstname = "Firstname",
                    Lastname = "Lastname",
                    Username = "Username",
                    Email = "Email",
                    MustChangePassword = true,
                    Password = "Password",
                    SaltValue = 1,
                    WrongPasswordCounter = 1
                };
                session.Save(user);
                transaction.Commit();
            }
            using (NHibernate.ISession session = SessionFactory.GetSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                User.User dbUser = session.Get<User.User>(user.UserId);
                if (!dbUser.Equals(user))
                    Assert.Fail();
            }
        }

        public void ActivateTestSchema()
        {
            SessionFactory.Assemblies = Assemblies;
            SessionFactory.ResetFactory();
            SessionFactory.Assemblies = Assemblies;
            string pathToConfig = "TestData" + Path.DirectorySeparatorChar;
            DbSettings dbSettings = DbSettings.LoadFromFile<DbSettings>(pathToConfig + "DBConnectionSettings.xml");
            dbSettings.DbSchema = "besttaf_test";
            dbSettings.SaveToFile(pathToConfig + "DBConnectionSettings_test.xml");
            SessionFactory.SettingsPath = pathToConfig + "DBConnectionSettings_test.xml";
        }
    }
}
