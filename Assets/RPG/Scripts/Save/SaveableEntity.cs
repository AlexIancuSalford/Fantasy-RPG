using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Save
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] public string UUID = "";

        public object SaveState()
        {
            Debug.Log("Capture state for entity: " + UUID);
            return null;
        }

        public void LoadState(object obj)
        {
            Debug.Log("Restore state for entity: " + UUID);
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject obj = new SerializedObject(this);
            SerializedProperty property = obj.FindProperty("UUID");

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                obj.ApplyModifiedProperties();
            }
        }
    }
#endif
}
