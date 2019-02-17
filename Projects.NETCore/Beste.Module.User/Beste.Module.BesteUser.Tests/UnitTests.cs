using Microsoft.VisualStudio.TestTools.UnitTesting;
using Beste.Module;
using Beste.Databases.User;
using Newtonsoft.Json;
using System.Reflection;
using Beste.Databases.Connector;
using System.IO;
using System;

namespace Beste.Module.Tests
{
    [TestClass]
    public class UnitTests
    {
        readonly Assembly[] Assemblies =
        {
            Assembly.GetAssembly(typeof(UserMap))
        };
        

        [TestMethod]
        public void CreateUserWrongPasswordGuidelines()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User();
            user.Username = "UsernamePasswordGuidelines";
            user.Lastname = "Lastname";
            user.Firstname = "Firstname";
            user.Email = "Email";
            user.Password = "passwort";
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.PASSWORD_GUIDELINES_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.PASSWORD_GUIDELINES_ERROR.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CreateUserMissingParams()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User();
            user.Username = "Username";
            user.Password = "Passwort1$";
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.MISSING_USER_PARAMS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.MISSING_USER_PARAMS.ToString());
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public void CreateUserAndLogin()
        {
            ActivateTestSchema();

            BesteUser besteUser = new BesteUser();

            User user = new User
            {
                Username = "UsernameLogin",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };

            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            User loginUser = new User
            {
                Username = user.Username,
                Password = user.Password
            };

            BesteUserAuthentificationResponse authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.MUST_CHANGE_PASSWORT)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.MUST_CHANGE_PASSWORT.ToString());
                Assert.Fail();
            }

            response = besteUser.ChangePasswordByUser(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.SUCCESS)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.SUCCESS.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CreateUserAndEdit()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UsernameToEdit",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            User loginUser = new User
            {
                Username = "UsernameToEdit",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$",
                MustChangePassword = false
            };
            response = besteUser.EditUser(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }

            BesteUserAuthentificationResponse authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.SUCCESS)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.SUCCESS.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CreateUserAndChangePasswortBreakRules()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UsernameChangePasswortBreakRules",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            User loginUser = new User
            {
                Username = user.Username,
                Password = "passwort"
            };
            response = besteUser.ChangePasswordByUser(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.PASSWORD_GUIDELINES_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
        }
        [TestMethod]
        public void CreateUserAndDelete()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UsernameLogin",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            User loginUser = new User
            {
                Username = user.Username,
                Password = user.Password
            };
            response = besteUser.DeleteUser(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void UnknownUser()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UnknnownUsernameLogin",
                Password = "Passwort1$"
            };
            BesteUserAuthentificationResponse authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.USER_UNKNOWN)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.SUCCESS.ToString());
                Assert.Fail();
            }
            ModifyUserResponse response = besteUser.EditUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.USER_UNKNOWN)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString());
                Assert.Fail();
            }
            response = besteUser.ChangePasswordByUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.USER_UNKNOWN)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString());
                Assert.Fail();
            }
            response = besteUser.DeleteUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.USER_UNKNOWN)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void CreateUserAndWrongPasswortCounter()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UsernameWrongPasswortCounter",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString());
                Assert.Fail();
            }
            User loginUser = new User();
            loginUser.Username = user.Username;
            loginUser.Password = user.Password + "1";

            BesteUserAuthentificationResponse authResponse;
            for (int i = 0; i < 13; i++)
            {
                authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
                if (authResponse.Result != BesteUserAuthentificationResult.WRONG_PASSWORD)
                {
                    Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.WRONG_PASSWORD.ToString());
                    Assert.Fail();
                }
            }

            loginUser.Password = user.Password;
            authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.WRONG_PASSWORD_COUNTER_TOO_HIGH)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.WRONG_PASSWORD_COUNTER_TOO_HIGH.ToString());
                Assert.Fail();
            }
        }

        [TestMethod]
        public void ForcedJsonSerializationErrors()
        {
            BesteUser besteUser = new BesteUser();
            ModifyUserResponse response = besteUser.CreateUser("no json]");
            if (response.Result != ModifyUserResult.JSON_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.JSON_ERROR.ToString());
                Assert.Fail();
            }
            response = besteUser.ChangePasswordByUser("no json]");
            if (response.Result != ModifyUserResult.JSON_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.JSON_ERROR.ToString());
                Assert.Fail();
            }
            response = besteUser.DeleteUser("no json]");
            if (response.Result != ModifyUserResult.JSON_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.JSON_ERROR.ToString());
                Assert.Fail();
            }
            response = besteUser.EditUser("no json]");
            if (response.Result != ModifyUserResult.JSON_ERROR)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.JSON_ERROR.ToString());
                Assert.Fail();
            }
            BesteUserAuthentificationResponse authResponse = besteUser.Authenticate("no json]");
            if (authResponse.Result != BesteUserAuthentificationResult.JSON_ERROR)
            {
                Console.WriteLine("response.Result = " + authResponse.Result.ToString() + " Expected = " + ModifyUserResult.JSON_ERROR.ToString());
                Assert.Fail();
            }

        }

        [TestMethod]
        public void WrongParameters()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "",
                Password = ""
            };
            BesteUserAuthentificationResponse authResponse = besteUser.Authenticate(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.WRONG_PARAMETER)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.WRONG_PARAMETER.ToString());
                Assert.Fail();
            }
        }
        [TestMethod]
        public void CreateUserAndTryLoginWithWrongPepper()
        {
            ActivateTestSchema();
            BesteUser besteUser = new BesteUser();
            User user = new User
            {
                Username = "UsernameLoginWrongPepper",
                Lastname = "Lastname",
                Firstname = "Firstname",
                Email = "Email",
                Password = "Passwort1$"
            };
            ModifyUserResponse response = besteUser.CreateUser(JsonConvert.SerializeObject(user, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            User loginUser = new User
            {
                Username = user.Username,
                Password = user.Password
            };
            response = besteUser.ChangePasswordByUser(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (response.Result != ModifyUserResult.SUCCESS)
            {
                Console.WriteLine("response.Result = " + response.Result.ToString() + " Expected = " + ModifyUserResult.SUCCESS.ToString());
                Assert.Fail();
            }
            BesteUser besteUserOtherPepper = new BesteUser("otherPepper");
            BesteUserAuthentificationResponse authResponse = besteUserOtherPepper.Authenticate(JsonConvert.SerializeObject(loginUser, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            if (authResponse.Result != BesteUserAuthentificationResult.WRONG_PASSWORD)
            {
                Console.WriteLine("authResponse.Result = " + authResponse.Result.ToString() + " Expected = " + BesteUserAuthentificationResult.SUCCESS.ToString());
                Assert.Fail();
            }
        }

        public void ActivateTestSchema(bool regenerateSchema = true)
        {
            SessionFactory.Assemblies = Assemblies;
            SessionFactory.ResetFactory();
            string pathToConfig = "TestData" + Path.DirectorySeparatorChar;
            //DbSettings dbSettings = Xml.Xml.LoadFromFile<DbSettings>(pathToConfig + "DBConnectionSettingsTest.xml");
            //dbSettings.DbSchema = "besttaf_test";
            //dbSettings.DbPassword = "";
            //dbSettings.SaveToFile(pathToConfig + "DBConnectionSettings_test.xml");
            SessionFactory.SettingsFileName = pathToConfig + "DBConnectionSettings_test.xml";
            if (regenerateSchema)
                SessionFactory.GenerateTables();
        }
    }
}
