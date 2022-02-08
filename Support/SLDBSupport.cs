using System.Collections.Generic;
using System.Text;

namespace FastSLQL.Support
{
    internal static class SLDBSupport
    {
        #region Checks
        public static bool MinLenghtCorrect(int lenght, string element) => lenght == -1 || element.Length - SLDBSettings.ShortSetupLenght >= lenght;
        public static bool MaxLenghtCorrect(int lenght, string element) => lenght == -1 || element.Length - SLDBSettings.ShortSetupLenght <= lenght;
        #endregion

        #region Get
        public static string GetParametres(string parameter, int i)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int backSlash = 1;

            for (i += 1; i < parameter.Length - 1; i++)
            {
                backSlash += (parameter[i] == '(' ? 1 : 0) + (parameter[i] == ')' ? -1 : 0);

                if (backSlash == 0)
                    break;

                stringBuilder.Append(parameter[i]);
            }

            return stringBuilder.ToString();
        }

        public static int[] GetStructureParametres(string parameter)
        {
            int unique = 0;
            int min = -1;
            int max = -1;

            for (int i = 0; i < parameter.Length; i++)
            {
                if (parameter[i] == '(')
                {
                    string[] values = GetParametres(parameter, i).Replace(" ", "").Split(",");

                    for (i = 0; i < values.Length; i++)
                    {
                        if (values[i].Contains("U"))
                            unique = 1;
                        else if (values[i].Contains("Min"))
                            SetValueThrowBrackets(values, i, ref min);
                        else if (values[i].Contains("Max"))
                            SetValueThrowBrackets(values, i, ref max);
                    }

                    break;
                }
            }

            return new int[SLDBSettings.LongSetupLenght] { unique, min, max };
        }

        private static string GetSplittedStructureValueName(string fullName)
        {
            return fullName.Split('(')[0];
        }

        public static string[] GetFilteredElements(List<string> data, Dictionary<int, string> formedRestrictions)
        {
            int index = SLDBSettings.LongSetupLenght;
            List<string> result = new List<string>();

            for (int i = index; i < data.Count; i++)
            {
                if (data[i] == "")
                    continue;

                string[] separatedElement = SeparateElement(data[i]);
                bool success = true;

                for (int p = 0; p < separatedElement.Length; p++)
                    if (formedRestrictions.ContainsKey(p) && formedRestrictions.TryGetValue(p, out string restriction) && separatedElement[p].Replace("\"", "") != restriction)
                        success = false;

                if (success)
                    result.Add(data[i]);
            }

            return result.ToArray();
        }
        #endregion

        #region Set
        public static void SetValueThrowBrackets(string[] values, int i, ref int value)
        {
            if (values[i][SLDBSettings.LongSetupLenght] == '(')
            {
                StringBuilder stringBuilder = new StringBuilder();

                for (int a = 4; a < values[i].Length - 1; a++)
                {
                    stringBuilder.Append(values[i][a]);
                }

                int.TryParse(stringBuilder.ToString(), out value);
            }
        }
        #endregion

        #region Additive
        public static string RemoveFirstLastSymbol(string value)
        {
            value = value.Remove(0, 1);
            value = value.Remove(value.Length - 1);

            return value;
        }

        public static Dictionary<int, string> FormRestrictions(string[] splittedStructure, string[] restrictions)
        {
            Dictionary<int, string> formedRestrictions = new Dictionary<int, string>();

            for (int r = 0; r < restrictions.Length; r++)
            {
                string[] splittedRestriction = restrictions[r].Split('=');

                for (int s = 0; s < splittedStructure.Length; s++)
                    if (splittedRestriction[0] == GetSplittedStructureValueName(splittedStructure[s]))
                        formedRestrictions.Add(s, splittedRestriction[1].Replace("\"", ""));
            }

            return formedRestrictions;
        }

        private static string[] SeparateElement(string element)
        {
            return element.Split(" | ");
        }
        #endregion
    }
}
