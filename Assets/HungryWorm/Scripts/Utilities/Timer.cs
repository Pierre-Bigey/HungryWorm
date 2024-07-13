using System;
using System.Collections;
using UnityEngine;

namespace HungryWorm
{
    public class Timer
    {
        private float sectionCurrentTime;
        
        private bool isTimerGoing;
 
        private Coroutine coUpdateTimer;

        
        
        public void BeginTimer()
        {
            isTimerGoing = false;
            if (coUpdateTimer != null) {
                Coroutines.StopCoroutine(coUpdateTimer);
            }
 
            sectionCurrentTime = 0f;
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
                sectionCurrentTime += Time.deltaTime;
                GameEvents.TimeUpdated.Invoke(sectionCurrentTime);
                
                yield return null;
            }
        }
    }
}