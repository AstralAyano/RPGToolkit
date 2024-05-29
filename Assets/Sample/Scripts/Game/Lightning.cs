using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Sprite angryCloud;

    private float timer = 0;

    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 0.2f)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = angryCloud;
        }
    }
}
