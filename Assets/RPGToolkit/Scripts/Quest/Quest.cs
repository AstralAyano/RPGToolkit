using System.Collections;
using System.Collections.Generic;
using RPGToolkit;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public enum QuestState
    {
        REQUIREMENTS_NOT_MET,
        CAN_START,
        IN_PROGRESS,
        CAN_FINISH,
        FINISHED
    }

    public QuestInfoSO info;
    public QuestState state;
    public int currentQuestStepIndex;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
