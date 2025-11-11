using System.Collections.Generic;
using System;
using UnityEngine;

namespace HM.CodeBase
{
    public abstract class ASession { }

    public class SessionProvider : ASingletone<SessionProvider>
    {
        private Dictionary<Enum, ASession> _dic_sessions = new();

        public void CreateSession(Enum pKey , ASession pSession)
        {
            if ( _dic_sessions.ContainsKey(pKey) )
            {
                return;
            }
            _dic_sessions.Add(pKey , pSession);

        }

        public void DeleteSession(Enum pKey)
        {
            if ( _dic_sessions.ContainsKey(pKey) == false )
            {
                Debug.Log("해당 세션이 존재하지 않아 삭제에 실패했습니다.");
                return;
            }

            _dic_sessions.Remove(pKey);
        }

        public ASession GetSession(Enum pKey)
        {
            if ( _dic_sessions.ContainsKey(pKey) == false )
            {
                Debug.Log("해당 세션이 존재하지 않아 가져오지 못했습니다.");
                return null;
            }

            return _dic_sessions[ pKey ];
        }
    }
}