using RPG.Dialogue;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using static RPG.Dialogue.DialogueEnums;
using DialogueObject = RPG.Dialogue.Dialogue;

namespace RPG.Helper
{
    /// <summary>
    /// This script is a Unity Editor window that allows the user to edit a dialogue. The window has a scroll
    /// view and allows the user to create, delete, and connect nodes that represent dialogue options. The user
    /// can also move and resize these nodes within the scroll view. When the user selects a node, they can also
    /// edit the node's properties such as the dialogue text and the options that are available for the player
    /// to choose from. The window is opened by selecting the "Window/Dialogue/Dialogue Editor" menu item in Unity,
    /// or by double clicking a DialogueObject asset in the project window. The OnOpenAsset attribute ensures that
    /// the window is opened when the user double clicks a DialogueObject asset. The NonSerialized attribute on some
    /// of the fields indicates that they should not be serialized by Unity's serialization system.
    /// </summary>
    public class DialogueEditorWindow : EditorWindow
    {
        /// <summary>
        /// The dialogue being edited in the window
        /// </summary>
        private DialogueObject Dialogue { get; set; } = null;

        /// <summary>
        /// The scroll position for the scroll view
        /// </summary>
        private Vector2 ScrollPosition { get; set; } = new Vector2();

        /// <summary>
        /// The style for the nodes
        /// </summary>
        [field : NonSerialized] private GUIStyle NodeStyle { get; set; } = new GUIStyle();

        /// <summary>
        /// The style for the nodes where the player is speaking
        /// </summary>
        [field: NonSerialized] private GUIStyle PlayerNodeStyle { get; set; } = new GUIStyle();

        /// <summary>
        /// The title of the window
        /// </summary>
        [field : NonSerialized] private static string WindowTitle { get; set; } = "Dialogue Editor";

        /// <summary>
        /// The currently selected node
        /// </summary>
        [field : NonSerialized] private Node SelectedNode { get; set; } = null;

        /// <summary>
        /// The offset between the mouse position and the position of the selected node when the user starts dragging the node
        /// </summary>
        [field : NonSerialized] private Vector2 Offset { get; set; } = new Vector2();

        [NonSerialized] private Node newNode = null;                // A new node that is being added by the user
        [NonSerialized] private Node deleteNode = null;             // A node that is being deleted by the user
        [NonSerialized] private Node linkParentNode = null;         // A parent node that is being linked to a child node
        [NonSerialized] private bool isDraggingCanvas = false;      // Whether the user is currently dragging the canvas
        [NonSerialized] private Vector2 canvasOffset = new();       // The offset between the mouse position and the scroll position when the user starts dragging the canvas

        // The size of the background texture
        private const float backgroundSize = 50;

        /// <summary>
        /// Opens the Dialogue Editor window.
        /// </summary>
        [MenuItem("Window/Dialogue/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            // Show the window as a utility window and dock it with the SceneView window
            Type inspectorType = typeof(SceneView);
            GetWindow<DialogueEditorWindow>(WindowTitle, true, new Type[] { inspectorType });
        }

        /// <summary>
        /// Opens the Dialogue Editor window when the user double clicks a `DialogueObject` asset in the project window.
        /// </summary>
        /// <param name="instanceID">The instance ID of the selected asset.</param>
        /// <param name="line">The line number in the asset.</param>
        /// <returns>True if the asset is a `DialogueObject`, false otherwise.</returns>
        [OnOpenAsset(1)]
        public static bool OnOpenAssetWindow(int instanceID, int line)
        {
            // Get the Dialogue scriptable object that is being opened by ID
            DialogueObject dialogue = EditorUtility.InstanceIDToObject(instanceID) as DialogueObject;

            // If the object being opened is not a dialog scriptable object, do nothing
            if (dialogue == null) { return false; }

            ShowEditorWindow();
            return true;
        }

        private void OnEnable()
        {
            // Set the `Dialogue` property to the currently selected object if it is a `DialogueObject` when the selection changes
            Selection.selectionChanged += () =>
            {
                DialogueObject newDialogue = Selection.activeObject as DialogueObject;

                if (newDialogue != null)
                {
                    Dialogue = newDialogue;
                    Repaint();
                }
            };

            // Set the style for the nodes
            SetNodeStyle();
        }

        private void OnGUI()
        {
            // If no `DialogueObject` is selected, display a message and return
            if (Dialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected");
                return;
            }

            // Process user input events
            ProcessEvents();

            // Begin a scroll view
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);

            // Draw the canvas
            Rect canvas = GUILayoutUtility.GetRect(Dialogue.DefaultCanvasSize.x, Dialogue.DefaultCanvasSize.y);
            Texture2D texture = Resources.Load("background") as Texture2D;
            Rect textureCoordinates = new Rect(
                0, 
                0, 
                Dialogue.DefaultCanvasSize.x/backgroundSize, 
                Dialogue.DefaultCanvasSize.y/backgroundSize);
            
            GUI.DrawTextureWithTexCoords(canvas, texture, textureCoordinates);

            // Draw the nodes and connections
            foreach (Node node in Dialogue.Nodes)
            {
                OnGUINode(node);
                DrawConnections(node);
            }

            // End the scroll view
            EditorGUILayout.EndScrollView();

            // Process user actions for adding a new node
            ProcessDialogAction(ref newNode, ActionType.Add);
            // Process user actions for deleting a node
            ProcessDialogAction(ref deleteNode, ActionType.Delete);
        }

