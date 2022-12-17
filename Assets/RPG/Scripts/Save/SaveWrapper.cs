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
                GetComponent<SaveManager>().Save(_defaultSaveFile);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GetComponent<SaveManager>().Load(_defaultSaveFile);
            }
        }
    }
}
