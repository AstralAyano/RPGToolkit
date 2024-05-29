using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerHitbox"))
        {
            if (gameObject.scene.name == "GameLevel1")
            {
                GameObject.FindWithTag("SceneManager").GetComponent<SceneLoadManager>().LoadScene("GameLevel2");
            }
        }
    }
}
