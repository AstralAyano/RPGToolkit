using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class EventsManager : MonoBehaviour
    {
        public static EventsManager instance;
        public InputEvents inputEvents;
        public QuestEvents questEvents;
        public CollectEvents collectEvents;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            inputEvents = new InputEvents();
            questEvents = new QuestEvents();
            collectEvents = new CollectEvents();
        }
    }
}