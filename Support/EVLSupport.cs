using System.Collections.Generic;
using System.Text;

namespace FastSLQL.Support
{
    internal static class EVLSupport
    {
        #region Get
        public static void AddAllElementsList(List<string> result, string[] arguments, SLDB database)
        {
            if (arguments.Length > 3 && arguments[3] == "where")
            {
                List<string> argumentsValues = new List<string>();

                for (int i = FSLQLSettings.LongSetupLenght; i < arguments.Length; i++)
                    if(arguments[i] != "")
                        argumentsValues.Add(arguments[i]);

                result.AddRange(database.GetElements(argumentsValues.ToArray()));
            }
            else
                result.AddRange(database.GetElements());
        }

        public static void AddFirstElements(List<string> result, string[] arguments, SLDB database)
        {
            if (!int.TryParse(arguments[2], out int lenghtFirst))
                return;

            if (arguments.Length > 4 && arguments[3] == "where")
            {
                List<string> argumentsValues = new List<string>();

                for (int i = FSLQLSettings.LongSetupLenght; i < arguments.Length; i++)
                    if (arguments[i] != "")
                        argumentsValues.Add(arguments[i]);

                string[] filteredElements = database.GetElements(argumentsValues.ToArray());

                int finalIndex = filteredElements.Length > lenghtFirst ? lenghtFirst : filteredElements.Length;

                if (finalIndex == 0)
                    return;

                for (int i = 0; i < finalIndex; i++)
                    result.Add(filteredElements[i]);
            }
            else
            {
                string[] elements = database.GetElements();

                int finalIndex = elements.Length > lenghtFirst ? lenghtFirst : elements.Length;

                if (finalIndex == 0)
                    return;

                for (int i = 0; i < finalIndex; i++)
                    result.Add(elements[i]);
            }
        }

        public static void AddLastElements(List<string> result, string[] arguments, SLDB database)
        {
            if (!int.TryParse(arguments[2], out int lenghtLast))
                return;

            if (arguments.Length > 4 && arguments[3] == "where")
            {
                List<string> argumentsValues = new List<string>();

                for (int i = FSLQLSettings.LongSetupLenght; i < arguments.Length; i++)
                    if (arguments[i] != "")
                        argumentsValues.Add(arguments[i]);

                string[] filteredElements = database.GetElements(argumentsValues.ToArray());

                if (filteredElements.Length == 0)
                    return;

                int finalIndex = filteredElements.Length >= lenghtLast ? lenghtLast : filteredElements.Length;

                for (int i = filteredElements.Length; i > filteredElements.Length - finalIndex; i--)
                    result.Add(filteredElements[i]);
            }
            else
            {
                string[] elements = database.GetElements();

                if (elements.Length == 0)
                    return;

                int finalIndex = elements.Length >= lenghtLast ? lenghtLast : elements.Length;

                for (int i = elements.Length - 1; i > elements.Length - 1 - finalIndex; i--)
                    result.Add(elements[i]);
            }
        }

        public static void AddElementsLenght(List<string> result, string[] arguments, SLDB database)
        {
            if (arguments.Length > 3 && arguments[2] == "where")
            {
                List<string> argumentsValues = new List<string>();

                for (int i = FSLQLSettings.LongSetupLenght; i < arguments.Length; i++)
                    if (arguments[i] != "")
                        argumentsValues.Add(arguments[i]);

                result.Add(database.GetElements(argumentsValues.ToArray()).Length.ToString());
            }
            else
                result.Add(database.GetElements().Length.ToString());
        }
        #endregion

        #region Change
        public static string[] ChangeByFormat(SLDB database, string[] arguments)
        {
            if (int.TryParse(arguments[1], out int index) && arguments[2] == "with")
            {
                if (index <= 0)
                    return CommandStatus.BadIndex;

                if (arguments[3].Contains("(") && arguments[3].Contains(")"))
                    return ChangeLongIndexedElement(database, arguments, index);
                else
                    return ChangeShortIndexedElement(database, arguments, index);
            }
            else if (arguments[1] == "where")
            {
                List<string> argumentsValues = new List<string>();
                argumentsValues.Add(arguments[2]);

                string[] elements = database.GetElements(argumentsValues.ToArray());

                if (arguments[4].Contains("(") && arguments[4].Contains(")"))
                    return ChangeLongElements(database, arguments, elements);
                else
                    return ChangeShortElements(database, arguments, elements);
            }

            return CommandStatus.InvalidArgumentFormatOrUnexpectedException;
        }

        private static string[] ChangeLongIndexedElement(SLDB database, string[] arguments, int index)
        {
            if (database.VerifyParameters(arguments[3]))
            {
                database.ChangeElement(index, arguments[3].Replace("(", "").Replace(")", ""));
                return CommandStatus.ChangeSuccess;
            }

            return CommandStatus.InvalidArgumentFormatOrUnexpectedException;
        }

        private static string[] ChangeShortIndexedElement(SLDB database, string[] arguments, int index)
        {
            string[] splittedElement = database.GetElement(index).Split(" | ");

            for (int i = 0; i < database.SplittedStructure.Length; i++)
                if (database.SplittedStructure[i].Split("(")[0] == arguments[3].Split("=")[0])
                    splittedElement[i] = arguments[3].Split("=")[1];

            string result = ReSplitElement(splittedElement);

            if (database.VerifyParameters(result, index))
            {
                database.ChangeElement(index, result);
                return CommandStatus.ChangeSuccess;
            }
            else
                return CommandStatus.InvalidArgumentFormatOrUnexpectedException;
        }

        private static string[] ChangeLongElements(SLDB database, string[] arguments, string[] elements)
        {
            string value = arguments[4].Replace("(", "").Replace(")", "");

            for (int i = 0; i < elements.Length; i++)
            {
                int elementIndex = database.GetElementIndex(elements[i]);

                if (database.VerifyParameters(value, elementIndex))
                    database.ChangeElement(elementIndex, value);
            }

            return CommandStatus.ChangeSuccess;
        }

        private static string[] ChangeShortElements(SLDB database, string[] arguments, string[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                int elementIndex = database.GetElementIndex(elements[i]);
                string[] splittedElement = elements[i].Split(" | ");

                for (int b = 0; b < database.SplittedStructure.Length; b++)
                    if (database.SplittedStructure[b].Split("(")[0] == arguments[4].Split("=")[0])
                        splittedElement[b] = arguments[4].Split("=")[1];

                string result = ReSplitElement(splittedElement);

                if (database.VerifyParameters(result, elementIndex))
                    database.ChangeElement(elementIndex, result);
            }

            return CommandStatus.ChangesSuccess;
        }

        public static string ReSplitElement(string[] splittedElement)
        {
            StringBuilder builder = new StringBuilder();

            for (int b = 0; b < splittedElement.Length; b++)
            {
                string additive = b == splittedElement.Length - 1 ? "" : " | ";
                builder.Append(splittedElement[b] + additive);
            }

            return builder.ToString();
        }
        #endregion

        #region Remove
        public static bool RemoveFilteredElements(string[] arguments, SLDB database)
        {
            List<string> argumentsValues = new List<string>();

            for (int i = FSLQLSettings.ShortSetupLenght; i < arguments.Length; i++)
                argumentsValues.Add(arguments[i]);

            string[] removalElements = database.GetElements(argumentsValues.ToArray());

            if (removalElements.Length == 0)
                return false;

            foreach (string element in removalElements)
                database.RemoveElement(element);

            return true;
        }
        #endregion
    }
}
