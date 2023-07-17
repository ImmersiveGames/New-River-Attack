/* <summary>
    Namespace:      Utils
    Class:          Tools
    Description:    Ferramentas de auxilio de funções estaticas
    Author:         Renato Innocenti                    Date: 26/03/2018
    Notes:          copyrights 2017-2018 (c) immersivegames.com.br - contato@immersivegames.com.br       
    Revision History:
    Name: v1.0           Date: 26/03/2018       Description: versão funcional
    Name: v2.0           Date: 07/07/2023       Description: Atualização para v2022.3
    </summary>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class Tools
    {
        public static List<T> ScriptableListToList<T>(List<int> listId, List<T> scriptableList) where T : ScriptableObject
        {
            var defaultList = new List<T>();
            foreach (int item in listId)
            {
                foreach (var sItem in scriptableList.Where(sItem => sItem.GetInstanceID() == item))
                {
                    defaultList.Add(sItem);
                    break;
                }
            }
            return defaultList;
        }

        public static List<int> ListToScriptableList<T>(List<T> listProducts) where T : ScriptableObject
        {
            return listProducts.Select(item => item.GetInstanceID()).ToList();
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            var type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (dst == null) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst;
        }

        public static float RateWightList(float weight, float totalWeight)
        {
            return (weight / totalWeight) * 100;
        }
        public static string FirstLetterToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            string lower = s.ToLower();
            char[] a = lower.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
        public static float GetPercentage(float val, float max)
        {
            return (100 * val) / max;
        }
        public static float GetValorPercentage(float porcentage, float max)
        {
            return (max * porcentage) / 100;
        }

        public static bool IsBetween<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
                return false;
            return value.CompareTo(maximum) <= 0;
        }
        /* <summary>
            fix collider whn flipped
            </summary>
            <param name="go">game object whit a collider</param>
        */
        public static void FixBoxCollider(GameObject go)
        {
            if (!go.GetComponent<Collider2D>())
                return;
            var col = go.GetComponent<Collider2D>();
            var sprite = go.GetComponent<SpriteRenderer>();

            float newX = (sprite.flipX) ? -col.offset.x : col.offset.x;
            float newY = (sprite.flipY) ? -col.offset.y : col.offset.y;
            col.offset = new Vector2(newX, newY);
        }
        /* <summary>
            Retorna um vetor2 com o formato em unity points da camera atual
            </summary>
        */
        public static Vector2 camSize
        {
            get
            {
                var cam = Camera.main;
                float height = 2f * cam!.orthographicSize;
                float width = height * cam.aspect;
                return new Vector2(width, height);
            }
        }
        

        public static bool TryParseEnum<TEnum>(string aName, out TEnum aValue) where TEnum : struct
        {
            try
            {
                aValue = (TEnum)Enum.Parse(typeof(TEnum), aName);
                return true;
            }
            catch
            {
                aValue = default(TEnum);
                return false;
            }
        }

        public static void ToggleChildren(Transform myTransform, bool setActive = true)
        {
            if (myTransform.childCount <= 0)
                return;
            for (int i = 0; i < myTransform.childCount; i++)
            {
                myTransform.GetChild(i).gameObject.SetActive(setActive);
            }
        }
        public static void TransformClear(Transform t)
        {
            foreach (Transform child in t)
            {
                Object.Destroy(child.gameObject);
            }
        }
        public class MinMaxRangeAttribute : Attribute
        {
            public MinMaxRangeAttribute(float min, float max)
            {
                Min = min;
                Max = max;
            }
            public float Min { get; private set; }
            public float Max { get; private set; }
        }
        [Serializable]
        public struct FloatRanged
        {
            public float minValue;
            public float maxValue;
        }
    }
}
