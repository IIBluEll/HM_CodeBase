using System.Collections.Generic;
using System;

namespace HM.CodeBase
{
    public class EventProvider : ASingletone<EventProvider>
    {
        private readonly Dictionary<Type, List<Delegate>> _dic_subscribers = new();

        // 구독
        public void Subscribe<T>(Action<T> pCallback)
        {
            var tType = typeof(T);
            if ( !_dic_subscribers.ContainsKey(tType) )
            {
                _dic_subscribers[ tType ] = new List<Delegate>();
            }
           
            _dic_subscribers[ tType ].Add(pCallback);
        }

        // 구독 해제
        public void Unsubscribe<T>(Action<T> pCallback)
        {
            var tType = typeof(T);
            if ( _dic_subscribers.ContainsKey(tType) )
            {
                _dic_subscribers[ tType ].Remove(pCallback);
            }
        }

        // 이벤트 발행
        public void Publish<T>(T pMessage)
        {
            var tType = typeof(T);
            if ( !_dic_subscribers.ContainsKey(tType) )
            {
                return;
            }
            
            foreach ( var callback in _dic_subscribers[ tType ] )
            {
                ( (Action<T>)callback ).Invoke(pMessage);
            }
        }
    }
}
