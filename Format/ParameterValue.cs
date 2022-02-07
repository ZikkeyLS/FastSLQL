
namespace FastSLQL.Format
{
    public static class ParameterValue
    {
        public static string ParseString(string parameter) => parameter.Replace("\"", "");
        public static int ParseInt(string parameter) => int.Parse(ParseString(parameter));
        public static float ParseFloat(string parameter) => float.Parse(ParseString(parameter));
        public static double ParseDouble(string parameter) => double.Parse(ParseString(parameter));
        public static short ParseShort(string parameter) => short.Parse(ParseString(parameter));
        public static long ParseLong(string parameter) => long.Parse(ParseString(parameter));
        public static bool ParseBool(string parameter) => bool.Parse(ParseString(parameter));
    }
}
