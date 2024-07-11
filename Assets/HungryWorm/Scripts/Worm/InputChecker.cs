using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InputChecker : MonoBehaviour
{
    [SerializeField] private float speed;
    
    [SerializeField] public float m_HorizontalValue;
    [SerializeField] public float m_VerticalValue;

    [SerializeField] private FloatingJoystick joystick;
    
    [SerializeField] private Rigidbody2D m_Rigidbody;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void FixedUpdate()
    {
        m_VerticalValue = joystick.Vertical;
        m_HorizontalValue = joystick.Horizontal;

        Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
        m_Rigidbody.AddForce(direction * (speed * Time.fixedDeltaTime), ForceMode2D.Impulse);
    }
}
