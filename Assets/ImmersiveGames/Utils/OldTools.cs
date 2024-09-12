using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public static class OldTools
    {
        public static List<T> ScriptableListToList<T>(List<int> listId, List<T> scriptableList) where T : ScriptableObject
        {
            var defaultList = new List<T>();
            foreach (var item in listId)
            {
                foreach (var sItem in scriptableList.Where(sItem => sItem.GetInstanceID() == item))
                {
                    defaultList.Add(sItem);
                    break;
                }
            }
            return defaultList;
        }

        public static List<int> ListToScriptableList<T>(IEnumerable<T> listProducts) where T : ScriptableObject
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

        public static float GetPercentage(float val, float max)
        {
            return (100 * val) / max;
        }

        public static float GetValorPercentage(float percentage, float max)
        {
            return (max * percentage) / 100;
        }

        public static bool IsBetween<T>(T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
                return false;
            return value.CompareTo(maximum) <= 0;
        }

        public static Vector2 UnityCamSize
        {
            get
            {
                var cam = Camera.main;
                var height = 2f * cam!.orthographicSize;
                var width = height * cam.aspect;
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

        public static bool EnumIsInEnum<T>(T valorEnum, T enumAlvo) where T : Enum
        {
            var  valorEnumInt = (int)(object)valorEnum;
            var enumAlvoInt = (int)(object)enumAlvo;

            return (valorEnumInt & enumAlvoInt) != 0;
        }

        public static void SetLayersRecursively(LayerMask layerMask, Transform itemTransform)
        {
            var novoLayer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            if (itemTransform.gameObject.layer != novoLayer)
                itemTransform.gameObject.layer = novoLayer;
            for (var i = 0; i < itemTransform.childCount; i++)
            {
                var child = itemTransform.GetChild(i);
                SetLayersRecursively(layerMask, child);
            }
        }

        public static void EqualizeLists<T>(ref List<T> listA, ref List<T> listB) where T : new()
        {
            int maxSize = Mathf.Max(listA.Count, listB.Count);
            while (listA.Count < maxSize)
            {
                listA.Add(new T()); // Preencha com um elemento vazio (pode ser qualquer valor que você considere vazio)
            }
            while (listB.Count < maxSize)
            {
                listB.Add(new T());
            }
        }

        

        public static bool CheckSameElements<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var hashSet = new HashSet<T>(list1);
            var other = new HashSet<T>(list2);

            // Verificar se ambos os conjuntos são iguais
            return hashSet.SetEquals(other);
        }
    }
}