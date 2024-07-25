using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace HungryWorm

{
    public class WorldManager : MonoBehaviour
    {

        [SerializeField] private GameObject m_worldSlicePrefab;
        
        [SerializeField] private int m_poolSize = 5;


        private List<WorldSlice> worldSlicesPool;

        
        
        private float m_worldSliceWidth;




        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_worldSliceWidth = WorldSlice.SliceWidth;
            
            
            Camera cam = Camera.main;
            if (cam == null)
            {
                Debug.LogError("No main camera found");
                return;
            }
            
            InitializeWorld();
        }

        private void OnEnable()
        {
            GameEvents.GameStarted += InstantiateWorld;
            GameEvents.GameClosed += ResetWorld;
            
            WorldEvents.MoveSlice += MoveSlice;
        }
        
        private void OnDisable()
        {
            GameEvents.GameStarted -= InstantiateWorld;
            GameEvents.GameClosed -= ResetWorld;
            
            WorldEvents.MoveSlice -= MoveSlice;
        }
        
        private void InstantiateWorld()
        {
            for (int i = 0; i < m_poolSize; i++)
            {
                WorldSlice worldSlice = worldSlicesPool[i];
                worldSlice.gameObject.SetActive(true);
                int xPosIndex = i - (m_poolSize / 2);
                worldSlice.InitializeSlice(xPosIndex * m_worldSliceWidth);
            }
            float leftEnd = worldSlicesPool[0].transform.position.x;
            float rightEnd = worldSlicesPool[m_poolSize - 1].transform.position.x + m_worldSliceWidth;
            
            WorldEvents.LeftEdgeUpdated?.Invoke(leftEnd);
            WorldEvents.RightEdgeUpdated?.Invoke(rightEnd);
        }
        
        private void ResetWorld()
        {
            foreach (var worldSlice in worldSlicesPool)
            {
                worldSlice.DisableSlice();
                worldSlice.gameObject.SetActive(false);
            }
        }

        private void InitializeWorld()
        {
            worldSlicesPool = new List<WorldSlice>();
            for (int i = 0; i < m_poolSize; i++)
            {
                GameObject slice = Instantiate(m_worldSlicePrefab, transform);
                slice.name = "WorldSlice" + i;
                slice.SetActive(false);
                WorldSlice worldSlice = slice.GetComponent<WorldSlice>();
                /*int xPosIndex = i - (m_poolSize / 2);
                worldSlice.Initialize(xPosIndex * m_worldSliceWidth);*/
                worldSlicesPool.Add(worldSlice);
            }

            /*leftEnd = worldSlicesPool[0].transform.position.x;
            rightEnd = worldSlicesPool[m_poolSize - 1].transform.position.x + m_worldSliceWidth;*/
        }

        private void MoveSlice(Direction direction)
        {
            // Debug.Log("Move slice " + direction);
            if (direction == Direction.Left)
            {
                float newXPos = worldSlicesPool[0].transform.position.x - m_worldSliceWidth;
                WorldSlice newSlice = worldSlicesPool[m_poolSize - 1];
                newSlice.DisableSlice();
                worldSlicesPool.RemoveAt(m_poolSize - 1);
                worldSlicesPool.Insert(0, newSlice);
                newSlice.InitializeSlice(newXPos);
                float leftEnd = newSlice.transform.position.x;
                float rightEnd = worldSlicesPool[m_poolSize - 1].transform.position.x + m_worldSliceWidth;
                WorldEvents.LeftEdgeUpdated?.Invoke(leftEnd);
                WorldEvents.RightEdgeUpdated?.Invoke(rightEnd);
            }
            else
            {
                float newXPos = worldSlicesPool[m_poolSize - 1].transform.position.x + m_worldSliceWidth;
                WorldSlice newSlice = worldSlicesPool[0];
                newSlice.DisableSlice();
                worldSlicesPool.RemoveAt(0); 
                worldSlicesPool.Add(newSlice);
                newSlice.InitializeSlice(newXPos);
                float rightEnd = newSlice.transform.position.x + m_worldSliceWidth;
                float leftEnd = worldSlicesPool[0].transform.position.x;
                WorldEvents.LeftEdgeUpdated?.Invoke(leftEnd);
                WorldEvents.RightEdgeUpdated?.Invoke(rightEnd);
            }
        }

        
    }

    public enum Direction
     {
         Right,
         Left
     }
}
