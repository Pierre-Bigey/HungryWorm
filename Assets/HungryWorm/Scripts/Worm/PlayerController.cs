using System;
using HungryWorm;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private float m_health;
    private bool m_isInDirt;
    
    public float Health => m_health;
    public bool InDirt => m_isInDirt;
    
    private Rigidbody2D m_rigidbody2D;
    
    [FormerlySerializedAs("m_baseHealth")]
    [Header("Worm Health")] 
    [SerializeField] private float m_maxHealth = 50f;
    [SerializeField] private float m_healthLooseRate = 3f;
    
    
    //Values in the air
    [Header("Air rigidbody values")]
    [SerializeField] private float m_airLinearDrag = 0.1f;
    [SerializeField] private float m_airAngularDrag = 0.1f;
    [SerializeField] private float m_airGravityScale = 0.9f;
    [SerializeField] private float m_airSpeed = 5f;
    [SerializeField] private float m_airHorizontalMultiplier = 2f;
    
    [Header("Dirt rigidbody values")]
    [SerializeField] private float m_dirtLinearDrag = 1.5f;
    [SerializeField] private float m_dirtAngularDrag = 2f;
    [SerializeField] private float m_dirtGravityScale = 0f;
    [SerializeField] private float m_dirtSpeed = 10f;

    private float m_speed;

    private bool m_supperposedDirt;

    private void Start()
    {
        
        InitializeValues();

    }

    private void InitializeValues()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        
         m_isInDirt = false;
         SetWormMovementValues();
                
         m_health = m_maxHealth;

         m_supperposedDirt = false;
    }
    
    private void OnEnable()
    {
        SubscribeEvents();
    }
    
    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void LateUpdate()
    {
        transform.right = m_rigidbody2D.velocity.normalized;
    }

    private void SubscribeEvents()
    {
        WormEvents.DamageTaken += GameEvents_DamageTaken;
        WormEvents.EnemyEaten += GameEvents_EnemyEaten;
        
        WormEvents.WormGoToDirection += MoveTo;
        
        GameEvents.TimeUpdated += GameEvents_TimeUpdated;
    }
    
    private void UnsubscribeEvents()
    {
        WormEvents.DamageTaken -= GameEvents_DamageTaken;
        WormEvents.EnemyEaten -= GameEvents_EnemyEaten;
        
        WormEvents.WormGoToDirection -= MoveTo;
        
        GameEvents.TimeUpdated -= GameEvents_TimeUpdated;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the player collided with the dirt (with layer name "Dirt")
        if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
        {
            if(m_isInDirt)
            {
                m_supperposedDirt = true;
                return;
            }
            WormEvents.DirtEnter?.Invoke();
            OnDirtEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the player exited the dirt (with layer name "Dirt")
        if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
        {
            if (m_supperposedDirt)
            {
                m_supperposedDirt = false;
                return;
            }
            WormEvents.DirtExit?.Invoke();
            OnDirtExit();
        }
    }

    private void OnDirtEnter()
    {
        m_isInDirt = true;
        SetWormMovementValues();
    }
    
    private void OnDirtExit()
    {
        m_isInDirt = false;
        SetWormMovementValues();

    }
    
    private void MoveTo(Vector2 direction)
    {
        Vector2 force = direction * (m_speed * Time.deltaTime);
        if (!m_isInDirt)
        {
            force.x *= m_airHorizontalMultiplier;
        }
        
        m_rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
    
    private void SetWormMovementValues()
    {
        if (!m_isInDirt)
        {
            m_rigidbody2D.drag = m_airLinearDrag;
            m_rigidbody2D.angularDrag = m_airAngularDrag;
            m_rigidbody2D.gravityScale = m_airGravityScale;

            m_speed = m_airSpeed;
        }
        else
        {
            m_rigidbody2D.drag = m_dirtLinearDrag;
            m_rigidbody2D.angularDrag = m_dirtAngularDrag;
            m_rigidbody2D.gravityScale = m_dirtGravityScale;
            
            m_speed = m_dirtSpeed;
        }
    }
    
    private void GameEvents_TimeUpdated(float deltaTime)
    {
        UpdateHealth(-m_healthLooseRate * deltaTime);
    }

    private void UpdateHealth(float amount)
    {
        m_health += amount;
        m_health = Mathf.Clamp(m_health, 0, m_maxHealth);

        UIEvents.HealthBarUpdated?.Invoke(m_health / m_maxHealth);

        if (m_health <= 0)
        {
            Die();
        }
    
    }

    private void Die()
    {
        //TODO make an animation or something
        // Debug.Log("Player died");
        WormEvents.WormDied?.Invoke();
    }
    
    private void GameEvents_EnemyEaten(float amount)
    {
        m_health += amount;
    }
    
    private void GameEvents_DamageTaken(float damage)
    {
        m_health -= damage;
    }

}
