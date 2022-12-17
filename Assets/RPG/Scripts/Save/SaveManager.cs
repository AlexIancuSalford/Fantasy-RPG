using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RPG.Helper;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            CSerializer.WriteToFile(fileName, SaveState());

            Vector3 vector3 = new Vector3(10, 11, 12);
        }

        public void Load(string fileName)
        {
            LoadState(CSerializer.ReadFromFile(fileName));
        }

        private void LoadState(object state)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)state;
            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                entity.LoadState(stateDictionary[entity.UUID]);
            }
        }

        private object SaveState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            foreach (SaveableEntity entity in FindObjectsOfType<SaveableEntity>())
            {
                state[entity.UUID] = entity.SaveState();
            }

            throw new NotImplementedException();
        }
    }
}
