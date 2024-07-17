using System;
using UnityEngine;

namespace HungryWorm.Items
{
    public class MineItem: SliceItem
    {

        [SerializeField] private float m_MineDamage = 10f;
        
        public MineItem()
        {
            m_SliceItemType = SliceItemType.MINE;
            m_minScale = 1f;
            m_maxScale = 1f;
            m_XMin = - WorldSlice.SliceWidth / 2 + 0.5f;
            m_XMax = WorldSlice.SliceWidth / 2 - 0.5f;
            m_YMin = 0;
            m_YMax = 0;
            m_randomRotation = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Mine triggered with " + other.gameObject.name);
            //Check the layer
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                WormEvents.MineExploded?.Invoke(transform.position);
                WormEvents.DamageTaken?.Invoke(m_MineDamage);
                gameObject.SetActive(false);
            }
        }
    }
}