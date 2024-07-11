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