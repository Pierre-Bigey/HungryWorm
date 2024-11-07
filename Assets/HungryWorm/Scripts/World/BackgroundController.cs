using System;
using UnityEngine;

namespace HungryWorm
{
    public class BackgroundController : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            transform.parent = Camera.main.transform;
        }

        private void OnEnable()
        {
            UIEvents.LeaderboardScreenShown += UIEvents_LeaderboardScreenShown;
        }
        
        private void OnDisable()
        {
            UIEvents.LeaderboardScreenShown -= UIEvents_LeaderboardScreenShown;
        }

        private void UIEvents_LeaderboardScreenShown()
        {
            Destroy(this.gameObject);
        }
    }
}