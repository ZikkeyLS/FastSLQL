using System;

namespace FastSLQL.Support
{
    internal static class CommandStatus
    {
        #region BasicExceptions
        public static string[] UnknownCommand(string cname) => new string[] { $"Unknown command with name: {cname}!" };
        public static string[] UnknownException(Exception ex) => new string[] { $"Exception: \n{ex.StackTrace} \n{ex.Message}!" };

        public readonly static string[] BadIndex = { "Bad index founded!" };
        public readonly static string[] InvalidArgumentFormatOrUnexpectedException = { $"Invalid argument format or unexpected exception found!" };
        #endregion

        #region DBExceptions
        public static string[] DBDoesntExists(string dbName) => new string[] { $"DB with name \"{dbName}\" not exists!" };
        public static string[] DBAlreadyExists(string dbName) => new string[] { $"DB with name \"{dbName}\" already exists!" };
        #endregion

        #region TILStatus
        public static string[] CreateSuccess(string dbName) => new string[] { $"Succesfully created DB \"{dbName}\"!" };

        public readonly static string[] EditNameSuccess = { "Name was succesfully set!" };
        public readonly static string[] EditStructureSuccess = { "Structure was succesfully set!" };

        public static string[] DeleteSuccess(string dbName) => new string[] { $"DB with name \"{dbName}\" was succesfully deleted!" };
        #endregion

        #region EVLStatus
        public static string[] InsertSuccess = { "Element was successfully added!" };
        public static string[] InsertUnsuccess = { "Structure of arguments is not equals to base structure!" };

        public static string[] ChangeSuccess = { "Element was successfully changed!" };
        public static string[] ChangesSuccess = { "Elements were successfully changed!" };

        public readonly static string[] RemoveSuccess = { "Element was successfully removed!" };
        public readonly static string[] RemovesSuccess =  { "Elements were successfully removed!" };
        public readonly static string[] RemoveUnsuccess = { "Elements with this filter can't be removed or not exists!" };
        #endregion
    }
}
