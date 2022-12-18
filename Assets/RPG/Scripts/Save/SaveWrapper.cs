using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
    public class SaveWrapper : MonoBehaviour
    {
        public const string _defaultSaveFile = "save";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            GetComponent<SaveManager>().Save(_defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SaveManager>().Load(_defaultSaveFile);
        }
    }
}
