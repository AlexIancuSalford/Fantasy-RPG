using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Save
{
    public class SaveableEntity : MonoBehaviour
    {
        public string UUID { get; private set; }

        public object SaveState()
        {
            Debug.Log("Capture state for entity: " + UUID);
            return null;
        }

        public void LoadState(object obj)
        {
            Debug.Log("Restore state for entity: " + UUID);
        }
    }
}
