using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
    public class SaveManager : MonoBehaviour
    {
        public void Save(string fileName)
        {
            Debug.Log("Saving to file " + fileName);
        }

        public void Load(string fileName)
        {
            Debug.Log("Loading from file " + fileName);
        }
    }
}
