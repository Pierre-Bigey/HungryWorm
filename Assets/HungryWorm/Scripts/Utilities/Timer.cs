using System;
using System.Collections;
using UnityEngine;

namespace HungryWorm
{
    public class Timer
    {
        
        private bool isTimerGoing;
 
        private Coroutine coUpdateTimer;

        
        
        public void BeginTimer()
        {
            isTimerGoing = false;
            if (coUpdateTimer != null) {
                Coroutines.StopCoroutine(coUpdateTimer);
            }
            isTimerGoing = true;
            coUpdateTimer = Coroutines.StartCoroutine(UpdateTimer());
        }
        
        public void StopTimer()
        {
            isTimerGoing = false;
            if (coUpdateTimer != null) {
                Coroutines.StopCoroutine(coUpdateTimer);
            }
        }
        
        private IEnumerator UpdateTimer()
        {
            while (isTimerGoing)
            {
                GameEvents.TimeUpdated.Invoke(Time.deltaTime);
                
                yield return null;
            }
        }
    }
}