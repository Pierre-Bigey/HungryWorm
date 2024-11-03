using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace HungryWorm.Scripts.Food
{
    public class Human: Edible
    {
        [SerializeField] private List<GameObject> HumanModels; 
        
        private Animator m_AnimatorController;

        private bool sawTheWorm = false;
        
        private GameObject m_model;
        
        private void Start()
        {
            GameObject model = HumanModels[UnityEngine.Random.Range(0, HumanModels.Count)];
            m_model = Instantiate(model, transform);

            // The first child must be the model and must contain the animator
            m_AnimatorController = m_model.GetComponentInChildren<Animator>();
            m_AnimatorController.SetBool("Walk", true);
        }
    }
}