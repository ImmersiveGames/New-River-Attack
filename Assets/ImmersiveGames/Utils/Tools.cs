using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.ObstaclesSystems;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ImmersiveGames.Utils
{
    public class Tools
    {
        
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
         * GetAnimationTime(Animator animator, string animationName)
         * - retorna o tempo da animação
         * */

        public static string TimeFormat(float timeToFormat)
        {
            var hour = Mathf.FloorToInt(timeToFormat / 3600);
            var minutes = Mathf.FloorToInt((timeToFormat % 3600) / 60);
            var seconds = Mathf.FloorToInt(timeToFormat % 60);

            var time = $"{hour:D2}:{minutes:D2}:{seconds:D2}";
            
            return time;
        }
        public static float GetAnimationDuration(Animator animator, string animationName)
        {
            if (animator == null || animator.runtimeAnimatorController == null)
            {
                DebugManager.LogWarning<Tools>("Animator or Animator Controller not set. Unable to get animation duration.");
                return 0f;
            }

            var clip = animator.runtimeAnimatorController.animationClips.FirstOrDefault(c => c.name == animationName);

            if (clip != null)
            {
                return clip.length;
            }

            DebugManager.LogWarning<Tools>($"Animation clip '{animationName}' not found in the animator controller.");
            return 0f;
            
        }

        public static GameObject GetRandomDrop(EnemyDropData[] items)
        {
            // Calcular a soma total das chances
            var totalChance = items.Aggregate(0, (current, item) => (current + item.dropChance));

            // Gerar um número aleatório entre 0 e a soma total das chances
            var randomValue = Random.Range(0, totalChance);

            // Percorrer os itens e retornar o item cujo intervalo contém o valor aleatório
            var cumulativeChance = 0;
            foreach (var item in items)
            {
                cumulativeChance += item.dropChance;
                if (randomValue < cumulativeChance)
                {
                    return item.prefabPowerUp;
                }
            }
            return null;
        }
    }
    
}
