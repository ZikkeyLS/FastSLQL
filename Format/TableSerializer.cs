using System.Text;

namespace FastSLQL.Format
{
    public static class TableSerializer
    {
        public static string[] Serialize(string[] result, string[] data)
        {
            for (int i = 0; i < result.Length; i++)
            {
                byte[] byteDataPart = Encoding.Default.GetBytes(data[i]);
                string part = "";

                for (int a = 0; a < byteDataPart.Length; a++)
                {
                    string additive = a == byteDataPart.Length - 1 ? "" : " ";
                    part += byteDataPart[a].ToString() + additive;
                }

                result[i] = part;
            }

            return result;
        }

        public static string[] DeSerialize(string[] fileData)
        {
            for (int i = 0; i < fileData.Length; i++)
            {
                string[] splittedPart = fileData[i].Split(" ");
                byte[] bytePart = new byte[splittedPart.Length];

                for (int a = 0; a < bytePart.Length; a++)
                    bytePart[a] = byte.Parse(splittedPart[a]);


                fileData[i] = Encoding.Default.GetString(bytePart);
            }

            return fileData;
        }
    }
}
