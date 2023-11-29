using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Elements;
    using Enumerations;
    using Utilites;

    public class DSGraphView : GraphView
    {
        private DSEditorWindow _editorWindow;
        private DSSearchWindow _searchWindow;
        
        public DSGraphView(DSEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;
            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            AddStyles();
        }

        #region Override Methods
        // 각 노드와의 연결이 가능하도록
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port)
                    return;
                else if (startPort.node == port.node)
                    return;
                else if (startPort.direction == port.direction)
                    return;

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            // 확대 축소
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            // node를 드래그하여 움직일 수 있게, 여러 개 선택을 할 수 있게.
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // 우클릭시 create node 창 띄우기.
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DSDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DSDialogueType.MultipleChoice));
        
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                    menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
                );

            return contextualMenuManipulator;
        }
        
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

            return contextualMenuManipulator;
        }
        #endregion

        #region Elements Creation
        public Group CreateGroup(string title, Vector2 localMousePosition)
        {
            Group group = new Group()
            {
                title = title
            };
            
            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            return group;
        }
        
        // Node 생성.
        public DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");

            DSNode node = (DSNode) Activator.CreateInstance(nodeType ?? throw new InvalidOperationException());

            node.Initialize(position);
            node.Draw();

            return node;
        }
        #endregion
        
        #region Elements Addition
        // SearchWindow 생성용
        private void AddSearchWindow()
        {
            if (_searchWindow != null) return;
            _searchWindow = ScriptableObject.CreateInstance<DSSearchWindow>();
            _searchWindow.Initialize(this);

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        
        // Background를 grid 형식으로 변경
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            this.Insert(0, gridBackground);
        }

        // DSGraphViewStyles로 스타일 변경
        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
            );
        }
        #endregion

        #region Utilites
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, mousePosition - _editorWindow.position.position);
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
        #endregion
    }
}
