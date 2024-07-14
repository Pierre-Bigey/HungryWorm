using System;
using Unity.VisualScripting;
using UnityEngine;

namespace HungryWorm.Trail
{
    [RequireComponent(typeof(PlayerController))]
    public class TrailManager : MonoBehaviour
    {
        
        private ObjectPooler m_objectPooler;

        [SerializeField] private int m_poolSize = 20;
    
        [SerializeField] private GameObject m_trailPrefab;
        private GameObject m_trailParent;
        [SerializeField] private float m_trailSeparation = 0.4f;
    
        [SerializeField] private float m_ZPosition = 0.1f;
        
        private Vector3 m_lastTrailPosition;
        
        private PlayerController m_playerController;
        
        
        private void Start()
        {
            m_playerController = GetComponent<PlayerController>();
            
            //Create an empty gameobject name "TrailParent" to hold all the trail objects
            m_trailParent = new GameObject("TrailParent");
         
            m_objectPooler = new ObjectPooler(m_trailPrefab, m_trailParent, m_poolSize);
            m_objectPooler.InitializePool();
            
            m_lastTrailPosition = transform.position;
        }

        private void OnDestroy()
        {
            m_objectPooler.ClearPool();
            GameObject.Destroy(m_trailParent);
        }

        private void LateUpdate()
        {
            //Check if player is in dirt
            if (!m_playerController.InDirt) return;
            
            //Check if the distance between the last trail object and the player is greater than the separation distance
            if (Vector3.Distance(m_lastTrailPosition, transform.position) > m_trailSeparation)
            {
                //Get a trail object from the pool
                GameObject trail = m_objectPooler.GetPooledObject();
                //Set the trail object position to the player position
                var currentPosition = transform.position;
                trail.transform.position = new Vector3(currentPosition.x, currentPosition.y, m_ZPosition);
                //Set the last trail position to the player position
                m_lastTrailPosition = currentPosition;
                //Set the trail object active
                trail.SetActive(true);
            }
        }
        
        
    }
}