using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace HungryWorm
{
    public abstract class SliceItem: MonoBehaviour
    {
        public SliceItemType m_SliceItemType;
        
        public float m_minScale;
        public float m_maxScale;

        public float m_XMin;
        public float m_XMax;
        public float m_YMin;
        public float m_YMax;
        
        public float z_pos;

        public bool m_randomRotation;
        
        public virtual void Init()
        {
            PlaceItem();
        }

        public void PlaceItem()
        {
            //Apply all the properties to the item
            float scale = Random.Range(m_minScale, m_maxScale);
            transform.localScale = new UnityEngine.Vector3(scale, scale, scale);
            
            float x = Random.Range(m_XMin, m_XMax);
            float y = Random.Range(m_YMin, m_YMax);
            
            transform.localPosition = new UnityEngine.Vector3(x, y, z_pos);
            
            if (m_randomRotation)
            {
                transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            }
            
        }

    }
}