using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm.Scripts.Food
{
    public class Human : Edible
    {
        [SerializeField] private List<GameObject> HumanModels;

        [SerializeField] private float m_distanceToSeeWorm = 10;
        [SerializeField] private float m_timeToCalm = 5;
        private bool sawTheWorm;
        
        private GameObject m_model;
        private Animator m_AnimatorController;
        
        private PlayerController m_playerController;
        
        private float m_timeWhenSawTheWorm;

        private bool m_FleeRight;

        private float m_speed;

        private void OnEnable()
        {
            sawTheWorm = false;
            
            // Clean up the previous model
            if (m_model != null)
            {
                Destroy(m_model);
            }

            GameObject model = HumanModels[UnityEngine.Random.Range(0, HumanModels.Count)];
            m_model = Instantiate(model, transform);

            // The first child must be the model and must contain the animator
            m_AnimatorController = m_model.GetComponentInChildren<Animator>();

            m_playerController = PlayerController.Instance;
            
            m_speed = UnityEngine.Random.Range(2.5f, 4.5f);
            m_AnimatorController.speed = 1;
        }

        private void Update()
        {
            CheckIfSeeWorm();
            
            if (sawTheWorm)
            {
                Flee();
                if(Time.time - m_timeWhenSawTheWorm > m_timeToCalm)
                {
                    sawTheWorm = false;
                    m_AnimatorController.SetBool("Walk", false);
                    m_AnimatorController.speed = 1;
                }
            }
        }

        private void Flee()
        {
            Vector2 direction = m_FleeRight ? Vector2.right : Vector2.left;
            transform.Translate(direction * m_speed * Time.deltaTime);
        }
        
        private void CheckIfSeeWorm()
        {
            if (m_playerController == null)
            {
                m_playerController = PlayerController.Instance;
            }

            if (m_playerController.InDirt)
            {
                return;
            }

            Vector2 playerPosition = m_playerController.transform.position;
            Vector2 humanPosition = transform.position;

            if (Vector2.Distance(playerPosition, humanPosition) < m_distanceToSeeWorm)
            {
                //Check if the worm is on the right or left
                m_FleeRight = playerPosition.x <= humanPosition.x;
                
                //Turn the human against the worm
                float angle = m_FleeRight ? 0 : 180;
                m_model.transform.localRotation = Quaternion.Euler(0, angle, 0);
                
                sawTheWorm = true;
                m_AnimatorController.speed = m_speed;
                m_AnimatorController.SetBool("Walk", true);
                m_timeWhenSawTheWorm = Time.time;
            }
        }
    }
}