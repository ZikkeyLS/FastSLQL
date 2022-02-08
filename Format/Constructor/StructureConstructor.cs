using System.Collections.Generic;
using System.Text;

namespace FastSLQL.Format
{
    public class StructureConstructor
    {
        public List<string> arguments = new List<string>();

        #region AddArgument
        public void AddArgument(string name) => AddArgument(name, false, -1, -1);
        public void AddArgument(string name, bool unique) => AddArgument(name, unique, -1, -1);
        public void AddArgument(string name, int min, int max) => AddArgument(name, false, min, max);

        public void AddArgument(string name, bool unique, int min, int max)
        {
            bool minDefault = min == -1;

            StringBuilder builder = new StringBuilder();

            builder.Append(name);

            if (unique || !minDefault)
                builder.Append("(");

            if (unique)
                builder.Append("U");

            if (unique && !minDefault)
                builder.Append(",");

            if(min != -1)
            {
                builder.Append($"Min({min}),");
                builder.Append($"Max({max})");
            }

            if (unique || !minDefault)
                builder.Append(")");

            arguments.Add(builder.ToString());
        }
        #endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(");

            for(int i = 0; i < arguments.Count; i++)
            {
                string additive = i == arguments.Count - 1 ? "" : " | ";
                builder.Append(arguments[i] + additive);
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
