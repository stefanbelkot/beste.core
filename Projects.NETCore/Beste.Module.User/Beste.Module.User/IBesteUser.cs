using Beste.Databases.User;
using Beste.Module.Settings;
using Newtonsoft.Json;
using System;

namespace Beste.Module
{
    interface IBesteUser
    {

        /// <summary>
        /// Authenticate a user
        /// </summary>
        /// <param name="param">provided param to authenticate a user, e.g. json</param>
        /// <returns>response</returns>
        BesteUserAuthentificationResponse Authenticate(string param);

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="param">provided param to create a user, e.g. json</param>
        /// <returns>response</returns>
        ModifyUserResponse CreateUser(string param);

        /// <summary>
        /// Edits a user
        /// </summary>
        /// <param name="param">provided param to edit a user, e.g. json</param>
        /// <returns>response</returns>
        ModifyUserResponse EditUser(string param);

        /// <summary>
        /// Change of password by the user
        /// </summary>
        /// <param name="param">provided param to change the user password, e.g. json</param>
        /// <returns>response</returns>
        ModifyUserResponse ChangePasswordByUser(string param);
        
        /// <summary>
        /// Delete a specified user
        /// </summary>
        /// <param name="param">provided param to delete a user, e.g. json</param>
        /// <returns>response</returns>
        ModifyUserResponse DeleteUser(string param);
        
    }

    public class BesteUserAuthentificationResponse
    {
        public BesteUserAuthentificationResult Result;
        public User UserData;

        public BesteUserAuthentificationResponse(BesteUserAuthentificationResult result, User userData)
        {
            Result = result;
            UserData = userData;
        }
    }

    [Flags]
    public enum BesteUserAuthentificationResult
    {
        USER_UNKNOWN,
        SUCCESS,
        WRONG_PASSWORD,
        WRONG_PASSWORD_COUNTER_TOO_HIGH,
        WRONG_PARAMETER,
        MUST_CHANGE_PASSWORT,
        JSON_ERROR,
        RIGHT_VIOLATION,
        UNKNOWN_EXCEPTION
    }

    public class ModifyUserResponse
    {
        public ModifyUserResult Result;
        public MandatoryUserParams MandatoryUserParams;
        public PasswordRules PasswordRules;
        public User UserData;

        public ModifyUserResponse(ModifyUserResult result, MandatoryUserParams mandatoryUserParams, PasswordRules passwordRules, User userData)
        {
            Result = result;
            MandatoryUserParams = mandatoryUserParams;
            PasswordRules = passwordRules;
            UserData = userData;
        }
    }
    [Flags]
    public enum ModifyUserResult
    {
        MISSING_USER_PARAMS,
        PASSWORD_GUIDELINES_ERROR,
        SUCCESS,
        UNKNOWN_EXCEPTION,
        USER_UNKNOWN,
        WRONG_PARAMETER,
        USER_ALREADY_EXISTS,
        RIGHT_VIOLATION,
        JSON_ERROR
    }

}
