using System;
using UnityEngine;

namespace HungryWorm
{
    [Serializable]
    public abstract class UIScreen : MonoBehaviour
    {
        #region Inspector fields
        [SerializeField] protected bool m_HideOnAwake = true;
        
        // Is the UI partially see-through? (i.e. use overlay effect)
        [SerializeField] protected bool m_IsTransparent;
        
        #endregion
        
        public bool IsTransparent => m_IsTransparent;
        public bool IsHidden => this.gameObject.activeSelf;

        protected void OnEnable()
        {
            Initialize();
        }

        public virtual void Initialize() {}

        public void Show()
        {
            this.gameObject.SetActive(true);
            //Debug.Log("Setting active : " + this.gameObject.name);
        }
        
        public void Hide()
        {
            this.gameObject.SetActive(false);
            //Debug.Log("Setting inactive : " + this.gameObject.name);
        }
        
        public void Clicked()
        {
            UIEvents.ButtonClicked?.Invoke();
        }
        
        
    }
}