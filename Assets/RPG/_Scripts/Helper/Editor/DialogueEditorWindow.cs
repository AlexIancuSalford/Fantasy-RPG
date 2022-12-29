using System;
using RPG.Dialogue;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using DialogueObject = RPG.Dialogue.Dialogue;

namespace RPG.Helper
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueObject Dialogue { get; set; } = null;
        private GUIStyle NodeStyle { get; set; } = new GUIStyle();
        private static string WindowTitle { get; set; } = "Dialogue Editor";

        [MenuItem("Window/Dialogue/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            Type inspectorType = typeof(SceneView);
            GetWindow<DialogueEditorWindow>(WindowTitle, true, new Type[] { inspectorType });
        }

        [OnOpenAsset(1)]
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

            SetNodeStyle();
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
                OnGUINode(node);
            }
        }

        private void OnGUINode(Node node)
        {
            GUILayout.BeginArea(node.Position, NodeStyle);

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            string newUUID = EditorGUILayout.TextField(node.UUID);
            string newText = EditorGUILayout.TextField(node.Text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Dialogue, "Update Dialog");
                node.Text = newText;
                node.UUID = newUUID;
            }

            GUILayout.EndArea();
        }

        private void SetNodeStyle()
        {
            NodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            NodeStyle.padding = new RectOffset(
                20,
                20,
                20,
                20
            );
            NodeStyle.border = new RectOffset(
                12,
                12,
                12,
                12
            );
            NodeStyle.normal.textColor = Color.white;
        }
    }
}
