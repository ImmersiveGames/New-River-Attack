using System;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class VersionChecker : MonoBehaviour
    {
        /// <summary>
        /// Compara a versão alvo com a versão atual do jogo.
        /// </summary>
        /// <param name="targetVersion">Versão alvo no formato "major.minor.patch".</param>
        /// <returns>True se a versão alvo for menor que a versão atual, caso contrário False.</returns>
        public static bool IsTargetVersionLower(string targetVersion)
        {
            try
            {
                // Obtém a versão atual do jogo
                var currentVersion = new Version(Application.version);
                // Cria a versão alvo
                var referenceVersion = new Version(targetVersion);

                // Compara se a versão alvo é menor
                return referenceVersion < currentVersion;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao comparar versões: {ex.Message}");
                return false; // Considere que a versão alvo não é menor em caso de erro
            }
        }
        public static bool IsTargetVersionLowerThenSave(string targetVersion, string saveVersion)
        {
            try
            {
                // Obtém a versão atual do jogo
                var currentVersion = new Version(saveVersion);
                // Cria a versão alvo
                var referenceVersion = new Version(targetVersion);

                // Compara se a versão alvo é menor
                return currentVersion < referenceVersion;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erro ao comparar versões: {ex.Message}");
                return false; // Considere que a versão alvo não é menor em caso de erro
            }
        }
    }
}