        /// <summary>
        /// Processes user input events.
        /// </summary>
        private void ProcessEvents()
        {
            switch (Event.current.type)
            {
                // When the user starts dragging a node or the canvas
                case EventType.MouseDown when SelectedNode == null:
                {
                    // Check if the user clicked on a node
                    SelectedNode = GetNodeAtPosition(Event.current.mousePosition + ScrollPosition);
                    // Calculate the offset between the mouse position and the position of the selected node
                    if (SelectedNode != null)
                    {
                        Offset = SelectedNode.GetRect().position - Event.current.mousePosition;
                        Selection.activeObject = SelectedNode;
                    }
                    else
                    {
                        // If the user didn't click on a node, start dragging the canvas
                        isDraggingCanvas = true;
                        canvasOffset = Event.current.mousePosition + ScrollPosition;
                        Selection.activeObject = Dialogue;
                    }

                    break;
                }
                // When the user is dragging a node
                case EventType.MouseDrag when SelectedNode != null:
                    // Update the position of the selected node
                    SelectedNode.SetRectPosition(Event.current.mousePosition + Offset);
                    GUI.changed = true;
                    break;
                // When the user is dragging on the canvas
                case EventType.MouseDrag when isDraggingCanvas:
                    // Update the scroll position (the coordinated of the canvas)
                    ScrollPosition = canvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                    break;
                // When the user has finished dragging a node
                case EventType.MouseUp when SelectedNode != null:
                    // Set the node to null (i.e. no node is being dragged at the moment)
                    SelectedNode = null;
                    EditorUtility.SetDirty(Dialogue);
                    break;
                // When the user has finished dragging on the canvas
                case EventType.MouseUp when isDraggingCanvas:
                    // Set the canvas dragging bool flag to false
                    isDraggingCanvas = false;
                    EditorUtility.SetDirty(Dialogue);
                    break;
            }
        }

        /// <summary>
        /// Draws a node in the editor window.
        /// </summary>
        /// <param name="node">The node to draw.</param>
        private void OnGUINode(Node node)
        {
            GUIStyle style = node.CurrentSpeaker switch
            {
                Speaker.Player => PlayerNodeStyle,
                _ => NodeStyle
            };

            // Begins drawing a node based on the node style defined
            GUILayout.BeginArea(node.GetRect(), style);

            // Change the nodes' text to be the text in the text field
            node.SetText(EditorGUILayout.TextField(node.Text));

            // Draw the buttons on the node
            DrawButtons(node);

            GUILayout.EndArea();
        }

        /// <summary>
        /// Sets the style for the nodes.
        /// </summary>
        private void SetNodeStyle()
        {
            // Set the node style to use the default label style
            NodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            // Set node padding
            NodeStyle.padding = new RectOffset(
                20,
                20,
                20,
                20
            );
            // Set node border
            NodeStyle.border = new RectOffset(
                12,
                12,
                12,
                12
            );
            // Set node text color
            PlayerNodeStyle.normal.textColor = Color.white;

            // Set the node style to use the default label style
            PlayerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            // Set node padding
            PlayerNodeStyle.padding = new RectOffset(
                20,
                20,
                20,
                20
            );
            // Set node border
            PlayerNodeStyle.border = new RectOffset(
                12,
                12,
                12,
                12
            );
            // Set node text color
            PlayerNodeStyle.normal.textColor = Color.white;
        }

        /// <summary>
        /// Gets the node at a given position in the editor window.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>The node at the position, or null if there is no node at the position.</returns>
        private Node GetNodeAtPosition(Vector2 mousePosition)
        {
            return Dialogue.Nodes.LastOrDefault(node => node.GetRect().Contains(mousePosition));
        }

