using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGToolkit
{
    public class SkillRange : MonoBehaviour
    {
        public int layerToDetect;
        public GameObject[] allEnemies;
        public List<GameObject> inRangeEnemies;
        public List<GameObject> checkInRange;
        public GameObject closestEnemy;

        void Awake()
        {
            layerToDetect = LayerMask.NameToLayer("Enemy");

            allEnemies = FindGameObjectsInLayer(layerToDetect);
        }

        GameObject[] FindGameObjectsInLayer(int layer)
        {
            var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
            var goList = new System.Collections.Generic.List<GameObject>();
            
            for (int i = 0; i < goArray.Length; i++)
            {
                if (goArray[i].layer == layer)
                {
                    goList.Add(goArray[i]);
                }
            }

            if (goList.Count == 0)
            {
                return null;
            }

            return goList.ToArray();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
            {
                inRangeEnemies.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("EnemyShield"))
            {
                var enemyToRemove = new List<GameObject>();
                
                foreach (var enemy in inRangeEnemies)
                {
                    if (other.gameObject.name == enemy.name)
                    {
                        enemyToRemove.Add(enemy);
                    }
                }

                foreach (var enemy in enemyToRemove)
                {
                    inRangeEnemies.Remove(enemy);
                }

                enemyToRemove.Clear();
            }
        }

        public GameObject ClosestEnemy()
        {
            GameObject closestHere = null;
            float leastDist = Mathf.Infinity;

            foreach (var check in inRangeEnemies)
            {
                checkInRange.Add(check);
            }

            if (inRangeEnemies.Count > 0)
            {
                foreach (var enemy in checkInRange)
                {
                    float distHere = Vector2.Distance(transform.position, enemy.transform.position);

                    if (distHere < leastDist)
                    {
                        leastDist = distHere;
                        closestHere = enemy;
                    }
                }
            }

            checkInRange.Clear();
            return closestHere;
        }

        public void RemoveEnemy(GameObject enemyObject)
        {
            var enemyToRemove = new List<GameObject>();
                
            foreach (var enemy in inRangeEnemies)
            {
                if (enemyObject.name == enemy.name)
                {
                    enemyToRemove.Add(enemy);
                }
            }

            foreach (var enemy in enemyToRemove)
            {
                inRangeEnemies.Remove(enemy);
            }
        }
    }
}