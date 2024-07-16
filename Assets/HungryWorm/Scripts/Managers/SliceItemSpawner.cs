using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    /// <summary>
    /// Class to pool food objects, then a world slice can call it to get a food object
    /// </summary>
    public class SliceItemSpawner: MonoBehaviour
    {
        //Singleton Pattern
        public static SliceItemSpawner Instance { get; private set; }
        
        [Header("Rocks")]
        [SerializeField] private GameObject m_RockPrefab;
        [SerializeField] private float m_RockMinScale = 1.5f;
        [SerializeField] private float m_RockMaxScale = 3f;
        
        [SerializeField] private int m_InitialRockPoolSize = 15;
        
        [Header("Humans")]
        [SerializeField] private GameObject m_HumanPrefab;

        private List<GameObject> m_UnusedRockPool;
        private Dictionary<float, List<GameObject>> RockPerSliceDict;
        private List<GameObject> m_HumanPool;
        
        private float m_worldSliceWidth;
        private float m_worldSliceHeight;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            m_worldSliceWidth = WorldSlice.SliceWidth;
            m_worldSliceHeight = WorldSlice.SliceHeight;
            
            m_UnusedRockPool = new List<GameObject>();
            RockPerSliceDict = new Dictionary<float, List<GameObject>>();
            InstantiateRocks();
        }
        
        private void InstantiateRocks()
        {
            for (int i = 0; i < m_InitialRockPoolSize; i++)
            {
                GameObject obj = GameObject.Instantiate(m_RockPrefab, transform);
                obj.SetActive(false);
                m_UnusedRockPool.Add(obj);
            }
        }
        
        public List<GameObject> GetRockForSlice(float x_pos)
        {
            // Debug.Log("Instantiating rocks for slice at " + x_pos);
            //Get a random number of rocks to get from the unused pool
            int randomRockCount = UnityEngine.Random.Range(2, 4);
            List<GameObject> rocks = new List<GameObject>();
            for (int i = 0; i < randomRockCount; i++)
            {
                if(m_UnusedRockPool.Count == 0)
                {
                    GameObject obj = GameObject.Instantiate(m_RockPrefab, transform);
                    m_UnusedRockPool.Add(obj);
                }
                
                GameObject rock = m_UnusedRockPool[0];
                m_UnusedRockPool.RemoveAt(0);
                rocks.Add(rock);
                
                //set random but square scale
                float scale = UnityEngine.Random.Range(m_RockMinScale, m_RockMaxScale);
                rock.transform.localScale = new Vector3(scale, scale, 1);
            }
            RockPerSliceDict.Add(x_pos, rocks);
            return RockPerSliceDict[x_pos];
        }
        
        public void RemoveRocksFromSlice(float x_pos)
        {
            // Debug.Log("Destroying rocks for slice at " + x_pos);
            //Return the rocks to the unused pool
            if (RockPerSliceDict.ContainsKey(x_pos))
            {
                foreach (var rock in RockPerSliceDict[x_pos])
                {
                    rock.SetActive(false);
                    rock.transform.parent = transform;
                    m_UnusedRockPool.Add(rock);
                }
                RockPerSliceDict.Remove(x_pos);
            }
            else
            {
                Debug.LogError("No rocks found for this slice! x_pos: " + x_pos);
            }
        }
    }
}