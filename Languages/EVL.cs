using FastSLQL.Support;
using System.Collections.Generic;
using System.IO;

namespace FastSLQL
{
    internal static class EVL
    {
        #region Get
        public static string[] Get(string[] arguments)
        {
            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            List<string> result = new List<string>();

            if (arguments.Length < FSLQLSettings.ShortSetupLenght)
                return result.ToArray();

            SLDB database = FSLQL.GetDB(dbName);
            string type = arguments[1];

            if (int.TryParse(type, out int _index))
            {
                string indexedResult = database.GetElement(_index);
                if(indexedResult != null)
                    result.Add(indexedResult);
                return result.ToArray();
            }

            switch (type)
            {
                case "ALL":
                    EVLSupport.AddAllElementsList(result, arguments, database);
                    break;
                case "First":
                    EVLSupport.AddFirstElements(result, arguments, database);
                    break;
                case "Last":
                    EVLSupport.AddLastElements(result, arguments, database);
                    break;
                case "LENGHT":
                    EVLSupport.AddElementsLenght(result, arguments, database);
                    break;
            }

            return result.ToArray();
        }
        #endregion

        #region Insert
        public static string[] Insert(string[] arguments)
        {
            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            if (arguments.Length > 1 && arguments[1][0] == '(')
            {
                string data = arguments[1];

                data = SLDBSupport.RemoveFirstLastSymbol(data);

                SLDB _database = FSLQL.GetDB(dbName);

                if (_database.VerifyParameters(data))
                {
                    _database.InsertElement(data);
                    return CommandStatus.InsertSuccess;
                }
            }

            return CommandStatus.InsertUnsuccess;
        }
        #endregion

        #region Change
        public static string[] Change(string[] arguments)
        {
            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            SLDB database = FSLQL.GetDB(dbName);

            if (arguments.Length > 3)
                EVLSupport.ChangeByFormat(database, arguments);

            return CommandStatus.InvalidArgumentFormatOrUnexpectedException;
        }
        #endregion

        #region Remove
        public static string[] Remove(string[] arguments)
        {
            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            SLDB database = FSLQL.GetDB(dbName);

            if (int.TryParse(arguments[1], out int index))
            {
                database.RemoveElement(index);
                return CommandStatus.RemoveSuccess;
            }
            else
            {
                bool removed = EVLSupport.RemoveFilteredElements(arguments, database);
                return removed ? CommandStatus.RemovesSuccess : CommandStatus.RemoveUnsuccess;
            }
        }
        #endregion
    }
}
