using UnityEngine;

namespace RPGToolkit
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestPoint2D : MonoBehaviour
    {
        [Header("Quest")]
        public QuestInfoSO questInfoForPoint;

        [Header("Dialogue")]
        public DialogueInfoSO startQuestDialogue;
        public DialogueInfoSO finishQuestDialogue;

        [Header("Settings")]
        [SerializeField] private bool startPoint = true;
        [SerializeField] private bool finishPoint = true;

        private bool playerIsNear = false;
        private string questID;
        private Quest currentQuest;
        private QuestState currentQuestState;
        private QuestIcon questIcon;

        private void Start()
        {
            EventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
            EventsManager.Instance.inputEvents.onSubmitPressed += SubmitPressed;
        }

        private void OnDisable()
        {
            EventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
            EventsManager.Instance.inputEvents.onSubmitPressed -= SubmitPressed;
        }
        
        private void Awake()
        {
            questID = questInfoForPoint.questID;
            questIcon = GetComponentInChildren<QuestIcon>();
        }

        private void QuestStateChange(Quest quest)
        {
            // Only update the quest state if this point has the corresponding quest
            if (quest.info.questID.Equals(questID))
            {
                currentQuest = quest;
                currentQuestState = quest.state;
                questIcon.SetState(currentQuestState, startPoint, finishPoint);

                Debug.Log("Quest with ID '" + questID + "' update to state '" + currentQuestState + "'");
            }
        }

        private void SubmitPressed()
        {
            if (!playerIsNear)
            {
                return;
            }

            if (startPoint)
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.StartDialogue(startQuestDialogue);
                }
                else if (currentQuestState.Equals(QuestState.CAN_START))
                {
                    EventsManager.Instance.questEvents.StartQuest(questID);
                }
            }
            else if (finishPoint)
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.StartDialogue(finishQuestDialogue);

                    if (currentQuest.info.questType == QuestInfoSO.QuestType.COLLECTING)
                    {
                        ItemInfoSO itemToCollect = currentQuest.info.questSteps[currentQuest.currentQuestStepIndex - 1].GetComponent<CollectQuestStep>().itemToCollect;
                        int amountToCollect = currentQuest.info.questSteps[currentQuest.currentQuestStepIndex - 1].GetComponent<CollectQuestStep>().amountToComplete;

                        InventoryManager.Instance.RemoveItem(itemToCollect, amountToCollect);
                    }
                }
                else if (currentQuestState.Equals(QuestState.CAN_FINISH))
                {
                    EventsManager.Instance.questEvents.FinishQuest(questID);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("RPGToolkitPlayer"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("RPGToolkitPlayer"))
            {
                playerIsNear = false;
            }
        }
    }
}
