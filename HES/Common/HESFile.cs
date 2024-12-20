using System.IO;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using HES.Models;

/**
* Author: Ricardo Silva
* Date: 22-11-2024 
*/

namespace HES
{
    public static class HESFile
    {
        private const string _DEFAULT_IN_DIR = @"\in\";
        private const string _DEFAULT_OUT_DIR = @"\out\";

        public static void ReadJSON()
        {

        }

        public static Dictionary<string, Dictionary<char, VK_CODE>> ReadSettings(string file)
        {
            string data = File.ReadAllText(GetPath() + file);
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

        public static List<string> ReadCSV(string file)
        {
            return ReadCSV(file, $@"{GetPath() + _DEFAULT_IN_DIR}", $@"{GetPath() + _DEFAULT_OUT_DIR}");
        }

        public static List<string> ReadCSV(string file, string inPath, string outPath)
        {
            List<string> csvData = new List<string>() { string.Empty, string.Empty, string.Empty };
            try
            {
                using (StreamReader reader = new StreamReader(inPath + file))
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
                File.Move($"{inPath + file}", $"{outPath}data_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv.out");
            }

            return csvData;
        }

        public static void CreateDefaultDirsIfRequired()
        {
            CreateDirIfRequired(_DEFAULT_IN_DIR);
            CreateDirIfRequired(_DEFAULT_OUT_DIR);
        }

        public static void CreateDirIfRequired(string path)
        {
            if (!HasDir(path)) Directory.CreateDirectory(GetPath() + path);
        }

        public static bool HasDefaultDirs()
        {
            return HasDir(_DEFAULT_IN_DIR) && HasDir(_DEFAULT_OUT_DIR);
        }

        public static bool HasDir(string path)
        {
            return Directory.Exists(GetPath() + path);
        }

        public static bool HasFile()
        {
            return Directory.GetFiles(GetPath() + _DEFAULT_IN_DIR, "*.csv").Count().Equals(0) ? false : true;
        }

        public static bool HasFile(string file)
        {
            return File.Exists(GetPath() + file);
        }

        public static string GetPath()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
