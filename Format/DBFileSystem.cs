using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FastSLQL.Format
{
    internal static class DBFileSystem
    {
            public static void Serialize(string fileName, string[] data)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (Stream writer = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    bf.Serialize(writer, data);
                }
            }

            public static string[] DeSerialize(string fileName)
            {
                BinaryFormatter bf = new BinaryFormatter();
                using (Stream reader = new FileStream(fileName, FileMode.Open))
                {
                    if (reader.Length <= 0)
                        return new string[] { "", "", "" };

                    return (string[])bf.Deserialize(reader);
                }
            }
    }
}
