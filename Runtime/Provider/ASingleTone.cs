using UnityEngine;

namespace HM.CodeBase
{
    public abstract class ASingletone<T> : MonoBehaviour where T : ASingletone<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                Object[] tArr_obj = FindObjectsByType(typeof(T), FindObjectsSortMode.None);
                if ( tArr_obj == null || tArr_obj.Length == 0 )
                {
                    GameObject tObj = new();
                    _instance = tObj.AddComponent<T>();
                    tObj.name = typeof(T).Name;

                    Debug.Log($"[Singleton] {typeof(T).Name} 인스턴스가 필요하기 때문에, '{tObj}'을(를) 생성했습니다.");
                }
                else
                {
                    _instance = tArr_obj[ 0 ] as T;
                    if ( tArr_obj.Length > 1 )
                    {
                        Debug.LogWarning($"[Singleton] {typeof(T).Name}은 하나만 배치되어야 합니다!");
                    }
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            if ( _instance != null && _instance != this )
            {
                Destroy(this);
                Debug.LogWarning($"[Singleton] {typeof(T).Name} 인스턴스가 중복 존재하여 제거되었습니다.");
                return;
            }
        }
    }


}