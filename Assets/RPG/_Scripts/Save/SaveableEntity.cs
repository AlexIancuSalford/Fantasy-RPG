using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Save
{
    /// <summary>
    /// The SaveableEntity script is a Unity component that allows a game object
    /// to be saved and loaded in the game.It has two main methods: SaveState and
    /// LoadState.These methods are used to serialize and deserialize the object's data.
    ///
    /// The SaveState method creates a dictionary and iterates over all the
    /// components attached to the game object that implement the ISaveableEntity
    /// interface. It then adds each component's state to the dictionary using the
    /// component's type as the key and the result of the SaveState method as the
    /// value.The dictionary is then returned as the object's state.
    /// 
    /// The LoadState method takes an object as a parameter, which is expected to
    /// be a dictionary containing the object's state. It iterates over all the
    /// components attached to the game object that implement the ISaveableEntity
    /// interface and checks if the dictionary contains a state for that component.
    /// If it does, it calls the LoadState method on the component, passing in the
    /// state from the dictionary as an argument.
    ///  
    /// There is also a UUID (universally unique identifier) field that is
    /// serialized and used to identify the object in the game.This field is
    /// checked for uniqueness and if it is not unique, a new value is generated
    /// and applied to the field.This is done in the Update method, which is only
    /// called in the Unity editor.
    /// 
    /// The _saveableEntities dictionary is used to store all the SaveableEntity
    /// components in the game. It is used to check the uniqueness of the UUID
    /// field and to remove any null entries.
    /// </summary>
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        [SerializeField] public string UUID = "";
        static Dictionary<string, SaveableEntity> _saveableEntities = new Dictionary<string, SaveableEntity>();

        /// <summary>
        /// Serializes the object's data and returns it as an object.
        /// </summary>
        /// <returns>The object's serialized data.</returns>
        public object SaveState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            
            // Iterate over all the components that implement ISaveableEntity
            foreach (ISaveableEntity saveable in GetComponents<ISaveableEntity>())
            {
                // Add the component's state to the dictionary using the component's type as the key
                state[saveable.GetType().ToString()] = saveable.SaveState();
            }
            return state;
        }

        /// <summary>
        /// Deserializes the object's data from the given state object.
        /// </summary>
        /// <param name="state">The object's serialized data.</param>
        public void LoadState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            // Iterate over all the components that implement ISaveableEntity
            foreach (ISaveableEntity saveable in GetComponents<ISaveableEntity>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    // Load the component's state from the dictionary
                    saveable.LoadState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            // Only run in the editor and when the game object is not being played
            // No need to run at runtime

            if (Application.IsPlaying(gameObject)) { return; }
            if (string.IsNullOrEmpty(gameObject.scene.path)) { return; }

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("UUID");

            // Generate a new UUID if the current one is not unique
            if (string.IsNullOrEmpty(serializedProperty.stringValue) || !IsUnique(serializedProperty.stringValue))
            {
                serializedProperty.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            // Add the SaveableEntity to the dictionary using the UUID as the key
            _saveableEntities[serializedProperty.stringValue] = this;
        }
#endif

        /// <summary>
        /// Determines if the given UUID is unique among all the SaveableEntity components.
        /// </summary>
        /// <param name="candidate">The UUID to check for uniqueness.</param>
        /// <returns>True if the UUID is unique, false otherwise.</returns>
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
