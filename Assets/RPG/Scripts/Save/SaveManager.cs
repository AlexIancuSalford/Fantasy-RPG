using System.Collections.Generic;
using RPG.Helper;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            Dictionary<string, object> loadDictionary = (Dictionary<string, object>)CSerializer.ReadFromFile(fileName);

            SaveState(loadDictionary);

            CSerializer.WriteToFile(fileName, loadDictionary);
        }

        public Dictionary<string, object> Load(string fileName)
        {
            return LoadState(CSerializer.ReadFromFile(fileName));
        }

        private Dictionary<string, object> LoadState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;

            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                if (stateDictionary.ContainsKey(entity.UUID))
                {
                    entity.LoadState(stateDictionary[entity.UUID]);
                }
            }

            return stateDictionary;
        }

        private void SaveState(Dictionary<string, object> stateDictionary)
        {
            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                stateDictionary[entity.UUID] = entity.SaveState();
            }
        }
    }
}
