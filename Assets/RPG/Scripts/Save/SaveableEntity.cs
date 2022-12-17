using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using RPG.Helper;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Save
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] public string UUID = "";

        public object SaveState()
        {
            return new Vector3f(transform.position);
        }

        public void LoadState(object obj)
        {
            GetComponent<NavMeshAgent>().enabled = false;

            Vector3f position = (Vector3f)obj;
            transform.position = position;

            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionManager>().CancelAction();
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
