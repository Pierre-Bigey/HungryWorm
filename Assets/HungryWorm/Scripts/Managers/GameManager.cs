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
            SubscribeEvents();
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void Initialize()
        {
            NullRefChecker.Validate(this);
            
            UIEvents.GameScreenShown?.Invoke();
            GameEvents.GameStarted?.Invoke();
        }
        
        private void SubscribeEvents()
        {
            WormEvents.WormDied += WormEvents_OnWormDied;
        }
        
        private void UnsubscribeEvents()
        {
            WormEvents.WormDied -= WormEvents_OnWormDied;
        }

        private void WormEvents_OnWormDied()
        {
            GameEvents.GameEnded?.Invoke();
            UIEvents.EndScreenShown?.Invoke();
        }

    }
}