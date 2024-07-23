using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace  HungryWorm
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private VariableJoystick joystick;
        
        private bool _canMove = true;
        private void OnEnable()
        {
            GameEvents.GameStarted += AllowMovement;
            GameEvents.GameEnded += StopMovement;
            GameEvents.GamePaused += StopMovement;
            GameEvents.GameUnpaused += AllowMovement;
        }

        private void OnDisable()
        {
            GameEvents.GameStarted -= AllowMovement;
            GameEvents.GameEnded -= StopMovement;
            GameEvents.GamePaused -= StopMovement;
            GameEvents.GameUnpaused -= AllowMovement;
        }
        
        private void AllowMovement()
        {
            _canMove = true;
        }
        
        private void StopMovement()
        {
            _canMove = false;
        }

        private void FixedUpdate()
        {
            if (!_canMove)
            {
                return;
            }
            Vector2 direction = Vector2.up * joystick.Vertical + Vector2.right * joystick.Horizontal;
            WormEvents.WormGoToDirection?.Invoke(direction);
        }
    }
}