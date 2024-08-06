using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "RPGToolkit/New Dialogue Info", order = 14)]
    public class DialogueInfoSO : ScriptableObject
    {
        public string dialogueID;
        public List<DialogueNode> nodes;

        [System.Serializable]
        public class DialogueNode
        {
            public Sprite speakerSprite;
            public string speaker;
            public List<string> textLines;
            public List<DialogueOption> options;
            public bool startsQuest;
            public QuestInfoSO questInfo;
        }

        [System.Serializable]
        public class DialogueOption
        {
            public string optionText;
            public DialogueNode nextNode;
        }
    }
}