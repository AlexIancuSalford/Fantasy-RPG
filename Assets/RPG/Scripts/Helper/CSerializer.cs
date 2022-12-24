/*
 * 
 */

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Helper
{
    /// <summary>
    /// This script is a collection of static methods in a C# class in Unity that
    /// provide utility functions for serializing and deserializing objects to and
    /// from files.
    ///
    /// The WriteToFile() method takes a string fileName and an object obj as
    /// arguments, and serializes the object to a binary file with the specified
    /// file name.It first gets the full path to the file using the GetPathFromFile()
    /// method, and then creates a FileStream to the file using the File.Open()
    /// method in FileMode.Create mode.It then creates a BinaryFormatter, and
    /// uses the Serialize() method to serialize the object to the file stream.
    ///
    /// The ReadFromFile() method takes a string fileName as an argument, and
    /// deserializes an object from a binary file with the specified file name.
    /// It first gets the full path to the file using the GetPathFromFile() method,
    /// and then checks if the file exists.If the file does not exist, it returns
    /// an empty dictionary.If the file does exist, it creates a FileStream to the
    /// file using the File.Open() method in FileMode.Open mode, creates a
    /// BinaryFormatter, and uses the Deserialize() method to deserialize the
    /// object from the file stream.It then casts the deserialized object to a
    /// dictionary of strings and objects, and returns it.
    /// 
    /// The GetPathFromFile() method takes a string fileName as an argument, and
    /// returns the full path to a file with the specified file name in the
    /// Application.persistentDataPath folder. It combines the path to the folder
    /// and the file name using the Path.Combine() method.
    /// </summary>
    public static class CSerializer
    {
        /// <summary>
        /// Serializes an object to a binary file with the specified file name.
        /// </summary>
        /// <param name="fileName">The name of the file to serialize to.</param>
        /// <param name="obj">The object to serialize.</param>
        public static void WriteToFile(string fileName, object obj)
        {
            // Get the full path to the file.
            string path = GetPathFromFile(fileName);

            // Open a FileStream to the file in FileMode.Create mode, and serialize the object to the file stream.
            using (FileStream fs = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        /// <summary>
        /// Deserializes an object from a binary file with the specified file name.
        /// </summary>
        /// <param name="fileName">The name of the file to deserialize from.</param>
        /// <returns>The deserialized object, or an empty dictionary if the file does not exist.</returns>
        public static Dictionary<string, object> ReadFromFile(string fileName)
        {
            // Get the full path to the file.
            string path = GetPathFromFile(fileName);

            // If the file does not exist, return an empty dictionary.
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            // Open a FileStream to the file in FileMode.Open mode, and deserialize the object from the file stream.
            using (FileStream fs = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(fs);
            }
        }

        /// <summary>
        /// Returns the full path to a file with the specified file name in the Application.persistentDataPath folder.
        /// </summary>
        /// <param name="fileName">The name of the file to get the path for.</param>
        /// <returns>The full path to the file.</returns>
        public static string GetPathFromFile(string fileName)
        {
            // Combine the path to the Application.persistentDataPath folder and the file name.
            return Path.Combine(Application.persistentDataPath, fileName + ".save");
        }
    }
}
