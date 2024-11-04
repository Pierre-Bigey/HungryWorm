using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    public class AnimationManager: MonoBehaviour
    {
        [Header("Explosion")]
        [SerializeField] private GameObject m_ExplosionPrefab;
        [SerializeField] private int explosionPoolSize = 3;
        private List<GameObject> explosionPool = new List<GameObject>();
        
        [Header("Blood")]
        [SerializeField] private GameObject m_BloodPrefab;
        [SerializeField] private int bloodSize = 5;
        private List<GameObject> bloodPool = new List<GameObject>();

        [Header("General")] 
        [SerializeField] private float m_animationZpos = 1; 

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
            WormEvents.BloodSplatter += WormEvents_BloodSplatter;
        }

        private void UnsubscribeEvents()
        {
            WormEvents.MineExploded -= WormEvents_MineExploded;
            WormEvents.BloodSplatter -= WormEvents_BloodSplatter;
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
            
            // Instantiate blood pool
            bloodPool = new List<GameObject>();
            for (int i = 0; i < bloodSize; i++)
            {
                GameObject blood = Instantiate(m_BloodPrefab, transform);
                blood.SetActive(false);
                bloodPool.Add(blood);
            }
            
        }
        
        private void WormEvents_MineExploded(Vector3 position)
        {
            foreach (GameObject explosion in explosionPool)
            {
                if (!explosion.activeInHierarchy)
                {
                    explosion.transform.position = new Vector3(position.x, position.y, m_animationZpos);
                    explosion.SetActive(true);
                    return;
                }
            }
            //If no explosion is available, instantiate a new one
            GameObject newExplosion = Instantiate(m_ExplosionPrefab, transform);
            newExplosion.transform.position = new Vector3(position.x, position.y, m_animationZpos);
            newExplosion.SetActive(true);
            explosionPool.Add(newExplosion);
        }
        
        private void WormEvents_BloodSplatter(Vector3 position)
        {
            foreach (GameObject blood in bloodPool)
            {
                if (!blood.activeInHierarchy)
                {
                    blood.transform.position = new Vector3(position.x, position.y, m_animationZpos);
                    blood.SetActive(true);
                    return;
                }
            }
            //If no blood is available, instantiate a new one
            GameObject newBlood = Instantiate(m_BloodPrefab, transform);
            newBlood.transform.position = new Vector3(position.x, position.y, m_animationZpos);
            newBlood.SetActive(true);
            bloodPool.Add(newBlood);
        }
        
        
        
    }
}