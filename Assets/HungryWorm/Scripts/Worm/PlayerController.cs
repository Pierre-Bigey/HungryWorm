using System;
using HungryWorm;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private float m_health;
    private bool m_isInDirt;
    
    public float Health => m_health;
    public bool InDirt => m_isInDirt;
    
    private Rigidbody2D m_rigidbody2D;
    
    
    //Values in the air
    [Header("Air rigidbody values")]
    [SerializeField] private float m_airLinearDrag = 0.1f;
    [SerializeField] private float m_airAngularDrag = 0.1f;
    [SerializeField] private float m_airGravityScale = 0.9f;
    [SerializeField] private float m_airSpeed = 5f;
    
    [Header("Dirt rigidbody values")]
    [SerializeField] private float m_dirtLinearDrag = 1.5f;
    [SerializeField] private float m_dirtAngularDrag = 2f;
    [SerializeField] private float m_dirtGravityScale = 0f;
    [SerializeField] private float m_dirtSpeed = 10f;

    private float m_speed;

    private void Start()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        
        m_isInDirt = false;
        SetWormMovementValues(false);
        
        m_health = 100;
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
        WormEvents.DamageTaken += GameEvents_DamageTaken;
        WormEvents.EnemyEaten += GameEvents_EnemyEaten;
        
        WormEvents.WormGoToDirection += MoveTo;
    }
    
    private void UnsubscribeEvents()
    {
        WormEvents.DamageTaken -= GameEvents_DamageTaken;
        WormEvents.EnemyEaten -= GameEvents_EnemyEaten;
        
        WormEvents.WormGoToDirection -= MoveTo;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Check if the player collided with the dirt (with layer name "Dirt")
        if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
        {
            WormEvents.DirtEnter?.Invoke();
            OnDirtEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Check if the player exited the dirt (with layer name "Dirt")
        if (other.gameObject.layer == LayerMask.NameToLayer("Dirt"))
        {
            WormEvents.DirtExit?.Invoke();
            OnDirtExit();
        }
    }

    private void OnDirtEnter()
    {
        m_isInDirt = true;
        SetWormMovementValues(true);
    }
    
    private void OnDirtExit()
    {
        m_isInDirt = false;
        SetWormMovementValues(false);

    }
    
    private void MoveTo(Vector2 direction)
    {
        Vector2 force = direction * (m_speed * Time.deltaTime);
        m_rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }
    
    private void SetWormMovementValues(bool isInDirt)
    {
        if (!isInDirt)
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

    
    
    private void GameEvents_EnemyEaten(float amount)
    {
        m_health += amount;
    }
    
    private void GameEvents_DamageTaken(float damage)
    {
        m_health -= damage;
    }

}