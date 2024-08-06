using UnityEngine;

namespace RPGToolkit
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class QuestPoint2D : MonoBehaviour
    {
        [Header("Quest")]
        public QuestInfoSO questInfoForPoint;

        [Header("Dialogue")]
        public DialogueInfoSO dialogueInfo;

        [Header("Settings")]
        [SerializeField] private bool startPoint = true;
        [SerializeField] private bool finishPoint = true;

        private bool playerIsNear = false;
        private string questID;
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

            if (DialogueManager.Instance != null && dialogueInfo != null)
            {
                DialogueManager.Instance.StartDialogue(dialogueInfo);
            }

            // Start or Finish a quest
            // if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            // {
            //     EventsManager.instance.questEvents.StartQuest(questID);
            // }
            // else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            // {
            //     EventsManager.instance.questEvents.FinishQuest(questID);
            // }
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
