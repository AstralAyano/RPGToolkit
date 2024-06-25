using UnityEngine;

namespace RPGToolkit
{
    [RequireComponent(typeof(SphereCollider))]
    public class QuestPoint3D : MonoBehaviour
    {
        [Header("Quest")]
        [SerializeField] private QuestInfoSO questInfoForPoint;

        [Header("Settings")]
        [SerializeField] private bool startPoint = true;
        [SerializeField] private bool finishPoint = true;

        private bool playerIsNear = false;
        private string questID;
        private QuestState currentQuestState;

        private void OnEnable()
        {
            EventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
            EventsManager.instance.inputEvents.onSubmitPressed += SubmitPressed;
        }

        private void OnDisable()
        {
            EventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
            EventsManager.instance.inputEvents.onSubmitPressed -= SubmitPressed;
        }
        
        private void Awake()
        {
            questID = questInfoForPoint.questID;
        }

        private void QuestStateChange(Quest quest)
        {
            // Only update the quest state if this point has the corresponding quest
            if (quest.info.questID.Equals(questID))
            {
                currentQuestState = quest.state;
                //questIcon.SetState(currentQuestState, startPoint, finishPoint);

                Debug.Log("Quest with ID '" + questID + "' update to state '" + currentQuestState + "'");
            }
        }

        private void SubmitPressed()
        {
            if (!playerIsNear)
            {
                return;
            }

            // Start or Finish a quest
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                EventsManager.instance.questEvents.StartQuest(questID);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                EventsManager.instance.questEvents.FinishQuest(questID);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("RPGToolkitPlayer"))
            {
                playerIsNear = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("RPGToolkitPlayer"))
            {
                playerIsNear = false;
            }
        }
    }
}