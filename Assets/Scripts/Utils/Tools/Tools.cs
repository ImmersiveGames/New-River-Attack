using System;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Utils
{
    public static class Tools
    {
        /*
         * SerializableDictionary<TKey, TValue>]
         * - Cria um dicionário serializado para ser exibido no inspector
         * */
        [Serializable]
        public class SerializableDictionary<TKey, TValue>
        {
            [SerializeField]
            List<TKey> keys = new List<TKey>();

            [SerializeField]
            List<TValue> values = new List<TValue>();

            public void Add(TKey key, TValue value)
            {
                keys.Add(key);
                values.Add(value);
            }

            public bool TryGetValue(TKey key, out TValue value)
            {
                int index = keys.IndexOf(key);
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
        /*public static string FirstLetterToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            string lower = s.ToLower();
            char[] a = lower.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }*/
        /*
         * GetPercentage(float val, float max)
         * - retorna a porcentagem de um valor em porcentagem
         * */
        public static float GetPercentage(float val, float max)
        {
            return (100 * val) / max;
        }
        /*
         * GetValorPercentage(float porcentage, float max)
         * - Retorna o valor de uma porcentagem
         * */
        public static float GetValorPercentage(float porcentage, float max)
        {
            return (max * porcentage) / 100;
        } 
        /*
         * IsBetween<T>(this T value, T minimum, T maximum)
         * - Verifica se o valor está entre dois numeros.
         * */
        public static bool IsBetween<T>(this T value, T minimum, T maximum) where T : IComparable<T>
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
                float height = 2f * cam!.orthographicSize;
                float width = height * cam.aspect;
                return new Vector2(width, height);
            }
        }
        /*
         * TryParseEnum<TEnum>(string aName, out TEnum aValue)
         * - Tenta converter o valor de um enum em um tipo especico (String, int,..)
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
        /*
         * ToggleChildren(Transform myTransform, bool setActive = true)
         * - Des/Ativa os filhos de um objeto
         * */
        public static void ToggleChildren(Transform myTransform, bool setActive = true)
        {
            if (myTransform.childCount <= 0)
                return;
            for (int i = 0; i < myTransform.childCount; i++)
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
                Object.Destroy(child.gameObject);
            }
        }
        /*
         * SetLayersRecursively(LayerMask layerMask, Transform itemTransform)
         * - Define layers para todos os objetos partentes de forma recursiva
         * */
        public static void SetLayersRecursively(LayerMask layerMask, Transform itemTransform)
        {
            int novoLayer = Mathf.RoundToInt(Mathf.Log(layerMask.value, 2));
            if (itemTransform.gameObject.layer != novoLayer)
                itemTransform.gameObject.layer = novoLayer;
            for (int i = 0; i < itemTransform.childCount; i++)
            {
                var child = itemTransform.GetChild(i);
                SetLayersRecursively(layerMask, child);
            }
        } 
        /*
         * SetFollowVirtualCam(CinemachineVirtualCamera virtualCamera, Transform follow)
         * - Define o objeto que a camera virtual irá seguir
         * */
        public static void SetFollowVirtualCam(CinemachineVirtualCamera virtualCamera, Transform follow)
        {
            virtualCamera.Follow = follow;
        }
        /*
         * ChangeBindingReference(string track, Object animator, PlayableDirector playableDirector)
         * - Substituir a referência nula pelo Animator desejado em um Timeline
         * */
        public static void ChangeBindingReference(string track, Object animator, PlayableDirector playableDirector)
        {
            foreach (var playableBinding in playableDirector.playableAsset.outputs)
            {
                if (playableBinding.streamName != track)
                    continue;
                var bindingReference = playableDirector.GetGenericBinding(playableBinding.sourceObject);

                if (bindingReference == null)
                {
                    // Substituir a referência nula pelo Animator desejado
                    playableDirector.SetGenericBinding(playableBinding.sourceObject, animator);
                }
            }
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
        public static float GetAnimationTime(Animator animator, string animationName)
        {
            var controller = animator.runtimeAnimatorController;

            return (from t in controller.animationClips where t.name == animationName select t.length).FirstOrDefault();
        }
/*
 * SoundBase10(float normalizeNumber)
 *  - transforma um numero base de 0.0 à 1.0 em um valor de metrica para sons (dB)
 */
        public static float SoundBase10(float normalizeNumber)
        {
            return Mathf.Log10(normalizeNumber) * 20f;
        }
        
        public static string TimeFormat(float timeToFormat)
        {
            int hour = Mathf.FloorToInt(timeToFormat / 3600);
            int minutes = Mathf.FloorToInt((timeToFormat % 3600) / 60);
            int seconds = Mathf.FloorToInt(timeToFormat % 60);

            string time = $"{hour:D2}:{minutes:D2}:{seconds:D2}";
            
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
