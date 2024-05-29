using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed;
    Vector2 targetPos;

    private int targetIndex;
    
    void Start()
    {
        targetIndex = 0;
        targetPos = waypoints[targetIndex].transform.position;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[targetIndex].position) < 0.5f)
        {
            targetIndex++;

            if (targetIndex > waypoints.Count - 1)
            {
                targetIndex = 0;
            }

            targetPos = waypoints[targetIndex].transform.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);
            other.GetComponent<PlayerControllerA>().PlayerState = "Land";
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
