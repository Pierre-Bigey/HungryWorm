using UnityEngine;

namespace HungryWorm
{
    public abstract class UIScreen : MonoBehaviour
    {
        
        
        #region Inspector fields
        protected bool m_HideOnAwake = true;
        
        // Is the UI partially see-through? (i.e. use overlay effect)
        protected bool m_IsTransparent;
        
        #endregion
        
        public bool IsTransparent => m_IsTransparent;
        public bool IsHidden => this.gameObject.activeSelf;

        public void Show()
        {
            this.gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
        }
        
        
    }
}