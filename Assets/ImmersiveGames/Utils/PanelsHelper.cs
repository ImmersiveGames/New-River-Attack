﻿using System.Collections;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    
    public static class PanelsHelper
    {
        public static IEnumerator TogglePanel(GameObject panel, float openTimeDuration, float closeTimeDuration, bool open, bool destroy = true)
        {
            var duration = open ? openTimeDuration : closeTimeDuration;
            var initialScale = panel.transform.localScale;
            // Define o alvo de escala apenas se necessário
            var targetScale = open ? (initialScale != Vector3.one ? Vector3.one : initialScale) : Vector3.zero;

            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                panel.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            panel.transform.localScale = targetScale; // Garante a escala exata

            if (open) yield break;
            // Adiciona um pequeno atraso para exibir a escala final antes da destruição
            yield return new WaitForSeconds(0.1f);
            if(destroy)
                Object.Destroy(panel);
        }
        public static bool GetPanelActiveState(GameObject panel)
        {
            // Implementação da verificação de estado do painel
            return panel.activeInHierarchy && panel.transform.localScale == Vector3.one;
        }
    }
}