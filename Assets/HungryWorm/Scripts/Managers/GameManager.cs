using System;
using Unity.VisualScripting;
using UnityEngine;

namespace HungryWorm
{
    public class GameManager : MonoBehaviour
    {
        
        private void OnEnable()
        {
            Initialize();
            UIEvents.GameScreenShown?.Invoke();
            GameEvents.GameStarted?.Invoke();
        }
        
        private void OnDisable()
        {
            
        }
        
        private void Initialize()
        {
            NullRefChecker.Validate(this);
        }
        
        
        
    }
}