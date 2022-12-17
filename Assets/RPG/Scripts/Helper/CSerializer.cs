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
        public static Vector3 DeserializedVector { get; private set; }

        public static byte[] SerializeVector(Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

            return vectorBytes;
        }

        public static void WriteToFile(string fileName, object obj)
        {
            string path = GetPathFromFile(fileName);
            Debug.Log("Saving to file " + path);

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        public static object ReadFromFile(string fileName)
        {
            string path = GetPathFromFile(fileName);

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(fs);
            }
        }

        private static string GetPathFromFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".save");
        }
    }
}
