using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Helper
{
    public class DialogueEditorWindow : EditorWindow
    {
        private static string WindowTitle { get; set; } = "Dialogue Editor";

        [MenuItem("Window/Dialogue/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            Type inspectorType = typeof(UnityEditor.SceneView);
            GetWindow<DialogueEditorWindow>(WindowTitle, true, new Type[] { inspectorType });
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAssetWindow(int instanceID, int line)
        {
            Dialogue.Dialogue dialogue =  EditorUtility.InstanceIDToObject(instanceID) as Dialogue.Dialogue;

            if (dialogue == null) { return false; }
            
            ShowEditorWindow();
            return true;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Hello World!");
            EditorGUILayout.LabelField("Goodbye World");
        }
    }
}
