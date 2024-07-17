using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    /// <summary>
    /// Class to pool food objects, then a world slice can call it to get a food object
    /// </summary>
    public class SliceItemManager: MonoBehaviour
    {
        //Singleton Pattern
        public static SliceItemManager Instance { get; private set; }
        
        [Header("Rocks")]
        [SerializeField] private GameObject m_RockPrefab;
        [SerializeField] private int m_InitialRockPoolSize = 15;
        [SerializeField] private int m_minRockAmout = 2;
        [SerializeField] private int m_maxRockAmount = 4;
        
        [Header("Humans")]
        [SerializeField] private GameObject m_HumanPrefab;
        [SerializeField] private int m_InitialHumanPoolSize = 15;
        [SerializeField] private int m_minHumanAmount = 2;
        [SerializeField] private int m_maxHumanAmount = 4;
        
        [Header("Clouds")]
        [SerializeField] private GameObject m_CloudPrefab;
        [SerializeField] private int m_InitialCloudPoolSize = 15;
        [SerializeField] private int m_minCloudAmount = 1;
        [SerializeField] private int m_maxCloudAmount = 3;
        
        private Dictionary<SliceItemType, SliceItemPool> SliceItemPoolDict;
        private Dictionary<float, List<GameObject>> ItemPerSliceDict;    
        
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
            ItemPerSliceDict = new Dictionary<float, List<GameObject>>();
            
            InitializePools();
            InstantiateItems();
        }

        private void InitializePools()
        {
            SliceItemPoolDict = new Dictionary<SliceItemType, SliceItemPool>();
            
            foreach (SliceItemType sliceItemType in Enum.GetValues(typeof(SliceItemType)))
            {
                SliceItemPool sliceItemPool = new SliceItemPool();
                sliceItemPool.pool = new List<GameObject>();

                switch (sliceItemType)
                {
                    case SliceItemType.ROCK:
                        sliceItemPool.prefab = m_RockPrefab;
                        sliceItemPool.initialPoolSize = m_InitialRockPoolSize;
                        sliceItemPool.minAmountToSpawn = m_minRockAmout;
                        sliceItemPool.maxAmountToSpawn = m_maxRockAmount;
                        break;
                    case SliceItemType.HUMAN:
                        sliceItemPool.prefab = m_HumanPrefab;
                        sliceItemPool.initialPoolSize = m_InitialHumanPoolSize;
                        sliceItemPool.minAmountToSpawn = m_minHumanAmount;
                        sliceItemPool.maxAmountToSpawn = m_maxHumanAmount;
                        break;
                    case SliceItemType.CLOUD:
                        sliceItemPool.prefab = m_CloudPrefab;
                        sliceItemPool.initialPoolSize = m_InitialCloudPoolSize;
                        sliceItemPool.minAmountToSpawn = m_minCloudAmount;
                        sliceItemPool.maxAmountToSpawn = m_maxCloudAmount;
                        break;
                }
                
                SliceItemPoolDict.Add(sliceItemType, sliceItemPool);
            }
        }

        private void InstantiateItems()
        {
            foreach (SliceItemType sliceItemType in Enum.GetValues(typeof(SliceItemType)))
            {
                SliceItemPool sliceItemPool = SliceItemPoolDict[sliceItemType];

                for (int i = 0; i < sliceItemPool.initialPoolSize; i++)
                {
                    GameObject obj = GameObject.Instantiate(sliceItemPool.prefab, transform);
                    obj.SetActive(false);
                    sliceItemPool.pool.Add(obj);
                }
            }
        }
        
        public List<GameObject> GetItemsForSlice(float x_pos)
        {
            List<GameObject> items = new List<GameObject>();
            
            // cycle through item types and get a random number of items for each type
            foreach (SliceItemType sliceItemType in Enum.GetValues(typeof(SliceItemType)))
            {
                SliceItemPool sliceItemPool = SliceItemPoolDict[sliceItemType];
                
                int itemAmount = UnityEngine.Random.Range(sliceItemPool.minAmountToSpawn, sliceItemPool.maxAmountToSpawn);
                for (int i = 0; i < itemAmount; i++)
                {
                    if (sliceItemPool.pool.Count == 0)
                    {
                        GameObject obj = GameObject.Instantiate(sliceItemPool.prefab, transform);
                        sliceItemPool.pool.Add(obj);
                    }

                    GameObject item = sliceItemPool.pool[0];
                    sliceItemPool.pool.RemoveAt(0);
                    items.Add(item);
                }
            }
            
            ItemPerSliceDict.Add(x_pos, items);
            return items;
        }
        
        public void RemoveItemsFromSlice(float x_pos)
        {
            Debug.Log("Destroying items for slice at " + x_pos);
            //Return the items to the unused pool
            if (ItemPerSliceDict.ContainsKey(x_pos))
            {
                foreach (var item in ItemPerSliceDict[x_pos])
                {
                    item.SetActive(false);
                    item.transform.parent = transform;
                    SliceItemType sliceItemType = item.GetComponent<SliceItem>().m_SliceItemType;
                    SliceItemPoolDict[sliceItemType].pool.Add(item);
                }
                ItemPerSliceDict[x_pos].Clear();
                ItemPerSliceDict.Remove(x_pos);
            }
            else
            {
                Debug.LogError("No items found for this slice! x_pos: " + x_pos);
            }
        }
    }
    
    
}