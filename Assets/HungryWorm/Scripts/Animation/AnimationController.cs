using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm.Scripts.Animation
{
    public class AnimationController: MonoBehaviour
    {

        [SerializeField] private int animation_number = 1;
        
        private Animator _animator;
        private float lifetime;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            
        }

        private void OnEnable()
        {
            StartCoroutine(WaitAndDisable());
            int animation = UnityEngine.Random.Range(1, animation_number+1);
            _animator.SetInteger("Animation", animation);
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            // Debug.Log("Animator state "+stateInfo.shortNameHash+" length: " + stateInfo.length + " speed: " + stateInfo.speed);
            lifetime = stateInfo.length * stateInfo.speed;
        }
        
        private IEnumerator WaitAndDisable()
        {
            yield return new WaitForSeconds(lifetime);
            // Debug.Log("Disable animation");
            _animator.SetInteger("Animation", 0);
            gameObject.SetActive(false);
        }
        
        
    }
}