using UnityEngine;

namespace HM.CodeBase
{
    public abstract class ASingletone<T> : MonoBehaviour where T : ASingletone<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if ( s_instance != null )
                {
                    return s_instance;
                }

                s_instance = FindFirstObjectByType<T>(FindObjectsInactive.Include);
                if ( s_instance == null )
                {
                    GameObject instanceObj = new(typeof(T).Name);
                    s_instance = instanceObj.AddComponent<T>();
                    Debug.Log($"[Singleton] {typeof(T).Name} 인스턴스가 필요하기 때문에, '{instanceObj}'을(를) 생성했습니다.");
                }

                return s_instance;
            }
        }

        // 파생 클래스에서 재정의할 때는 base.Awake()를 호출해야 합니다.
        public virtual void Awake()
        {
            if ( s_instance != null && s_instance != this )
            {
                Destroy(this);
                Debug.LogWarning($"[Singleton] {typeof(T).Name} 인스턴스가 중복 존재하여 제거되었습니다.");
                return;
            }

            s_instance = (T)this;
        }
    }
}
