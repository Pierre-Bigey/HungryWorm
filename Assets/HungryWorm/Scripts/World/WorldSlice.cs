
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

        private List<GameObject> m_Rocks;
        

        public void InitializeSlice(float x_pos)
        {
            
            m_XPos = x_pos;
            PlaceSlice();

            // Debug.Log(gameObject.name + " initialized");
            m_Rocks = SliceItemSpawner.Instance.GetRockForSlice(m_XPos);
            PlaceRocks();
        }
        
        private void PlaceSlice()
        {
            transform.position = new Vector3(m_XPos, 0, 0);
        }

        private void PlaceRocks()
        {
            //place the rocks in the container and set their position randomly
            foreach (var rock in m_Rocks)
            {
                //the container is the first child of the world slice
                rock.transform.parent = this.transform.GetChild(0);
                rock.transform.localPosition = new Vector3(Random.Range(-SliceWidth / 2, SliceWidth / 2),
                    Random.Range(- SliceHeight+3, -3), 0);
                rock.SetActive(true);
            }
        }

        public void DisableSlice()
        {
            // Debug.Log("Disabling " + gameObject.name);
            m_Rocks = new List<GameObject>();
            SliceItemSpawner.Instance.RemoveRocksFromSlice(m_XPos);
        }
    }
}