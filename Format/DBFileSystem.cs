using System.IO;
using System.Text;

namespace FastSLQL.Format
{
    internal static class DBFileSystem
    {
        public static void Pack(string fileName, string[] data)
        {
            using FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);

            for (int i = 0; i < data.Length; i++)
            {
                writer.Write(data[i]);
            }
        }

        public static string[] Unpack(string fileName)
        {
            ulong linesCount = (ulong)File.ReadAllLines(fileName).LongLength;

            string[] data = new string[linesCount];

            using (FileStream stream = File.Open(fileName, FileMode.OpenOrCreate))
            {
                using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false);

                for (ulong i = 0; i < (ulong)data.LongLength; i++)
                {
                    data[i] = reader.ReadString();
                }
            }

            return data;
        }
    }
}
