using System;
using System.Linq;
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
        private Node SelectedNode { get; set; } = null;
        private Vector2 Offset { get; set; } = new Vector2();

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

            ProcessEvents();

            foreach (Node node in Dialogue.Nodes)
            {
                OnGUINode(node);
            }
        }

        private void ProcessEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown when SelectedNode == null:
                    SelectedNode = GetNodeAtPosition(Event.current.mousePosition);
                    Offset = SelectedNode != null
                        ? SelectedNode.RectPosition.position - Event.current.mousePosition
                        : Vector2.zero;
                    break;
                case EventType.MouseUp when SelectedNode != null:
                    SelectedNode = null;
                    break;
                case EventType.MouseDrag when SelectedNode != null:
                    Undo.RecordObject(Dialogue, "Move Dialogue Node");
                    SelectedNode.RectPosition.position = Event.current.mousePosition + Offset;
                    GUI.changed = true;
                    break;
            }
        }

        private void OnGUINode(Node node)
        {
            GUILayout.BeginArea(node.RectPosition, NodeStyle);

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

        private Node GetNodeAtPosition(Vector2 mousePosition)
        {
            return Dialogue.Nodes.LastOrDefault(node => node.RectPosition.Contains(mousePosition));
        }
    }
}
