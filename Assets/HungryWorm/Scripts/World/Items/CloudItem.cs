using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungryWorm.Items
{
    public class CloudItem: SliceItem
    {
        [SerializeField] List<GameObject> m_CloudModels;
        
        public CloudItem()
        {
            m_SliceItemType = SliceItemType.CLOUD;
            m_minScale = 0.7f;
            m_maxScale = 1.2f;
            m_XMin = - WorldSlice.SliceWidth / 2 + 1;
            m_XMax = WorldSlice.SliceWidth / 2 - 1;
            m_YMin = 10;
            m_YMax = 25;
            m_randomRotation = false;
        }

        public override void Init()
        {
            base.Init();
            ChooseModel();
        }

        private void ChooseModel()
        {
            //Enable a random cloud model
            int index = UnityEngine.Random.Range(0, m_CloudModels.Count);
            m_CloudModels[index].SetActive(true);
        }

        private void OnDisable()
        {
            //Disable all cloud models
            foreach (var cloudModel in m_CloudModels)
            {
                cloudModel.SetActive(false);
            }
        }
    }
}