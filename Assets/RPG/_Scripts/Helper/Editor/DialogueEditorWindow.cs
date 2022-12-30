using RPG.Dialogue;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using DialogueObject = RPG.Dialogue.Dialogue;

namespace RPG.Helper
{
    public class DialogueEditorWindow : EditorWindow
    {
        private DialogueObject Dialogue { get; set; } = null;
        private Vector2 ScrollPosition { get; set; } = new Vector2();
        [field : NonSerialized] private GUIStyle NodeStyle { get; set; } = new GUIStyle();
        [field : NonSerialized] private static string WindowTitle { get; set; } = "Dialogue Editor";
        [field : NonSerialized] private Node SelectedNode { get; set; } = null;
        [field : NonSerialized] private Vector2 Offset { get; set; } = new Vector2();

        [NonSerialized] private Node newNode = null;
        [NonSerialized] private Node deleteNode = null;
        [NonSerialized] private Node linkParentNode = null;
        [NonSerialized] private bool IsDraggingCanvas = false;
        [NonSerialized] private Vector2 CanvasOffset = new();

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

            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
            GUILayoutUtility.GetRect(Dialogue.DefaultCanvasSize.x, Dialogue.DefaultCanvasSize.y);

            foreach (Node node in Dialogue.Nodes)
            {
                OnGUINode(node);
                DrawConnections(node);
            }

            EditorGUILayout.EndScrollView();

            ProcessDialogAction(ref newNode, Node.ActionType.Add);
            ProcessDialogAction(ref deleteNode, Node.ActionType.Delete);
        }

        private void ProcessEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown when SelectedNode == null:
                {
                    SelectedNode = GetNodeAtPosition(Event.current.mousePosition + ScrollPosition);
                    if (SelectedNode != null)
                    {
                        Offset = SelectedNode.RectPosition.position - Event.current.mousePosition;
                    }
                    else
                    {
                        IsDraggingCanvas = true;
                        CanvasOffset = Event.current.mousePosition + ScrollPosition;
                    }

                    break;
                }
                case EventType.MouseDrag when SelectedNode != null:
                    Undo.RecordObject(Dialogue, "Move Dialogue Node");
                    SelectedNode.RectPosition.position = Event.current.mousePosition + Offset;
                    GUI.changed = true;
                    break;
                case EventType.MouseDrag when IsDraggingCanvas:
                    ScrollPosition = CanvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                    break;
                case EventType.MouseUp when SelectedNode != null:
                    SelectedNode = null;
                    EditorUtility.SetDirty(Dialogue);
                    break;
                case EventType.MouseUp when IsDraggingCanvas:
                    IsDraggingCanvas = false;
                    EditorUtility.SetDirty(Dialogue);
                    break;
            }
        }

        private void OnGUINode(Node node)
        {
            GUILayout.BeginArea(node.RectPosition, NodeStyle);

            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(node.Text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(Dialogue, "Update Dialog");
                node.Text = newText;
                EditorUtility.SetDirty(Dialogue);
            }

            DrawButtons(node);

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

        private void DrawConnections(Node node)
        {
            Vector3 startPos = new Vector3(node.RectPosition.xMax, node.RectPosition.center.y);
            foreach (Node childNode in Dialogue.GetAllNodeChildren(node))
            {
                Vector3 endPos = new Vector3(childNode.RectPosition.xMin, childNode.RectPosition.center.y);
                Vector3 offset = CalculateOffset(startPos, endPos);
                Handles.DrawBezier(
                    startPos, 
                    endPos, 
                    startPos + offset, 
                    endPos - offset, 
                    Color.white, 
                    null, 
                    4f);
            }
        }

        private Vector3 CalculateOffset(Vector3 startPos, Vector3 endPos)
        {
            Vector3 offset = endPos - startPos;
            offset.y = 0;
            offset.x *= .8f;

            return offset;
        }

        private void ProcessDialogAction(ref Node node, Node.ActionType actionType)
        {
            if (node == null) { return; }

            Undo.RecordObject(Dialogue, "Add/Delete Dialog Node");

            switch (actionType)
            {
                case Node.ActionType.Add:
                    Dialogue.CreateNode(node);
                    break;
                case Node.ActionType.Delete:
                    Dialogue.DeleteNode(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }

            EditorUtility.SetDirty(Dialogue);
            node = null;
        }

        private void DrawButtons(Node node)
        {
            GUILayout.BeginHorizontal();

            DrawDeleteButton(node);
            DrawLinkingButton(node);
            DrawAddButton(node);

            GUILayout.EndHorizontal();
        }

        private void DrawAddButton(Node node)
        {
            if (GUILayout.Button("Add"))
            {
                newNode = node;
            }
        }

        private void DrawDeleteButton(Node node)
        {
            if (GUILayout.Button("Del"))
            {
                deleteNode = node;
            }
        }

        private void DrawLinkingButton(Node node)
        {
            if (linkParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkParentNode = node;
                }
            }
            else if (linkParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkParentNode = null;
                }
            }
            else if (linkParentNode.NodeChildren.Contains(node.UUID))
            {
                if (!GUILayout.Button("Unlink")) { return; }
                Undo.RecordObject(Dialogue, "Remove Dialogue Link");
                linkParentNode.NodeChildren.Remove(node.UUID);
                linkParentNode = null;
            }
            else
            {
                if (!GUILayout.Button("Child")) { return; }
                Undo.RecordObject(Dialogue, "Add Dialogue Link");
                linkParentNode.NodeChildren.Add(node.UUID);
                linkParentNode = null;
            }
        }
    }
}
