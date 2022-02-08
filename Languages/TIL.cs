using FastSLQL.Support;
using System.IO;

namespace FastSLQL
{
    internal static class TIL
    {
        #region Create
        public static string[] Create(string[] arguments)
        {
            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                File.Create(directory).Close();
            else
                return CommandStatus.DBAlreadyExists(dbName);

            SLDB _db = FSLQL.AddDBase(new FileInfo(directory));
            _db.WriteDBName(dbName);

            string[] _editArguments = new string[3] { "STRUCTURE", arguments[0], arguments[1] };

            if(arguments.Length > 1)
                Edit(_editArguments);

            return CommandStatus.CreateSuccess(dbName);
        }
        #endregion

        #region Edit
        public static string[] Edit(string[] arguments)
        {
            string dbName = arguments[0];
            string instruction = arguments[1];
            string data = arguments[2];

            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            if(instruction == "NAME")
            {
                FSLQL.GetDB(dbName).WriteDBName(data);
                return CommandStatus.EditNameSuccess;
            }
            else if(instruction == "STRUCTURE" && arguments.Length > 1 && data[0] == '(')
            {
                data = SLDBSupport.RemoveFirstLastSymbol(data);

                FSLQL.GetDB(dbName).WriteDBStructure(data);
                return CommandStatus.EditStructureSuccess;
            }

            return CommandStatus.InvalidArgumentFormatOrUnexpectedException;
        }
        #endregion

        #region Delete
        public static string[] Delete(string[] arguments)
        {
            if (arguments.Length == 0)
                return CommandStatus.InvalidArgumentFormatOrUnexpectedException;

            string dbName = arguments[0];
            string directory = FSLQL.FormatDirectory(dbName);

            if (!File.Exists(directory))
                return CommandStatus.DBDoesntExists(dbName);

            SLDB database = FSLQL.GetDB(dbName);
            FSLQL.RemoveDBase(database);

            string dbStructureFirst = database.GetDBStructure().Split(" | ")[0];

            foreach (SLDB sldb in FSLQL.SLDBases)
                if (sldb.GetDBStructure().Split(" | ")[0] == dbStructureFirst && dbName.ToLower() != sldb.AssemblyName)
                    File.Delete(sldb.Directory);

            File.Delete(directory);

            return CommandStatus.DeleteSuccess(dbName);
        }
        #endregion
    }
}
