
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HungryWorm
{
    public class WorldSlice: MonoBehaviour
    {
        
        public static float SliceWidth = 20f;
        public static float SliceHeight = 20f;

        private float m_XPos;

        

        public void InitializeSlice(float x_pos)
        {
            m_XPos = x_pos;
            PlaceSlice();

            // Debug.Log(gameObject.name + " initialized");
            GetItems();
        }
        
        private void PlaceSlice()
        {
            transform.position = new Vector3(m_XPos, 0, 0);
        }

        private void GetItems()
        {
            Transform container = transform.GetChild(0);
            List<GameObject> items = SliceItemManager.Instance.GetItemsForSlice(m_XPos);
            foreach (var item in items)
            {
                item.transform.parent = container.transform;
                item.SetActive(true);
                item.GetComponent<SliceItem>().Init();
            }
        }
        

        public void DisableSlice()
        {
            Debug.Log("Disabling " + gameObject.name);
            SliceItemManager.Instance.RemoveItemsFromSlice(m_XPos);
        }
    }
}