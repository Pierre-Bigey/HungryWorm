using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm.Scripts.Animation
{
    public class AnimationController: MonoBehaviour
    {
        private Animator _animator;

        private float lifetime;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // Debug.Log("Animator state info length: " + stateInfo.length + " speed: " + stateInfo.speed);
            lifetime = stateInfo.length * stateInfo.speed;
        }

        private void OnEnable()
        {
            StartCoroutine(WaitAndDisable());
        }
        
        private IEnumerator WaitAndDisable()
        {
            yield return new WaitForSeconds(lifetime);
            // Debug.Log("Disable animation");
            gameObject.SetActive(false);
        }
        
        
    }
}