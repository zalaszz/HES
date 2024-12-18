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
    public static class HESFile
    {
        public static Dictionary<string, Dictionary<char, VK_CODE>> ReadSettings(string file)
        {
            string data = File.ReadAllText(file);
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

        public static List<string> ReadCsvData(string path)
        {
            List<string> csvData = new List<string>() { string.Empty, string.Empty, string.Empty };
            try
            {
                using (StreamReader reader = new StreamReader(path))
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
                File.Move($"{CreateDirIfRequired(@"\in\")}data.csv", $"{CreateDirIfRequired(@"\out\")}data_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv.out");
            }

            return csvData;
        }

        public static string CreateDirIfRequired(string path)
        {
            if (!HasDir(path)) Directory.CreateDirectory(GetPath() + path);
            return GetPath() + path;
        }

        public static bool HasDir(string path)
        {
            return Directory.Exists(GetPath() + path);
        }

        public static bool HasFile(string file)
        {
            return File.Exists(file);
        }

        public static string GetPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
