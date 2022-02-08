using FastSLQL.Format;
using FastSLQL.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FastSLQL
{
    public static class FSLQL
    {
        public static string DBDirectory { get; private set; } = "";
        public static readonly List<SLDB> SLDBases = new List<SLDB>();

        public static void Load(string dbDirectory = "")
        {
            DBDirectory = dbDirectory;

            if (Directory.Exists(dbDirectory) == false)
                Directory.CreateDirectory(dbDirectory);
            else
                LoadDBases(dbDirectory);
        }

        private static void LoadDBases(string dbDirectory)
        {
            foreach (FileInfo file in new DirectoryInfo(dbDirectory).GetFiles())
            {
                if (file.Extension == ".sldb")
                    AddDBase(file);
            }
        }

        public static void Link(string dbDirectory)
        {
            DBDirectory = dbDirectory;
        }

        public static string[] SendCommand(string command)
        {
            List<string> commandContent = command.SplitCommand().ToList();

            string baseCommand = commandContent[0];
            commandContent.RemoveAt(0);

            string[] arguments = commandContent.ToArray();

            string[] result;

            try
            {
                switch (baseCommand)
                {
                    case "CREATE":
                        result = TIL.Create(arguments);
                        break;
                    case "EDIT":
                        result = TIL.Edit(arguments);
                        break;
                    case "DELETE":
                        result = TIL.Delete(arguments);
                        break;
                    case "GET":
                        return EVL.Get(arguments);
                    case "INSERT":
                        result = EVL.Insert(arguments);
                        break;
                    case "CHANGE":
                        result = EVL.Change(arguments);
                        break;
                    case "REMOVE":
                        result = EVL.Remove(arguments);
                        break;
                    default:
                        result = CommandStatus.UnknownCommand(baseCommand);
                        break;
                }
            }
            catch(Exception ex)
            {
                result = CommandStatus.UnknownException(ex);
            }

            return SLDBSettings.Logging ? result : new string[] { "" };
        }

        public static string FormatDirectory(string databaseName)
        {
            RemoveExtraPathSigns();
            string subDirectory = DBDirectory != "" ? $"{DBDirectory}/" : "";
            return $"{subDirectory}{databaseName}.sldb";
        }

        public static SLDB AddDBase(FileInfo file)
        {
            SLDB db = new SLDB(file);
            SLDBases.Add(db);
            return db;
        }

        public static SLDB GetDB(string name)
        {
            if (SLDBases.Count == 0)
                return null;

            foreach (SLDB database in SLDBases)
            {
                if (database.AssemblyName == name.ToLower())
                    return database;
            }

            return null;
        }

        public static void RemoveDBase(SLDB database)
        {
            SLDBases.Remove(database);
        }

        private static void RemoveExtraPathSigns()
        {
            if (DBDirectory.Length > 0)
            {
                int lastChar = DBDirectory.Length - 1;
                if (DBDirectory[lastChar] == '/')
                    DBDirectory.Remove(lastChar);
            }
        }
    }
}
