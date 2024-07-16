namespace HungryWorm.Items
{
    public class HumanItem: SliceItem
    {
        public HumanItem()
        {
            m_SliceItemType = SliceItemType.HUMAN;
            m_minScale = 1f;
            m_maxScale = 1f;
            m_XMin = - WorldSlice.SliceWidth / 2 + 1;
            m_XMax = WorldSlice.SliceWidth / 2 - 1;
            m_YMin = 0;
            m_YMax = 0;
            m_randomRotation = false;
        }
    }
}