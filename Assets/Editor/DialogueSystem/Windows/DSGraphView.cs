using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{
    using Elements;
    using Enumerations;

    public class DSGraphView : GraphView
    {
        public DSGraphView()
        {
            AddManipulatiors();
            AddGridBackground();

            AddStyles();
        }

        private void AddManipulatiors()
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
        }

        // Background를 grid 형식으로 변경
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();

            gridBackground.StretchToParentSize();

            this.Insert(0, gridBackground);
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DSDialogueType dialogueType)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                    menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.localMousePosition)))
                );

            return contextualMenuManipulator;
        }

        // Node 생성.
        private DSNode CreateNode(DSDialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DS.Elements.DS{dialogueType}Node");

            DSNode node = (DSNode) Activator.CreateInstance(nodeType);

            node.Initialize(position);
            node.Draw();

            return node;
        }

        // DSGraphViewStyles로 스타일 변경
        private void AddStyles()
        {
            StyleSheet graphViewStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSGraphViewStyles.uss");
            StyleSheet NodeStyleSheet = (StyleSheet)EditorGUIUtility.Load("DialogueSystem/DSNodeStyles.uss");

            styleSheets.Add(graphViewStyleSheet);
        }
    }
}
