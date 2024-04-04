using System.Collections.Generic;
using UnityEngine;
using ImmersiveGames.Utils;

namespace ImmersiveGames.LevelBuilder
{
    public class LevelBuilderManager : Singleton<LevelBuilderManager>
    {
        //public Transform spawnPoint;
        public int startNumOfSegments = 5;
        public int maxBehindActive = 2;
        public int maxSegmentsInFront = 2;
        public Vector3 offset;

        [SerializeField] private List<ScenarioObjectData> activeObjects = new List<ScenarioObjectData>();
        private GameObject _initialSegmentObject;
        private bool _initialSegmentInstantiated;
        
        private GameObject _setsContainer;
        [SerializeField] private LevelData levelData;

        private void Update()
        {
            if(GameManager.StateManager == null) return;
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddNextSegment(1);
            }

            if (Input.GetKeyDown(KeyCode.R) && activeObjects.Count > 0)
            {
                RemoveSegments(0, 1); // Remove o primeiro objeto do cenário
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var randomZPosition = Random.Range(0, activeObjects[^1].absolutePosition+100); // Altere os valores conforme necessário
                OptimizeSegments(randomZPosition);
            }
        }

        public void StartToBuild(LevelData data)
        {
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
            _initialSegmentObject = Instantiate(pathStart, spawnPosition, Quaternion.identity, _setsContainer.transform);
            var initialSegmentBounds = CalculateRealLength.GetBounds(_initialSegmentObject);
            var initialSegmentLength = initialSegmentBounds.size.z; // Acessar tamanho no eixo Z
            var initialSegmentPosition = spawnPosition - Vector3.forward * initialSegmentLength + offset;
            _initialSegmentObject.transform.position = initialSegmentPosition;
            _initialSegmentInstantiated = true;
        }

        private void AddNextSegment(int count)
        {
            if (count <= 0 || levelData.setLevelList.Count - activeObjects.Count < count)
            {
                Debug.LogWarning("Quantidade de segmentos a adicionar inválida.");
                return;
            }
            var spawnTransform = _setsContainer.transform;

            for (var i = 0; i < count; i++)
            {

                var index = activeObjects.Count; // Próximo índice é o tamanho atual da lista
                var segmentPrefab =
                    levelData.setLevelList[index]
                        .segmentObject; // Alteração 3: Usar o objeto de segmento do ScenarioObjectData
                var newSegment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, _setsContainer.transform);

                // Calcular o tamanho do segmento anterior (se houver)
                var previousSegmentLength = activeObjects.Count > 0
                    ? CalculateRealLength.GetBounds(activeObjects[^1].segmentObject).size.z
                    : 0;

                // Posicionar o segmento ao longo do eixo Z com offset
                Vector3 position;
                if (activeObjects.Count > 0)
                {
                    var previousSegment = activeObjects[^1].segmentObject;
                    var newSegmentPositionZ =
                        previousSegment.transform.position.z + previousSegmentLength + offset.z;

                    position = newSegment.transform.position;
                    position = new Vector3(position.x + offset.x, position.y + offset.y, newSegmentPositionZ);
                    newSegment.transform.position = position;
                }
                else
                {
                    // Posicionar o primeiro segmento com offset
                    position = newSegment.transform.position;
                    position = new Vector3(position.x + offset.x, position.y + offset.y,
                        spawnTransform.position.z + offset.z); // Adição: Posicionar no spawn point com offset
                    newSegment.transform.position = position;
                }

                GameObject enemySetInstance = null;
                // Verificar se o conjunto de inimigos existe
                if (levelData.setLevelList[index].enemySetObject != null)
                {
                    // Instanciar o conjunto de inimigos na mesma posição e rotação do segmento
                    enemySetInstance = Instantiate(levelData.setLevelList[index].enemySetObject,
                        newSegment.transform.position, newSegment.transform.rotation, _setsContainer.transform);
                    enemySetInstance.SetActive(true);
                }
                var newSegmentPosition = activeObjects.Count > 0
                    ? activeObjects[^1].absolutePosition + previousSegmentLength + offset.z
                    : spawnTransform.position.z + offset.z;
                
                // Adicionar à lista de objetos ativos
                var newScenarioData =
                    new ScenarioObjectData(newSegment, enemySetInstance, newSegmentPosition);
                activeObjects.Add(newScenarioData);
            }

