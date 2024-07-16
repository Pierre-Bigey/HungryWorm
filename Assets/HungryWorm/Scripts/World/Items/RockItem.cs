using UnityEngine;

namespace HungryWorm.Items
{
    public class RockItem : SliceItem
    {
        public RockItem()
        {
            m_SliceItemType = SliceItemType.ROCK;
            m_minScale = 1.5f;
            m_maxScale = 3.5f;
            m_XMin = - WorldSlice.SliceWidth / 2;
            m_XMax = WorldSlice.SliceWidth / 2;
            m_YMin = WorldSlice.SliceHeight - 3f;
            m_YMax = -3f;
            m_randomRotation = true;
        }
    }
}