using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class EventsManager : MonoBehaviour
    {
        public static EventsManager Instance { get; private set; }
        public InputEvents inputEvents;
        public PlayerEvents playerEvents;
        public QuestEvents questEvents;
        public CollectEvents collectEvents;

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

            inputEvents = new InputEvents();
            playerEvents = new PlayerEvents();
            questEvents = new QuestEvents();
            collectEvents = new CollectEvents();
        }
    }
}