using UnityEditor;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Utilites;
    
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

        #region Elements Addition
        // Dialogue window에 GraphView추가.
        private void AddGraphView()
        {
            DSGraphView graphView = new DSGraphView(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        // Style Variables추가.
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }
        #endregion
    }
}

