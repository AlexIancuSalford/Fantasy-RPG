using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            Debug.Log("Saving to file " + GetPathFromFile(fileName));
        }

        public void Load(string fileName)
        {
            Debug.Log("Loading from file " + GetPathFromFile(fileName));
        }

        private string GetPathFromFile(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".save");
        }
    }
}
