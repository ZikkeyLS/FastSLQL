using System.Collections.Generic;
using System.Linq;

namespace FastSLQL.Format
{
    public static class CommandFormatter
    {
        public static string[] SplitCommand(this string command)
        {
            char[] commandLetters = command.ToArray();
            List<string> parsedValues = new List<string>();

            string tempValue = string.Empty;
            bool elementBlocked = false;
            int blockedCount = 0;

            for(int i = 0; i < commandLetters.Length; i++)
            {
                char letter = commandLetters[i];
                bool used = false;

                if (letter == ' ' && elementBlocked == false)
                {
                    parsedValues.Add(tempValue);
                    tempValue = string.Empty;
                    continue;
                }
                else
                {
                    tempValue += letter;
                    CheckBlockState(letter, ref blockedCount, ref elementBlocked);
                }

                if (i == commandLetters.Length - 1 && used == false)
                    parsedValues.Add(tempValue);
            }

            return parsedValues.ToArray();
        }

        private static void CheckBlockState(char letter, ref int blockedCount, ref bool elementBlocked)
        {
            if (letter == '(')
                blockedCount += 1;
            else if (letter == ')' && blockedCount > 0)
                blockedCount -= 1;

            elementBlocked = blockedCount > 0;
        }
    }
}
