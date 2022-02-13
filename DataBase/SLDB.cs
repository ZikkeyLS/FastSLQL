using FastSLQL.Format;
using FastSLQL.Support;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FastSLQL
{
    public sealed  class SLDB
    {
        private FileInfo _dbFile;
        private List<string> _data = new List<string>();
        private bool _readed = false;

        public string[] SplittedStructure { get; private set; }
        private List<int[]> StructuresParametres = new List<int[]>();

        public string Directory => _dbFile.FullName;
        public string AssemblyName => Path.GetFileNameWithoutExtension(_dbFile.Name).ToLower();
        public string[] Data => _data.ToArray();

        public SLDB(FileInfo file)
        {
            _dbFile = file;
        }

        public void ReadData()
        {
            _readed = true;

            _data = DBFileSystem.DeSerialize(Directory).ToList();

            UpdateParametres();
        }

        #region Get
        public string GetDBName()
        {
            ReadCheck();
            return _data[0];
        }

        public string GetDBStructure()
        {
            ReadCheck();
            return _data[1];
        }

        public string GetElement(int index)
        {
            ReadCheck();

            index += FSLQLSettings.LongSetupLenght; // skip information data and go to values
            if (_data.Count >= index)
                return _data[index - 1];

            return "";
        }

        public int GetElementIndex(string element)
        {
            for (int i = FSLQLSettings.LongSetupLenght; i < _data.Count; i++)
            {
                if (_data[i] == element)
                    return i - FSLQLSettings.ShortSetupLenght;
            }

            return -1;
        }

        public string[] GetElements()
        {
            ReadCheck();

            int index = FSLQLSettings.LongSetupLenght;
            List<string> result = new List<string>();

            for (int i = index; i < _data.Count; i++)
                if (_data[i] != "")
                    result.Add(_data[i]);

            return result.ToArray();
        }

        public string[] GetElements(string[] restrictions)
        {
            ReadCheck();

            Dictionary<int, string> formedRestrictions = SLDBSupport.FormRestrictions(SplittedStructure, restrictions);

            return SLDBSupport.GetFilteredElements(_data, formedRestrictions);
        }

        #endregion

        #region Write
        private void RewriteData()
        {
            DBFileSystem.Serialize(Directory, Data);
        }

        private void RenameFile(string name)
        {
            string _oldFile = _dbFile.FullName;
            _dbFile.MoveTo(Path.Combine(_dbFile.Directory.FullName, $"{name}.sldb"));

            if (File.Exists(_oldFile))
                File.Delete(_oldFile);
        }

        public void WriteDBName(string name)
        {
            ReadCheck();
            _data[0] = name;
            RewriteData();
            RenameFile(name);
        }

        public void WriteDBStructure(string structure)
        {
            ReadCheck();
            _data[1] = structure;

            StringBuilder separator = new StringBuilder();
            for (int i = 0; i < structure.Length; i++)
                separator.Append('-');

            _data[FSLQLSettings.ShortSetupLenght] = separator.ToString();
            RewriteData();
            UpdateParametres();
        }

        public void InsertElement(string element)
        {
            ReadCheck();
            _data.Add(element);
            RewriteData();
        }

        public void ChangeElement(int index, string element)
        {
            ReadCheck();
            _data[index + FSLQLSettings.ShortSetupLenght] = element;
            RewriteData();
        }


        public bool RemoveElement(int index)
        {
            ReadCheck();

            bool result = index >= 0 && index < _data.Count - FSLQLSettings.LongSetupLenght;
            if (result)
                _data.RemoveAt(index + FSLQLSettings.LongSetupLenght);

            RewriteData();

            return result;
        }

        public bool RemoveElement(string element)
        {
            ReadCheck();
            bool _result = _data.Remove(element);
            RewriteData();

            return _result;
        }

        private void UpdateParametres()
        {
            SplittedStructure = GetDBStructure().Split(" | ");

            for (int i = 0; i < SplittedStructure.Length; i++)
                StructuresParametres.Add(SLDBSupport.GetStructureParametres(SplittedStructure[i]));
        }
        #endregion

        #region Checks
        private void ReadCheck()
        {
            if (_readed == false)
                ReadData();
        }

        public bool VerifyParameters(string _data, int _exceptionIndex = -1)
        {
            ReadCheck();

            string[] splittedData = _data.Split(" | ");

            if (SplittedStructure.Length != splittedData.Length)
                return false;

            int i = FSLQLSettings.LongSetupLenght;

            return CheckUniqueParameters(i, splittedData, _exceptionIndex - 1 + i) && CheckMinMaxParameters(splittedData);
        }

        private bool CheckUniqueParameters(int index, string[] splittedData, int exceptionIndex = -1)
        {
            while(index != _data.Count)
            {
                if (index == exceptionIndex)
                {
                    index++;
                    continue;
                }

                for (int a = 0; a < splittedData.Length; a++)
                {
                    bool _elementsEquals = splittedData[a] == _data[index].Split(" | ")[a].Replace(" ", "");
                    if (StructuresParametres[a][0] == 1 && _elementsEquals)
                        return false;
                }

                index++;
            }

            return true;
        }


        private bool CheckMinMaxParameters(string[] splittedData)
        {
            for (int index = 0; index < splittedData.Length; index++)
            {
                if (!SLDBSupport.MinLenghtCorrect(StructuresParametres[index][1], splittedData[index])
                    || !SLDBSupport.MaxLenghtCorrect(StructuresParametres[index][FSLQLSettings.ShortSetupLenght], splittedData[index]))
                    return false;
            }

            return true;
        }
        #endregion
    }
}
