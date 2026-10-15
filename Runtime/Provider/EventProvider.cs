using System.Collections.Generic;
using System;

namespace HM.CodeBase
{
    public class EventProvider : ASingletone<EventProvider>
    {
        private readonly Dictionary<Type, List<Delegate>> _dic_subscribers = new();

        public void Subscribe<T>(Action<T> pCallback)
        {
            var tType = typeof(T);
            if ( !_dic_subscribers.ContainsKey(tType) )
            {
                _dic_subscribers[ tType ] = new List<Delegate>();
            }
           
            _dic_subscribers[ tType ].Add(pCallback);
        }

        public void Unsubscribe<T>(Action<T> pCallback)
        {
            var tType = typeof(T);
            if ( _dic_subscribers.ContainsKey(tType) )
            {
                _dic_subscribers[ tType ].Remove(pCallback);
            }
        }

        public void Publish<T>(T pMessage)
        {
            var tType = typeof(T);
            if ( !_dic_subscribers.ContainsKey(tType) )
            {
                return;
            }
            
            // 구독 변경은 다음 발행부터 반영하고, 현재 발행은 시작 시점의 목록을 사용합니다.
            var callbacks = _dic_subscribers[ tType ].ToArray();
            foreach ( var callback in callbacks )
            {
                ( (Action<T>)callback ).Invoke(pMessage);
            }
        }
    }
}
