using UnityEngine;

namespace HM.CodeBase
{
    /// <summary>
    /// 모든 UI는 이 스크립트를 상속 받아 사용 요망.
    /// </summary>
    public abstract class AView : MonoBehaviour, IOpen
    {
        public virtual void Open()
        {
            this.gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            this.gameObject.SetActive(false);
        }

        public virtual void Clear() { }

    }
}