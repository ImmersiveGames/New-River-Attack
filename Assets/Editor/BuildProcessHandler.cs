using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    // Classe para processar o build e resetar ScriptableObject antes de criar a build
    public class BuildProcessHandler : IPreprocessBuildWithReport
    {
        // Ordem de execução no build (quanto menor, mais cedo o código é executado)
        public int callbackOrder => 0;

        // Referência ao ScriptableObject que queremos resetar
        private GemeStatisticsDataLog _statisticsDataLog;

        // Método que será chamado antes de iniciar a build
        public void OnPreprocessBuild(BuildReport report)
        {
            // Carrega o ScriptableObject
            _statisticsDataLog = Resources.Load<GemeStatisticsDataLog>("SavesSO/GemeStatisticsDataLog");

            if (_statisticsDataLog != null)
            {
                // Reseta os dados do ScriptableObject
                _statisticsDataLog.ResetLogs();
                Debug.Log("GemeStatisticsDataLog foi resetado antes da build.");
                
                // Marca o objeto como sujo para garantir que ele seja salvo, se necessário
                EditorUtility.SetDirty(_statisticsDataLog);
            }
            else
            {
                Debug.LogWarning("GemeStatisticsDataLog não foi encontrado em 'SavesSO/GemeStatisticsDataLog'.");
            }
        }
    }
}