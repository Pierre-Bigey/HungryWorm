using System;
using UnityEngine;

namespace HungryWorm
{
    public class WormBodyPart : MonoBehaviour
    {
        private Rigidbody2D m_Rigidbody2D;
        private Collider2D m_Collider2D;

        private void Start()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            m_Collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            WormEvents.WormDied += WormEvents_WormDied;
        }
        
        private void OnDisable()
        {
            WormEvents.WormDied -= WormEvents_WormDied;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // IF it's dirt, stop simulating the rigidbody
            if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
            {
                m_Rigidbody2D.simulated = false;
            }
        }


        public void WormEvents_WormDied()
        {
            m_Rigidbody2D.simulated = true;
            
            // Add a random upward force
            Vector3 force = Vector3.up * UnityEngine.Random.Range(2f, 3f) + Vector3.right * UnityEngine.Random.Range(-1f, 1f);
            m_Rigidbody2D.AddForce(force*10f, ForceMode2D.Impulse);
        }
    }
}