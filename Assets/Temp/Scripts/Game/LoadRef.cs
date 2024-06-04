using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LoadRef : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("ScreenSpaceUI").GetComponent<Canvas>().worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject.Find("Cinemachine Camera").GetComponent<CinemachineVirtualCamera>().m_Follow = GameObject.Find("Player").transform;
        GameObject.Find("Player").transform.position = new Vector3(-16.25f, -3.75f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
