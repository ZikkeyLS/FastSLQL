using System.Collections.Generic;
using System.Text;

namespace FastSLQL.Format
{
    public class ElementConstructor
    {
        public List<string> arguments = new List<string>();

        #region AddArgument
        public void AddArgument(object value) => arguments.Add($"\"{value}\"");
        public void AddArray(object[] values)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("{");

            for(int i = 0; i < values.Length; i++)
            {
                string additive = i == values.Length - 1 ? "" : ", ";
                builder.Append(arguments[i] + additive);
            }

            builder.Append("}");

            arguments.Add(builder.ToString());
        }
        #endregion

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(");

            for (int i = 0; i < arguments.Count; i++)
            {
                string additive = i == arguments.Count - 1 ? "" : " | ";
                builder.Append(arguments[i] + additive);
            }

            builder.Append(")");

            return builder.ToString();
        }
    }
}
