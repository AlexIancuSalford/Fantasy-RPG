using System;
using RPG.Dialogue;
using UnityEditor;
using UnityEditor.Callbacks;
using DialogueObject = RPG.Dialogue.Dialogue;

namespace RPG.Helper
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueObject Dialogue { get; set; } = null;
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
            DialogueObject dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueObject;

            if (dialogue == null) { return false; }

            ShowEditorWindow();
            return true;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += () =>
            {
                Dialogue = Selection.activeObject as DialogueObject;
                Repaint();
            };
        }

        private void OnGUI()
        {
            if (Dialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
                return;
            }

            foreach (Node node in Dialogue.Nodes)
            {
                EditorGUI.BeginChangeCheck();

                EditorGUILayout.LabelField("Node:");
                string newUUID = EditorGUILayout.TextField(node.UUID);
                string newText = EditorGUILayout.TextField(node.Text);

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(Dialogue, "Update Dialog");
                    node.Text = newText;
                    node.UUID = newUUID;
                }
            }
        }
    }
}