            // Instanciar o segmento final no fim do último segmento
            if (activeObjects.Count >= levelData.setLevelList.Count && levelData.pathEnd != null && activeObjects.Count > 0)
            {
                FinalSegment(levelData.pathEnd);
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

        private void RemoveSegments(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= activeObjects.Count)
            {
                Debug.LogWarning("Índice inicial inválido para remoção de segmento.");
                return;
            }

            if (count <= 0 || startIndex + count > activeObjects.Count)
            {
                Debug.LogWarning("Quantidade de segmentos a remover inválida.");
                return;
            }

            // Percorrer a lista de trás para frente ao remover os segmentos
            for (var i = startIndex + count - 1; i >= startIndex; i--)
            {
                RemoveSegment(i);
            }
        }

        private void RemoveSegment(int index)
        {
            if (index < 0 || index >= activeObjects.Count)
            {
                Debug.LogWarning("Índice inválido para remoção de segmento.");
                return;
            }

            var segmentToRemove = activeObjects[index];
            var enemySetToRemove = segmentToRemove.enemySetObject;

            // Chamar CleanupEnemies antes de destruir o conjunto de inimigos
            if (enemySetToRemove != null)
            {
                CleanupEnemies(enemySetToRemove);
            }

            // Remover o objeto do cenário
            Destroy(segmentToRemove.segmentObject);

            // Remover da lista de objetos ativos
            activeObjects.RemoveAt(index);

            if (_initialSegmentInstantiated && _initialSegmentObject != null)
            {
                _initialSegmentInstantiated = false;
                Destroy(_initialSegmentObject);
                _initialSegmentObject = null;
            }
        }
        private ScenarioObjectData? FindSegmentAtPosition(float zPosition)
        {
            Debug.Log("Procurando segmento na posição Z: " + zPosition);

            ScenarioObjectData? closestData = null;
            var closestDistance = float.MaxValue;

            foreach (var data in activeObjects)
            {
                var distance = Mathf.Abs(data.absolutePosition - zPosition);
                Debug.Log("Segmento analisado: " + data.segmentObject.name + ", distância: " + distance);

                if (!(distance < closestDistance)) continue;
                closestData = data;
                closestDistance = distance;
            }

            Debug.Log("Segmento mais próximo encontrado: " + (closestData?.segmentObject.name ?? "nenhum"));

            return closestData;
        }

        
        public void OptimizeSegments(float position)
        {
            Debug.Log("Otimizando segmentos com base na posição Z: " + position);

            // Encontrar o segmento ativo
            var activeSegment = FindSegmentAtPosition(position);
            if (activeSegment == null)
            {
                Debug.Log("Segmento ativo encontrado: nenhum");
                return;
            }
            var activeSegmentIndex = activeObjects.IndexOf((ScenarioObjectData)activeSegment);

            
            Debug.Log("Índice do segmento ativo: " + activeSegmentIndex);
            
            // Desativar segmentos atrás do ativo
            var segmentsToRemoveCount = Mathf.Max(0, activeSegmentIndex - maxBehindActive);
            if (segmentsToRemoveCount > 0)
            {
                RemoveSegments(0, segmentsToRemoveCount);
            }
            Debug.Log("Índice do segmento para remover: " + segmentsToRemoveCount);

            // Calcular a quantidade de segmentos a serem instanciados à frente
            var maxScenarioObjects = maxBehindActive + maxSegmentsInFront + 1;
            var segmentsToInstantiateFront = Mathf.Clamp(
                maxSegmentsInFront - (activeObjects.Count - activeSegmentIndex), // Ajusta para considerar segmentos já instanciados
                0,
                maxScenarioObjects - activeObjects.Count);
            Debug.Log("Índice do segmento para Adicionar: " + segmentsToInstantiateFront);
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