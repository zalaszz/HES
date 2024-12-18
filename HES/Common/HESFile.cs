using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public class HESFile
    {
        private string _FILENAME;
        private const string _DIR_IN = @"\in\";
        private const string _DIR_OUT = @"\out\";
        public HESFile(string fileName)
        {
            _FILENAME = fileName;
            CreateDirsIfRequired();
        }

        public Dictionary<string, Dictionary<char, VK_CODE>> ReadSettings()
        {
            string data = File.ReadAllText(_FILENAME);
            var dataJson = JsonSerializer.Deserialize<Dictionary<string, Dictionary<char, string>>>(data);

            Dictionary<string, Dictionary<char, VK_CODE>> dataJsonConverted = new Dictionary<string, Dictionary<char, VK_CODE>>();

            foreach (var item in dataJson)
            {
                Dictionary<char, VK_CODE> subDictionary = new Dictionary<char, VK_CODE>();

                foreach (var subItem in item.Value)
                {
                    subDictionary.Add(subItem.Key, (VK_CODE)Enum.Parse(typeof(VK_CODE), subItem.Value));
                }

                dataJsonConverted.Add(item.Key, subDictionary);
            }

            return dataJsonConverted;
        }

        public List<string> ReadCsvData()
        {
            List<string> csvData = new List<string>() { string.Empty, string.Empty, string.Empty };
            try
            {
                using (StreamReader reader = new StreamReader($"{GetInFullPath()}{_FILENAME}"))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(';');

                        for (int i = 0; i < csvData.Count; i++)
                        {
                            if (values[i].Any(c => char.IsLetter(c)))
                            {
                                throw new HESException();
                            }
                        }

                        csvData[0] += $"{values[0]} ";
                        csvData[1] += $"{values[1]} ";
                        csvData[2] += $"{values[2]} ";
                    }
                }
            }
            catch (Exception e)
            {
                new HESException("CSV data came in an unexpected format...", e);
                Console.ReadKey();
                Environment.Exit(1);
            }
            finally
            {
                File.Move($"{GetInFullPath()}data.csv", $"{GetOutFullPath()}data_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv.out");
            }

            return csvData;
        }

        private void CreateDirsIfRequired()
        {
            if (!HasInDir()) Directory.CreateDirectory(GetInFullPath());
            if (!HasOutDir()) Directory.CreateDirectory(GetOutFullPath());
        }

        public bool HasInDir()
        {
            return Directory.Exists(GetInFullPath());
        }

        public bool HasOutDir()
        {
            return Directory.Exists(GetOutFullPath());
        }

        public bool HasFile()
        {
            return _FILENAME.Contains(".csv") ? File.Exists($"{GetInFullPath()}{_FILENAME}") : File.Exists($"{_FILENAME}");
        }

        public string GetInFullPath()
        {
            return Directory.GetCurrentDirectory() + _DIR_IN;
        }

        public string GetOutFullPath()
        {
            return Directory.GetCurrentDirectory() + _DIR_OUT;
        }
    }
}
