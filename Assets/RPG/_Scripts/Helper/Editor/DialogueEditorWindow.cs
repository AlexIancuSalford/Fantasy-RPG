using System;
using UnityEditor;

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
    }
}
