using System.Collections;
using HungryWorm.Scripts.Food;
using UnityEngine;

namespace HungryWorm
{
    public class BombController : MonoBehaviour
    {

        [SerializeField] private float m_BombDamage = 30f;
        [SerializeField] private float m_explosionRadius = 2f;
        
        [SerializeField] private GameObject m_ExplosionEffect;
        
        private bool m_Dropped = false;
        private bool m_Exploded = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (m_Exploded || !m_Dropped)
            {
                return;
            }
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Dirt") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Explose();
            }
            
        }

        private void Explose()
        {
            Debug.Log("Bomb exploded");
            m_ExplosionEffect.SetActive(true);
            m_Exploded = true;
            
            GetComponent<Rigidbody2D>().simulated = false;
            
            // Check if player or human are in the range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, m_explosionRadius);
            foreach (Collider2D hit in colliders)
            {
                //Check if the object's layer is Player or Human
                if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    WormEvents.DamageTaken?.Invoke(m_BombDamage);
                }
                else if (hit.gameObject.layer == LayerMask.NameToLayer("Human"))
                {
                    Human human = hit.GetComponent<Human>();
                    human.Kill();
                }
            }
            GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(DestroyAfter(1.8f));
        }

        public void Drop(Vector3 force)
        {
            m_Dropped = true;
            GetComponent<CapsuleCollider2D>().enabled = true;
            GetComponent<Rigidbody2D>().simulated = true;
            
            // Add a force to the bomb to mimic the inertia from the airplane 
            GetComponent<Rigidbody2D>().AddForce(force*20, ForceMode2D.Impulse);
        }
        
        private IEnumerator DestroyAfter(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}