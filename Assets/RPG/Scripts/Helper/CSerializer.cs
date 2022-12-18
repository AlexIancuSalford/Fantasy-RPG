using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace RPG.Helper
{
    public static class CSerializer
    {
        public static void WriteToFile(string fileName, object obj)
        {
            string path = GetPathFromFile(fileName);

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        public static Dictionary<string, object> ReadFromFile(string fileName)
        {
            string path = GetPathFromFile(fileName);

            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(fs);
            }
        }

        private static string GetPathFromFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".save");
        }
    }
}
