using System;
using System.Net;
using System.Text;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace HM.CodeBase
{
    public class Util
    {
        /// <summary>
        /// 내 IP 주소 가져오기 헬퍼
        /// </summary>
        public static string GetMyIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());

            foreach ( var ip in host.AddressList )
            {
                if ( ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork )
                {
                    return ip.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// string으로 되어있는 Vector3 변환
        /// </summary>
        public static Vector3 VectorParse(string vectorString)
        {
            // 문자열에서 괄호 제거
            vectorString = vectorString.Trim('(' , ')');
            // 쉼표로 나누기
            string[] values = vectorString.Split(',');
            if ( values.Length != 3 )
            {
                throw new System.FormatException($"Invalid vector format: {vectorString}");
            }
            // 각각의 값을 float로 변환
            return new Vector3(
                float.Parse(values[ 0 ]) ,
                float.Parse(values[ 1 ]) ,
                float.Parse(values[ 2 ])
            );
        }

        /// <summary>
        /// string으로 되어있는 HaxColor 변환
        /// </summary>
        public static Color HaxToColor(string s)
        {
            Color color = new Color();

            string hax = "#" + s;
            ColorUtility.TryParseHtmlString(hax , out color);

            return color;
        }

        /// <summary>
        /// 주어진 JSON 문자열을 제네릭 타입 T 객체로 역직렬화하여 반환
        /// </summary>
        public static T ParseJson<T>(string json)
        {
            TryParseJson<T>(json, out var result);
            return result;
        }

        /// <summary>
        /// JSON 변환 성공 여부를 반환합니다. 참조 타입의 JSON null은 정상 결과입니다.
        /// </summary>
        public static bool TryParseJson<T>(string json, out T result)
        {
            result = default;
            if ( string.IsNullOrWhiteSpace(json) )
            {
                Debug.LogError("ParseJson: json 문자열이 비어있습니다.");
                return false;
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            catch ( Exception exception )
            {
                Debug.LogError("ParseJson: JSON 파싱 중 오류 발생 - " + exception.Message);
                return false;
            }
        }

        /// <summary>
        /// 일반 C# 데이터를 Newtonsoft.Json으로 저장하고 성공 여부를 반환합니다.
        /// </summary>
        public static bool TryWriteJson(string filePath, object obj)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                File.WriteAllText(filePath, json, Encoding.UTF8);
                return true;
            }
            catch ( Exception exception )
            {
                Debug.LogError($"Json 파일 저장 실패: {filePath}. Error: {exception.Message}");
                return false;
            }
        }
    }
}
