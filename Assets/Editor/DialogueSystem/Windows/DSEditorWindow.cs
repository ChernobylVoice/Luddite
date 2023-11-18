using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Windows
{
    public class DSEditorWindow : EditorWindow
    {
        [MenuItem("Window/DS/Dialogue Graph")]
        public static void Open()
        {
            GetWindow<DSEditorWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            AddGraphView();

            AddStyles();
        }

        // Dialogue window에 GraphView추가.
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView();

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        // Style Variables추가.
        private void AddStyles()
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSVariables.uss");

            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}

