using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    public class AnimationManager: MonoBehaviour
    {
        
        [SerializeField] private GameObject m_ExplosionPrefab;
        [SerializeField] private int explosionPoolSize = 3;
        private List<GameObject> explosionPool = new List<GameObject>();

        private void Start()
        {
            InstantiatePools();
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void SubscribeEvents()
        {
            WormEvents.MineExploded += WormEvents_MineExploded;
        }

        private void UnsubscribeEvents()
        {
            WormEvents.MineExploded -= WormEvents_MineExploded;
        }

        private void InstantiatePools()
        {
            // Instantiate explosion pool
            explosionPool = new List<GameObject>();
            for (int i = 0; i < explosionPoolSize; i++)
            {
                GameObject explosion = Instantiate(m_ExplosionPrefab, transform);
                explosion.SetActive(false);
                explosionPool.Add(explosion);
            }
            
        }
        
        private void WormEvents_MineExploded(Vector3 position)
        {
            foreach (GameObject explosion in explosionPool)
            {
                if (!explosion.activeInHierarchy)
                {
                    explosion.transform.position = position;
                    explosion.SetActive(true);
                    return;
                }
            }
            //If no explosion is available, instantiate a new one
            GameObject newExplosion = Instantiate(m_ExplosionPrefab, transform);
            newExplosion.transform.position = position;
            newExplosion.SetActive(true);
            explosionPool.Add(newExplosion);
        }
        
        
        
    }
}