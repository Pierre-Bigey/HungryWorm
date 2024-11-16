using System;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    [Header("Structure")]
    [SerializeField] private GameObject m_AirplaneModel;
    [SerializeField] private GameObject m_SmokeParticle;
    [SerializeField] private GameObject m_ThrustAnimation;
    private Rigidbody2D m_Rigidbody2D;
    
    [Header("Movement")]
    [SerializeField] private float atlitude = 5.35f;
    [SerializeField] private float m_turnAtDistance = 50f;
    [SerializeField] private float m_MaxSpeed = 10f;
    private bool movingRight = true;
    


    [Header("Bomb")]
    [SerializeField] private float m_BombReloadTime = 15;
    [SerializeField] private GameObject m_BombPrefab;
    private GameObject m_Bomb;
    [SerializeField] private Transform m_BombSpawnPoint;
    private bool bombReady = true;
    private float m_timeWhenBombDropped;
    
    [Header("Missile")]
    [SerializeField] private float m_distanceToFireMissile = 10;
    [SerializeField] private float m_MissileReloadTime = 5;
    [SerializeField] private GameObject m_MissilePrefab;
    private GameObject m_Missile;
    [SerializeField] private Transform m_MissileSpawnPoint;
    private bool missileReady = true;
    private float m_timeWhenMissileFired;

    private GameObject m_player;
    private PlayerController m_playerController;
    
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, atlitude, transform.position.z);
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_playerController = PlayerController.Instance;
        m_player = m_playerController.gameObject;
        
        m_Missile = Instantiate(m_MissilePrefab, m_MissileSpawnPoint);
        m_Bomb = Instantiate(m_BombPrefab, m_BombSpawnPoint);
    }
    
    private void FixedUpdate()
    {
        if (m_player == null)
        {
            Debug.LogError("Player not found in AirplaneController");
        }
        
        // Move the airplane
        Move();
        
        // Get the signed distance between the airplane and the player
        float distance = transform.position.x - m_player.transform.position.x;
        
        //Check if can fire missile
        if (missileReady)
        {
            CheckMissile(distance);
        }
        
        
        // Check if the airplane should turn
        if (Math.Abs(distance) > m_turnAtDistance && ( (distance > 0 && movingRight) || (distance < 0 && !movingRight) ) )
        {
            Turn();
        }

        Reload();

    }

    private void Reload()
    {
        if (!missileReady && Time.time - m_timeWhenMissileFired > m_MissileReloadTime)
        {
            missileReady = true;
            m_Missile = Instantiate(m_MissilePrefab, m_MissileSpawnPoint);
        }
        
        if (!bombReady && Time.time - m_timeWhenBombDropped > m_BombReloadTime)
        {
            bombReady = true;
            m_Bomb = Instantiate(m_BombPrefab, m_BombSpawnPoint);
        }
    }

    private void Move()
    {
        // Move the airplane
        m_Rigidbody2D.velocity = new Vector2(movingRight ? m_MaxSpeed : -m_MaxSpeed, 0);
    }

    private void Turn()
    {
        movingRight = !movingRight;
        
        // Reverse the rotation of the airplane
        transform.rotation = Quaternion.Euler(0, movingRight ? 0 : 180, 0);
        
        // Reverse the position of the missile
        var localPosition = m_MissileSpawnPoint.localPosition;
        m_MissileSpawnPoint.localPosition = new Vector3(localPosition.x, localPosition.y, movingRight ? -1 : 1);
        
        // Reverse the position of the bomb
        var position = m_BombSpawnPoint.localPosition;
        m_BombSpawnPoint.localPosition = new Vector3(position.x, position.y, movingRight ? 1 : -1);
        
        // Reverse the position of the ThrustAnimation
        var thrustPosition = m_ThrustAnimation.transform.localPosition;
        m_ThrustAnimation.transform.localPosition = new Vector3(thrustPosition.x, thrustPosition.y, movingRight ? 1 : -1);
        
    }
    
    private void CheckMissile(float distance)
    {
        // Check if can fire a missile on the player. If the distance is less than the distance to fire a missile and 
        // the player is not in dirt and the player is in front of the airplane, fire
        bool playerInFront = movingRight ? distance < 0 : distance > 0;
        if (!m_playerController.InDirt && playerInFront && Math.Abs(distance) < m_distanceToFireMissile)
        {
            FireMissile();
        }
        
    }



    private void FireMissile()
    {
        missileReady = false;
        
        // Anticipate where the player will be when the missile reaches him
        Rigidbody2D playerRigidbody = m_player.GetComponent<Rigidbody2D>();
        float timeToReachPlayer = Vector2.Distance(m_MissileSpawnPoint.position, m_player.transform.position) / (m_MaxSpeed * 2) - 0.2f;
        Vector3 target = m_player.transform.position + new Vector3(playerRigidbody.velocity.x * timeToReachPlayer, playerRigidbody.velocity.y * timeToReachPlayer, 0);
        
        m_timeWhenMissileFired = Time.time;
        
        Debug.Log("Firing missile");
        m_Missile.transform.SetParent(null);
        m_Missile.GetComponent<MissileController>().Fire(target);
    }
}
