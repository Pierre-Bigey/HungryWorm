using System;
using UnityEngine;

namespace HungryWorm.Scripts.Food
{
    public abstract class Edible : MonoBehaviour
    {
        [Tooltip("The food value of this food item.")]
        [SerializeField] private float m_foodValue;
        [Tooltip("The score value of this food item.")]
        [SerializeField] private float m_scoreValue;


        /// <summary>
        /// Check if collide with the player, if yes trigger the events and destroy the object.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Debug.Log("Edible triggered with " + other.gameObject.name);
            //Check the layer
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Eat();
                Kill();
            }
        }

        private void Eat()
        {
            WormEvents.EnemyEaten?.Invoke(m_foodValue);
            GameEvents.ScoreUpdated?.Invoke(m_scoreValue);
        }

        /// <summary>
        /// Called when eaten by the player.
        /// </summary>
        private void Kill()
        {
            gameObject.SetActive(false);
            WormEvents.BloodSplatter?.Invoke(transform.position);
        }
    }
}