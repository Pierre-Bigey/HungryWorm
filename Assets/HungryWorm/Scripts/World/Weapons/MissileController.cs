using System;
using System.Collections;
using HungryWorm;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    [SerializeField] private GameObject m_SmokeParticle;
    [SerializeField] private GameObject m_ThrustAnimation;
    
    [SerializeField] private float m_missileDamage = 20f;
    
    private Rigidbody2D m_Rigidbody2D;
    private SpriteRenderer m_SpriteRenderer;
    
    [SerializeField] private float force = 10f;

    private bool m_engineOn = false;
    private bool m_exploded = false;
    private Vector3 direction;
    
    private Vector3 m_target;
    
    
    private void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Fire(Vector3 target)
    {
        m_target = target;
        
        m_Rigidbody2D.simulated = true;
        m_SmokeParticle.SetActive(true);
        m_ThrustAnimation.SetActive(true);
        
        direction = target - transform.position;
        direction.z = 0;
        direction.Normalize();
        
        // Turn the missile toward the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        m_engineOn = true;
    }
    
    private void FixedUpdate()
    {
        if (m_engineOn)
        {
            m_Rigidbody2D.AddForce(direction * (force * Time.fixedDeltaTime));
            // If distance to target is more than 1000, destroy the missile
            if (Vector3.Distance(transform.position, m_target) > 1000)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Missile hit: " + other.name);
        if (m_exploded)
        {
            return;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            WormEvents.DamageTaken?.Invoke(m_missileDamage);
            Explose();
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
        {
            Explose();
        }
    }

    private void Explose()
    {
        Debug.Log("Missile exploded");
        
        m_engineOn = false;
        m_exploded = true;
        
        AudioManager.Instance.PlayMissileExplosionSound();
        WormEvents.MineExploded?.Invoke(transform.position);
        
        m_ThrustAnimation.SetActive(false);
        m_SpriteRenderer.enabled = false;
        m_Rigidbody2D.simulated = false;
        
        StartCoroutine(DestroyAfter(2));
    }
    
    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