        /// <summary>
        /// Draws connections between nodes.
        /// </summary>
        /// <param name="node">The parent node.</param>
        private void DrawConnections(Node node)
        {
            // Calculate the position of the parent node in the scroll view
            Vector3 startPos = new Vector3(node.GetRect().xMax, node.GetRect().center.y);
            
            // Draw a line to each child node
            foreach (Node childNode in Dialogue.GetAllNodeChildren(node))
            {
                // Calculate the position of the child node in the scroll view
                Vector3 endPos = new Vector3(childNode.GetRect().xMin, childNode.GetRect().center.y);
                // Calculate the offset from the mouses' position to the center of the node
                // This is done so, when the node is dragged, it is dragged from the click position
                Vector3 offset = CalculateOffset(startPos, endPos);

                // Draw a line (with a bezier curve) from the parent node to the child node
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

        /// <summary>
        /// Calculate the offset between two vectors
        /// </summary>
        /// <param name="startPos">The start position vector</param>
        /// <param name="endPos">The end position vector</param>
        /// <returns>Returns the offset between the two with the y set to 0 and the x multiplied by 0.8</returns>
        private Vector3 CalculateOffset(Vector3 startPos, Vector3 endPos)
        {
            Vector3 offset = endPos - startPos;
            offset.y = 0;
            offset.x *= .8f;

            return offset;
        }

        /// <summary>
        /// Processes user actions for adding or deleting a node.
        /// </summary>
        /// <param name="node">The node being added or deleted.</param>
        /// <param name="actionType">The type of action being performed on the node.</param>
        private void ProcessDialogAction(ref Node node, ActionType actionType)
        {
            // If the user is not adding or deleting a node, return
            if (node == null) { return; }

            // Add or delete the node
            switch (actionType)
            {
                case ActionType.Add:
                    Dialogue.CreateNode(node);
                    break;
                case ActionType.Delete:
                    Dialogue.DeleteNode(node);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
            }
            
            // Set the node to null, since it is passed in as reference
            node = null;
        }

        /// <summary>
        /// Draw all node buttons in a horizontal orientation
        /// </summary>
        /// <param name="node">The node being added buttons</param>
        private void DrawButtons(Node node)
        {
            GUILayout.BeginHorizontal();

            DrawDeleteButton(node);
            DrawLinkingButton(node);
            DrawAddButton(node);

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a button on a node with the text Add
        /// </summary>
        /// <param name="node">The node to which the button is attached</param>
        private void DrawAddButton(Node node)
        {
            if (GUILayout.Button("Add"))
            {
                newNode = node;
            }
        }

        /// <summary>
        /// Draws a button on a node with the text Del (meaning delete)
        /// </summary>
        /// <param name="node">The node to which the button is attached</param>
        private void DrawDeleteButton(Node node)
        {
            if (GUILayout.Button("Del"))
            {
                deleteNode = node;
            }
        }

        /// <summary>
        /// This method is responsible for drawing a button that allows the user to link or unlink nodes in a node tree.
        /// </summary>
        /// <param name="node">The node to which the button is attached</param>
        private void DrawLinkingButton(Node node)
        {
            // If the linkParentNode is null, that means we are not currently linking any nodes.
            // In this case, draw a "Link" button that allows the user to start linking a node.
            if (linkParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    // When the "Link" button is clicked, we set the linkParentNode to the current node.
                    linkParentNode = node;
                }
            }
            // If the linkParentNode is not null and is equal to the current node, that means we are currently linking this node.
            // In this case, draw a "Cancel" button that allows the user to cancel the link.
            else if (linkParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    // When the "Cancel" button is clicked, we set the linkParentNode to null to cancel the link.
                    linkParentNode = null;
                }
            }
            // If the linkParentNode is not null and is not equal to the current node, that means we are linking a different node.
            // In this case, check if the current node is already a child of the linkParentNode.
            // If it is, draw a "Unlink" button that allows the user to unlink the current node from the linkParentNode.
            else if (linkParentNode.GetNodeChildren().Contains(node.name))
            {
                if (!GUILayout.Button("Unlink")) { return; }

                // When the "Unlink" button is clicked, remove the current node from the list of children of the linkParentNode
                // and set the linkParentNode to null.
                linkParentNode.RemoveNodeChild(node.name);
                linkParentNode = null;
            }
            // If the linkParentNode is not null, is not equal to the current node, and the current node is not already a child of the linkParentNode,
            // that means it is possible to link the current node to the linkParentNode as a child.
            // In this case, draw a "Child" button that allows the user to link the current node as a child of the linkParentNode.
            else
            {
                if (!GUILayout.Button("Child")) { return; }

                // When the "Child" button is clicked, add the current node to the list of children of the linkParentNode
                // and set the linkParentNode to null.
                linkParentNode.AddNodeChild(node.name);
                linkParentNode = null;
            }
        }
    }
}
