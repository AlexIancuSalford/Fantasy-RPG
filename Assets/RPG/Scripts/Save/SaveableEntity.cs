using System;
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

        public static Dictionary<string, SaveableEntity> globalSaveableEntities =
            new Dictionary<string, SaveableEntity>();

        public object SaveState()
        {
            Dictionary<string, object> stateDictionary = new Dictionary<string, object>();
            foreach (ISaveableEntity saveableEntity in GetComponents<ISaveableEntity>())
            {
                stateDictionary[saveableEntity.GetType().ToString()] = saveableEntity.SaveState();
            }

            return stateDictionary;
        }

        public void LoadState(object obj)
        {
            Dictionary<string, object> stateDictionary = (Dictionary<string, object>)obj;
            foreach (ISaveableEntity saveableEntity in GetComponents<ISaveableEntity>())
            {
                string type = saveableEntity.GetType().ToString();
                if (stateDictionary.ContainsKey(type))
                {
                    saveableEntity.LoadState(stateDictionary[type]);
                }
            }
        }

        #if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject obj = new SerializedObject(this);
            SerializedProperty property = obj.FindProperty("UUID");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                obj.ApplyModifiedProperties();
            }

            globalSaveableEntities[property.stringValue] = this;
        }
        #endif

        private bool IsUnique(string uuid)
        {
            if (!globalSaveableEntities.ContainsKey(uuid)) { return true; }

            if (globalSaveableEntities[uuid] == this) { return true; }

            if (globalSaveableEntities[uuid] == null || globalSaveableEntities[uuid].UUID != uuid)
            {
                globalSaveableEntities.Remove(uuid);
                return true;
            }

            return false;
        }
    }
}
