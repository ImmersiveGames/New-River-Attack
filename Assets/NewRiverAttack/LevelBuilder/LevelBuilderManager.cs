using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.Utils;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    public class LevelBuilderManager : Singleton<LevelBuilderManager>
    {
        //public Transform spawnPoint;
        public int startNumOfSegments = 5;
        public int maxBehindActive = 1;
        public int maxSegmentsInFront = 2;
        public Vector3 offset;

        [SerializeField] private List<ScenarioObjectData> activeObjects = new List<ScenarioObjectData>();
        private int _nextSegmentIndex; // Índice do próximo segmento a ser adicionado

        private GameObject _initialSegmentObject;
        private bool _initialSegmentInstantiated;

        private GameObject _setsContainer;
        private LevelData _levelData;

        public void StartToBuild(LevelData data)
        {
            _levelData = data;
            _nextSegmentIndex = 0;
            // Criar container para os sets
            _setsContainer = new GameObject(data.levelNameLocale.GetLocalizedString());

            InitialSegment(data.pathStart);

            // Instanciar os objetos do cenário
            AddNextSegment(startNumOfSegments);
        }

        private void InitialSegment(GameObject pathStart)
        {
            var spawnPosition = _setsContainer.transform.position;

            // Instanciar o segmento inicial se ainda não foi instanciado
            if (_initialSegmentInstantiated || pathStart == null) return;
            _initialSegmentObject =
                Instantiate(pathStart, spawnPosition, Quaternion.identity, _setsContainer.transform);
            var initialSegmentBounds = CalculateRealLength.GetBounds(_initialSegmentObject);
            var initialSegmentLength = initialSegmentBounds.size.z; // Acessar tamanho no eixo Z
            var initialSegmentPosition = spawnPosition - Vector3.forward * initialSegmentLength + offset;
            _initialSegmentObject.transform.position = initialSegmentPosition;
            _initialSegmentInstantiated = true;
        }

        private void AddNextSegment(int count)
        {
            DebugManager.Log<LevelBuilderManager>($"AddNextSegment chamado com count = {count}");

            if (count <= 0 || (_levelData.setLevelList.Count > 0 && _levelData.setLevelList.Count - _nextSegmentIndex < count))
            {
                DebugManager.LogWarning<LevelBuilderManager>("Quantidade de segmentos a adicionar inválida.");
                return;
            }

            var spawnTransform = _setsContainer.transform;

            for (var i = 0; i < count; i++)
            {
                DebugManager.Log<LevelBuilderManager>($"Índice do segmento a ser adicionado: {_nextSegmentIndex}");

                // Garantir que o índice não ultrapasse a quantidade de segmentos definidos
                if (_nextSegmentIndex >= _levelData.setLevelList.Count)
                {
                    DebugManager.LogWarning<LevelBuilderManager>(
                        "Tentativa de adicionar mais segmentos do que o disponível em LevelData");
                    return;
                }

                var segmentPrefab = _levelData.setLevelList[_nextSegmentIndex].segmentObject;
                var newSegment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity,
                    _setsContainer.transform);
                DebugManager.Log<LevelBuilderManager>($"Segmento instanciado: {segmentPrefab.name}");

                var previousSegmentLength = activeObjects.Count > 0
                    ? CalculateRealLength.GetBounds(activeObjects[^1].segmentObject).size.z
                    : 0;
                DebugManager.Log<LevelBuilderManager>($"Comprimento do segmento anterior: {previousSegmentLength}");

                Vector3 position;
                if (activeObjects.Count > 0)
                {
                    var previousSegment = activeObjects[^1].segmentObject;
                    var newSegmentPositionZ = previousSegment.transform.position.z + previousSegmentLength + offset.z;

                    position = newSegment.transform.position;
                    position = new Vector3(position.x + offset.x, position.y + offset.y, newSegmentPositionZ);
                    newSegment.transform.position = position;
                    DebugManager.Log<LevelBuilderManager>($"Posição do novo segmento (não primeiro): {position}");
                }
                else
                {
                    position = newSegment.transform.position;
                    position = new Vector3(position.x + offset.x, position.y + offset.y,
                        spawnTransform.position.z + offset.z);
                    newSegment.transform.position = position;
                    DebugManager.Log<LevelBuilderManager>($"Posição do novo segmento (primeiro): {position}");
                }

                GameObject enemySetInstance = null;
                if (_levelData.setLevelList[_nextSegmentIndex].enemySetObject != null)
                {
                    enemySetInstance = Instantiate(_levelData.setLevelList[_nextSegmentIndex].enemySetObject,
                        newSegment.transform.position, newSegment.transform.rotation, _setsContainer.transform);
                    enemySetInstance.SetActive(true);
                    var enemiesMaster = enemySetInstance.GetComponentsInChildren<ObjectMaster>();
                    foreach (var enemy in enemiesMaster)
                    {
                        enemy.InitializeObject();
                    }

                    DebugManager.Log<LevelBuilderManager>(
                        $"Conjunto de inimigos instanciado: {_levelData.setLevelList[_nextSegmentIndex].enemySetObject.name}");
                }

                var newSegmentPosition = activeObjects.Count > 0
                    ? activeObjects[^1].absolutePosition + previousSegmentLength + offset.z
                    : spawnTransform.position.z + offset.z;

                var newScenarioData = new ScenarioObjectData(newSegment, enemySetInstance, newSegmentPosition);
                activeObjects.Add(newScenarioData);
                DebugManager.Log<LevelBuilderManager>(
                    $"Novo segmento adicionado à lista de objetos ativos: {newSegment.name}");

                _nextSegmentIndex++; // Incrementar o índice para o próximo segmento
            }

            if (_nextSegmentIndex >= _levelData.setLevelList.Count && _levelData.pathEnd != null &&
                activeObjects.Count > 0)
            {
                FinalSegment(_levelData.pathEnd);
                DebugManager.Log<LevelBuilderManager>($"Segmento final instanciado: {_levelData.pathEnd.name}");
            }
        }

        private void RemoveSegments(int startIndex, int count)
        {
            DebugManager.Log<LevelBuilderManager>(
                $"RemoveSegments chamado com startIndex = {startIndex}, count = {count}");

            if (startIndex < 0 || count <= 0 || startIndex + count > activeObjects.Count)
            {
                DebugManager.LogWarning<LevelBuilderManager>("Índices de remoção inválidos.");
                return;
            }

            for (var i = 0; i < count; i++)
            {
                var segmentData = activeObjects[startIndex];
                if (segmentData.segmentObject != null)
                {
                    Destroy(segmentData.segmentObject);
                    DebugManager.Log<LevelBuilderManager>($"Segmento removido: {segmentData.segmentObject.name}");
                }

                if (segmentData.enemySetObject != null)
                {
                    CleanupEnemies(segmentData.enemySetObject);
                    Destroy(segmentData.enemySetObject);
                    DebugManager.Log<LevelBuilderManager>(
                        $"Conjunto de inimigos removido: {segmentData.enemySetObject.name}");
                }

                activeObjects.RemoveAt(startIndex);
            }

            // Ajustar o índice para garantir que o próximo segmento adicionado seja o correto
            // Se todos os segmentos foram removidos, reset a lista e recomeça do início
            if (activeObjects.Count == 0)
            {
                DebugManager.Log<LevelBuilderManager>(
                    "Todos os segmentos removidos, reiniciando a lista de segmentos ativos.");
                // Reiniciar contagem se necessário
            }
        }

        private void FinalSegment(GameObject endPath)
        {
            var lastSegment = activeObjects[^1].segmentObject;
            var lastSegmentBounds = CalculateRealLength.GetBounds(lastSegment);
            var lastSegmentLength = lastSegmentBounds.size.z;
            var finalSegmentInstance = Instantiate(endPath,
                lastSegment.transform.position + Vector3.forward * lastSegmentLength,
                Quaternion.identity);
        }

        private ScenarioObjectData? FindSegmentAtPosition(float zPosition)
        {
            DebugManager.Log<LevelBuilderManager>("Procurando segmento na posição Z: " + zPosition);

            var closestData = activeObjects
                .Select(data => new { Data = data, Distance = Mathf.Abs(data.absolutePosition - zPosition) })
                .OrderBy(x => x.Distance)
                .FirstOrDefault();

            if (closestData == null)
            {
                DebugManager.Log<LevelBuilderManager>("Nenhum segmento encontrado.");
                return null;
            }

            DebugManager.Log<LevelBuilderManager>("Segmento mais próximo encontrado: " +
                                                  closestData.Data.segmentObject.name);
            return closestData.Data;
        }


        public void OptimizeSegments(float position)
        {
            DebugManager.Log<LevelBuilderManager>("Otimizando segmentos com base na posição Z: " + position);

            // Encontrar o segmento ativo
            var activeSegment = FindSegmentAtPosition(position);
            if (activeSegment == null)
            {
                DebugManager.Log<LevelBuilderManager>("Segmento ativo encontrado: nenhum");
                return;
            }

            var activeSegmentIndex = activeObjects.IndexOf((ScenarioObjectData)activeSegment);

            DebugManager.Log<LevelBuilderManager>("Índice do segmento ativo: " + activeSegmentIndex);

            // Desativar segmentos atrás do ativo
            var segmentsToRemoveCount = Mathf.Max(0, activeSegmentIndex - maxBehindActive);
            if (segmentsToRemoveCount > 0)
            {
                RemoveSegments(0, segmentsToRemoveCount);
            }

            DebugManager.Log<LevelBuilderManager>("Índice do segmento para remover: " + segmentsToRemoveCount);

            // Calcular a quantidade de segmentos a serem instanciados à frente
            var maxScenarioObjects = maxBehindActive + maxSegmentsInFront + 1;
            var segmentsToInstantiateFront = Mathf.Clamp(
                maxSegmentsInFront -
                (activeObjects.Count - activeSegmentIndex), // Ajusta para considerar segmentos já instanciados
                0,
                maxScenarioObjects - activeObjects.Count);
            DebugManager.Log<LevelBuilderManager>("Índice do segmento para Adicionar: " + segmentsToInstantiateFront);
            // Instanciar os segmentos à frente
            if (segmentsToInstantiateFront > 0)
            {
                AddNextSegment(segmentsToInstantiateFront);
            }
        }

        public void DestroyLevel()
        {
            RemoveSegments(0, activeObjects.Count);
            activeObjects = new List<ScenarioObjectData>();
        }


        // Função para remover os inimigos de forma segura
        private void CleanupEnemies(GameObject enemySetToRemove)
        {
            // Obter todos os scripts de comportamento dos inimigos
            // var enemyBehaviors = enemySetToRemove.GetComponentsInChildren<EnemyBehavior>();

            // Parar todos os comportamentos dos inimigos
            // foreach (var enemyBehavior in enemyBehaviors)
            // {
            //     enemyBehavior.StopBehavior();
            // }

            enemySetToRemove.SetActive(false);

            //Destruir os objetos dos inimigos
            foreach (Transform child in enemySetToRemove.transform)
            {
                Destroy(child.gameObject);
            }

            // **Comentário:** Essa lógica está comentada pois os scripts de comportamento dos inimigos ainda não foram implementados.
            // **Lembre-se:** Descomete o código quando os scripts de comportamento dos inimigos estiverem prontos.
        }
    }
}