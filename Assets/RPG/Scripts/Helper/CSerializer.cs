using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public static Vector3 DeserializeVector(byte[] bufferBytes)
        {
            Vector3 vector = new Vector3
            {
                x = BitConverter.ToSingle(bufferBytes, 0),
                y = BitConverter.ToSingle(bufferBytes, 4),
                z = BitConverter.ToSingle(bufferBytes, 8)
            };

            return vector;
        }

        public static void WriteToFile(string fileName, byte[] bufferBytes)
        {
            string path = GetPathFromFile(fileName);
            Debug.Log("Saving to file " + path);

            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                fs.Write(bufferBytes, 0, bufferBytes.Length);
            }
        }

        public static void ReadFromFile(string fileName)
        {
            string path = GetPathFromFile(fileName);
            Debug.Log("Load from file " + path);

            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);

                Vector3 vector = DeserializeVector(buffer);
                DeserializedVector = vector;

                Debug.Log("Loaded from file: " + vector);
            }
        }

        private static string GetPathFromFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".save");
        }
    }
}
