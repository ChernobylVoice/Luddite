using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Elements
{
    using Enumerations;

    public class DSNode : Node
    {
        public string DialogueName { get; set; }
        public List<string> Choices { get; set; }
        public string Text { get; set; }
        public DSDialogueType DialogueType { get; set; }

        public virtual void Initialize(Vector2 position)
        {
            DialogueName = "DialgoueName";
            Choices = new List<string>();
            Text = "Dialogue text.";

            SetPosition(new Rect(position, Vector2.zero));
        }

        public virtual void Draw()
        {
            // Title Conatainer 작성.
            TextField dialgoueNameTextField = new TextField()
            {
                value = DialogueName
            };

            titleContainer.Insert(0, dialgoueNameTextField);

            // Input Container.
            Port inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            inputPort.portName = "Dialogue Connection";
            inputContainer.Add(inputPort);

            // Extension Container.
            VisualElement customDataContainer = new VisualElement();

            Foldout textFoldout = new Foldout()
            {
                text = "Dialgoue text."
            };

            TextField textTextField = new TextField()
            {
                value = Text
            };

            textFoldout.Add(textTextField);

            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}
