﻿using Beste.Core.Models;
using Beste.Databases.User;
using Beste.Module.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

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

    public class BesteUserAuthentificationResponse : IResponse<BesteUserAuthentificationResult>
    {
        public BesteUserAuthentificationResult Result { get; private set; }
        public User UserData { get; private set; }

        public BesteUserAuthentificationResponse(BesteUserAuthentificationResult result, User userData)
        {
            Result = result;
            UserData = userData;
        }
    }

    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
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

    public class ModifyUserResponse : IResponse<ModifyUserResult>
    {
        public ModifyUserResult Result { get; private set; }
        public MandatoryUserParams MandatoryUserParams { get; private set; }
        public PasswordRules PasswordRules { get; private set; }
        public User UserData { get; private set; }

        public ModifyUserResponse(ModifyUserResult result, MandatoryUserParams mandatoryUserParams, PasswordRules passwordRules, User userData)
        {
            Result = result;
            MandatoryUserParams = mandatoryUserParams;
            PasswordRules = passwordRules;
            UserData = userData;
        }
    }
    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
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

    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortUsersBy
    {
        USERNAME,
        EMAIL,
        LASTNAME,
        ID
    }
    public class GetUsersParams
    {
        public int Limit { get; set; } = 10;
        public int Offset { get; set; } = 0;
        public SortUsersBy SortUsersBy { get; set; } = SortUsersBy.USERNAME;

        public GetUsersParams(int limit, int offset, SortUsersBy sortUsersBy)
        {
            Limit = limit;
            Offset = offset;
            SortUsersBy = sortUsersBy;
        }
    }
    public class GetUsersResponse : IResponse<GetUserResult>
    {
        public GetUserResult Result { get; private set; }
        public List<User> Users { get; private set; }

        public GetUsersResponse(GetUserResult result, List<User> users)
        {
            Result = result;
            Users = users;
        }
    }

    [Flags]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum GetUserResult
    {
        SUCCESS,
        EXCEPTION,
        RIGHT_VIOLATION,
        JSON_ERROR
    }
}
