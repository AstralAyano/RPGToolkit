using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace RPGToolkit
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

        public DialogueInfoSO currentDialogue;
        private DialogueInfoSO.DialogueNode currentNode;
        private bool inDialogue;
        private int currentLineIndex;
        private Quest currentQuest;

        // UI components
        public GameObject dialogueUI;
        public Image speakerSprite;
        public TMP_Text speakerNameText;
        public TMP_Text dialogueText;
        public GameObject optionsPanel;
        public Button optionButtonPrefab;
        public GameObject dialoguePanel;
        public Button advanceButton;

        private Queue<Button> optionButtonPool = new Queue<Button>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            dialogueUI.SetActive(false);
            optionsPanel.SetActive(false);
        }

        private void Start()
        {
            EventTrigger trigger = dialoguePanel.GetComponent<EventTrigger>();

            if (trigger == null)
            {
                trigger = dialoguePanel.AddComponent<EventTrigger>();
            }

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) => { AdvanceDialogue(); });
            trigger.triggers.Add(entry);

            // Add click listener to the advance button
            //advanceButton.onClick.AddListener(AdvanceDialogue);
        }

        public void StartDialogue(DialogueInfoSO dialogue)
        {
            dialogueUI.SetActive(true);
            optionsPanel.SetActive(false);

            currentDialogue = dialogue;
            currentNode = dialogue.nodes[0];
            currentLineIndex = 0;
            inDialogue = true;
            DisplayNextLine();
        }

        private void DisplayNextLine()
        {
            optionsPanel.SetActive(false);

            if (currentNode != null)
            {
                if (currentLineIndex < currentNode.textLines.Count)
                {
                    speakerNameText.text = currentNode.speaker;
                    dialogueText.text = currentNode.textLines[currentLineIndex];

                    if (currentNode.speakerSprite != null)
                    {
                        speakerSprite.sprite = currentNode.speakerSprite;
                    }

                    currentLineIndex++;
                }
                else
                {
                    HandleQuestState();
                    
                    if (currentNode.options != null && currentNode.options.Count > 0)
                    {
                        CreateOptionButtons(currentNode.options);
                    }
                    else
                    {
                        EndDialogue();
                    }
                }
            }
            else
            {
                EndDialogue();
            }
        }

        private void HandleQuestState()
        {
            if (currentNode.questStartOrEnd && currentNode.questInfo != null)
            {
                currentQuest = GetQuestByID(currentNode.questInfo.questID);

                if (currentQuest != null)
                {
                    QuestState questState = currentQuest.state;

                    if (questState == QuestState.CAN_START)
                    {
                        EventsManager.Instance.questEvents.StartQuest(currentNode.questInfo.questID);
                    }
                    else if (questState == QuestState.CAN_FINISH)
                    {
                        EventsManager.Instance.questEvents.FinishQuest(currentNode.questInfo.questID);
                    }
                }
            }
        }

        private void CreateOptionButtons(List<DialogueInfoSO.DialogueOption> options)
        {
            foreach (Transform child in optionsPanel.transform)
            {
                ReleaseButton(child.GetComponent<Button>());
            }

            for (int i = 0; i < options.Count; i++)
            {
                var option = options[i];
                var button = GetButton();
                button.transform.SetParent(optionsPanel.transform, false);

                var buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = option.optionText;
                int optionIndex = i;
                button.onClick.AddListener(() => OnPlayerInput(optionIndex));
            }

            optionsPanel.SetActive(true);
        }

        private Button GetButton()
        {
            if (optionButtonPool.Count > 0)
            {
                var button = optionButtonPool.Dequeue();
                button.gameObject.SetActive(true);
                return button;
            }
            else
            {
                return Instantiate(optionButtonPrefab);
            }
        }

        private void ReleaseButton(Button button)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
            optionButtonPool.Enqueue(button);
        }

        public void OnPlayerInput(int optionIndex)
        {
            if (!inDialogue)
            {
                return;
            }

            if (optionIndex >= 0 && optionIndex < currentNode.options.Count)
            {
                currentNode = currentNode.options[optionIndex].nextNode;
                currentLineIndex = 0;
                DisplayNextLine();
            }
            else
            {
                Debug.LogWarning("Invalid option index.");
            }
        }

        private void AdvanceDialogue()
        {
            if (inDialogue)
            {
                DisplayNextLine();
            }
        }

        private void EndDialogue()
        {
            inDialogue = false;
            dialogueUI.SetActive(false);
            optionsPanel.SetActive(false);
        }

        private Quest GetQuestByID(string questID)
        {
            if (QuestManager.Instance == null)
            {
                Debug.LogError("QuestManager instance is not available.");
                return null;
            }

            return QuestManager.Instance.GetQuestByID(questID);
        }
    }
}