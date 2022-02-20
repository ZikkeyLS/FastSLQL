using System.Text;

namespace FastSLQL.Format
{
    public static class CommandConstructor
    {
        #region TIL
        public static string TILCreate(string name) => $"CREATE {name}";
        public static string TILCreate(string name, string structure) => $"CREATE {name} {structure}";
        public static string TILCreate(string name, StructureConstructor structure) => $"CREATE {name} {structure}";

        public static string TILEdit(string name, string instruction, string finalValue) => $"EDIT {name} {instruction} {finalValue}";

        public static string TILDelete(string name) => $"DELETE {name}";
        #endregion

        #region EVL
        public static string EVLGet(string name, int id) => $"GET {name} {id}";
        public static string EVLGet(string name, string instruction, string[] filters = null) => $"GET {name} {instruction} {ParseFilters(filters)}";
        public static string EVLGet(string name, string instruction, string subInstruction, string[] filters = null) => $"GET {name} {instruction} {subInstruction} {ParseFilters(filters)}";

        public static string EVLInsert(string name, string structure) => $"INSERT {name} {structure}";
        public static string EVLInsert(string name, ElementConstructor element) => $"INSERT {name} {element}";

        public static string EVLChange(string name, int id, string finalValue) => $"CHANGE {name} {id} with {finalValue}";
        public static string EVLChange(string name, string[] filters, string finalValue) => $"CHANGE {name} {ParseFilters(filters)} with {finalValue}";

        public static string EVLRemove(string name, int id) => $"REMOVE {name} {id}";
        public static string EVLRemove(string name, string[] filters) => $"REMOVE {name} {ParseFilters(filters)}";
        #endregion

        #region Additive
        public static string ParseFilters(string[] filters = null)
        {
            if (filters != null)
            {
                StringBuilder builder = new StringBuilder();

                builder.Append("where ");

                for (int i = 0; i < filters.Length; i++)
                {
                    string additive = i == filters.Length - 1 ? "" : " ";
                    builder.Append(filters[i] + additive);
                }

                return builder.ToString();
            }

            return "";
        }
        #endregion
    }
}
