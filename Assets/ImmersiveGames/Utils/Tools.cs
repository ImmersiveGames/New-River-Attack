using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace ImmersiveGames.Utils
{
    public abstract class Tools
    {
        /*
         * SerializableDictionary<TKey, TValue>]
         * - Cria um dicionário serializable para ser exibido no inspector
         * */
        [Serializable]
        public class SerializableDictionary<TKey, TValue>
        {
            [SerializeField] private List<TKey> keys = new List<TKey>();

            [SerializeField] private List<TValue> values = new List<TValue>();

            public void Add(TKey key, TValue value)
            {
                keys.Add(key);
                values.Add(value);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                var index = keys.IndexOf(key);
                if (index != -1)
                {
                    value = values[index];
                    return true;
                }

                value = default(TValue);
                return false;
            }
        }
        /*
         * ScriptableListToList<T>(List<int> listId, List<T> scriptableList)
         * - Transforma uma Lista de Scriptable Objects em uma lista padrão
         * */
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
        /*
         *  ListToScriptableList<T>(IEnumerable<T> listProducts)
         *  - Transforma ula lista de em uma Lista Escriptable Eumerada
         * */
        public static List<int> ListToScriptableList<T>(IEnumerable<T> listProducts) where T : ScriptableObject
        {
            return listProducts.Select(item => item.GetInstanceID()).ToList();
        } 
        /*
         * CopyComponent<T>(T original, GameObject destination)
         * - Copia um tipo de componente T de um objeto em outro em tempo de execução
         * */
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
        /*
         * RateWightList(float weight, float totalWeight)
         * - retorna a porcentagem de um valor em base 100 pelo seu total acumulado
         * */
        public static float RateWightList(float weight, float totalWeight)
        {
            return (weight / totalWeight) * 100;
        }
       
        /*
         * GetPercentage(float val, float max)
         * - retorna a porcentagem de um valor em porcentagem
         * */
        public static float GetPercentage(float val, float max)
        {
            return (100 * val) / max;
        }
        /*
         * GetValorPercentage(float percentage, float max)
         * - Retorna o valor de uma porcentagem
         * */
        public static float GetValorPercentage(float percentage, float max)
        {
            return (max * percentage) / 100;
        } 
        /*
         * IsBetween<T>(this T value, T minimum, T maximum)
         * - Verifica se o valor está entre dois números.
         * */
        public static bool IsBetween<T>(T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
                return false;
            return value.CompareTo(maximum) <= 0;
        }

        /* <summary>
            Retorna um vetor2 com o formato em unity points da camera atual
            </summary>
        */
        public static Vector2 unityCamSize
        {
            get
            {
                var cam = Camera.main;
                var height = 2f * cam!.orthographicSize;
                var width = height * cam.aspect;
                return new Vector2(width, height);
            }
        }
        /*
         * TryParseEnum<TEnum>(string aName, out TEnum aValue)
         * - Tenta converter o valor de um enum em um tipo especifico (String, int,..)
         * */
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
        /*
         * ToggleChildren(Transform myTransform, bool setActive = true)
         * - Des/Ativa os filhos de um objeto
         * */
        public static void ToggleChildren(Transform myTransform, bool setActive = true)
        {
            if (myTransform.childCount <= 0)
                return;
            for (var i = 0; i < myTransform.childCount; i++)
            {
                myTransform.GetChild(i).gameObject.SetActive(setActive);
            }
        }
        /*
         * TransformClear(Transform t)
         * - Destroy os filhos de um objeto
         * */
        public static void TransformClear(Transform t)
        {
            foreach (Transform child in t)
            {
                UnityEngine.Object.Destroy(child.gameObject);
            }
        }
        /*
         * SetLayersRecursively(LayerMask layerMask, Transform itemTransform)
         * - Define layers para todos os objetos parentes de forma recursiva
         * */
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
        /*
         * SetFollowVirtualCam(Cinemachine Virtual Camera virtualCamera, Transform follow)
         * - Define o objeto que a camera virtual irá seguir
         * */
        public static void SetFollowVirtualCam(CinemachineVirtualCamera virtualCamera, Transform follow)
        {
            virtualCamera.Follow = follow;
        }
        
        /*
         * EqualizeLists<T>(ref List<T> listA, ref List<T> listB)
         * - Torna as duas listas com quantidade iguais. (não valores)
         * */
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
        /*
         * GetAnimationTime(Animator animator, string animationName)
         * - retorna o tempo da animação
         * */
        public static float GetAnimationDuration(Animator animator, string animationName)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning("Animator or Animator Controller not set. Unable to get animation duration.");
                return 0f;
            }

            var clip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == animationName);

            if (clip != null)
            {
                return clip.length;
            }

            Debug.LogWarning($"Animation clip '{animationName}' not found in the animator controller.");
            return 0f;
            
        }
        
        public static string TimeFormat(float timeToFormat)
        {
            var hour = Mathf.FloorToInt(timeToFormat / 3600);
            var minutes = Mathf.FloorToInt((timeToFormat % 3600) / 60);
            var seconds = Mathf.FloorToInt(timeToFormat % 60);

            var time = $"{hour:D2}:{minutes:D2}:{seconds:D2}";
            
            return time;
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
