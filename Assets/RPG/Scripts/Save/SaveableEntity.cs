using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Save
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] public string UUID = "";
        static Dictionary<string, SaveableEntity> _saveableEntities = new Dictionary<string, SaveableEntity>();

        public object SaveState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveableEntity saveable in GetComponents<ISaveableEntity>())
            {
                state[saveable.GetType().ToString()] = saveable.SaveState();
            }
            return state;
        }

        public void LoadState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            foreach (ISaveableEntity saveable in GetComponents<ISaveableEntity>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.LoadState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("UUID");

            if (string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            _saveableEntities[serializedProperty.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!_saveableEntities.ContainsKey(candidate)) return true;

            if (_saveableEntities[candidate] == this) return true;

            if (_saveableEntities[candidate] == null)
            {
                _saveableEntities.Remove(candidate);
                return true;
            }

            if (_saveableEntities[candidate].UUID != candidate)
            {
                _saveableEntities.Remove(candidate);
                return true;
            }

            return false;
        }
    }
}
