using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm
{
    public class ObjectPooler
    {
        private GameObject m_objectToPool;
        private int m_amountToPool;
        private GameObject m_parent;
        private List<GameObject> m_pooledObjects;
        
        private int m_currentIndex = 0;

        public ObjectPooler(GameObject objectToPool, GameObject parent, int amountToPool)
        {
            this.m_objectToPool = objectToPool;
            this.m_parent = parent;
            this.m_amountToPool = amountToPool;
            this.m_pooledObjects = new List<GameObject>();
            m_currentIndex = 0;
        }

        public void InitializePool()
        {
            for (int i = 0; i < m_amountToPool; i++)
            {
                GameObject obj = GameObject.Instantiate(m_objectToPool, m_parent.transform);
                obj.SetActive(false);
                m_pooledObjects.Add(obj);
            }
        }
        
        public GameObject GetPooledObject()
        {
            if(m_pooledObjects.Count == 0)
            {
                InitializePool();
            }

            if (m_currentIndex >= m_amountToPool) m_currentIndex = 0;
            
            GameObject obj = m_pooledObjects[m_currentIndex];
            m_currentIndex++;
            return obj;
        }

        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            if (!m_pooledObjects.Contains(obj))
            {
                m_pooledObjects.Add(obj);
            }
        }
        
        public void ReturnAllToPool()
        {
            foreach (var obj in m_pooledObjects)
            {
                obj.SetActive(false);
            }
        }
        
        public void ClearPool()
        {
            foreach (var obj in m_pooledObjects)
            {
                GameObject.Destroy(obj);
            }
            m_pooledObjects.Clear();
        }

    }
}