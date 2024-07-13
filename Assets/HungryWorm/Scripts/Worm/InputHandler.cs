using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace  HungryWorm
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private VariableJoystick joystick;
        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void FixedUpdate()
        {
            Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            WormEvents.WormGoToDirection?.Invoke(direction);
        }
    }
}