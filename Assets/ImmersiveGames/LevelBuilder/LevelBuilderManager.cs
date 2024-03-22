using System.Collections.Generic;
using UnityEngine;
using ImmersiveGames.Utils;

namespace ImmersiveGames.LevelBuilder
{
    public class LevelBuilderManager : MonoBehaviour
    {
        public ScenarioObjectData[] scenarioObjects;
        public GameObject initialSegmentPrefab;
        public GameObject finalSegmentPrefab;
        public Transform spawnPoint;
        public int startNumOfSegments = 5;
        public Vector3 offset;

        [SerializeField] private List<ScenarioObjectData> activeObjects = new List<ScenarioObjectData>();
        private GameObject _initialSegmentObject;
        private bool _initialSegmentInstantiated;
        
        private GameObject _setsContainer;

        private void Start()
        {
            InitialSegment();
            
            // Criar container para os sets
            _setsContainer = new GameObject("Sets Container")
            {
                transform =
                {
                    parent = transform
                }
            };

            // Instanciar os objetos do cenário
            AddNextSegment(startNumOfSegments);
        }

        private void Update()
        {
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
                var randomZPosition = Random.Range(0, 1000); // Altere os valores conforme necessário
                var segmentAtPosition = FindSegmentAtPosition(randomZPosition);
                Debug.Log(segmentAtPosition != null
                    ? $"Posição Z procurada: {randomZPosition}, Objeto encontrado: {segmentAtPosition.name}"
                    : $"Posição Z procurada: {randomZPosition}, Nenhum objeto encontrado.");
            }
        }

        private void InitialSegment()
        {
            var spawnPosition = spawnPoint.position;

            // Instanciar o segmento inicial se ainda não foi instanciado
            if (_initialSegmentInstantiated || initialSegmentPrefab == null) return;
            _initialSegmentObject = Instantiate(initialSegmentPrefab, spawnPosition, Quaternion.identity);
            var initialSegmentBounds = CalculateRealLength.GetBounds(_initialSegmentObject);
            var initialSegmentLength = initialSegmentBounds.size.z; // Acessar tamanho no eixo Z
            var initialSegmentPosition = spawnPosition - Vector3.forward * initialSegmentLength + offset;
            _initialSegmentObject.transform.position = initialSegmentPosition;
            _initialSegmentInstantiated = true;
        }

        private void AddNextSegment(int count)
        {
            if (count <= 0 || scenarioObjects.Length - activeObjects.Count < count)
            {
                Debug.LogWarning("Quantidade de segmentos a adicionar inválida.");
                return;
            }

            for (var i = 0; i < count; i++)
            {

                var index = activeObjects.Count; // Próximo índice é o tamanho atual da lista
                var segmentPrefab =
                    scenarioObjects[index]
                        .segmentObject; // Alteração 3: Usar o objeto de segmento do ScenarioObjectData
                var newSegment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, transform);

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
                        spawnPoint.position.z + offset.z); // Adição: Posicionar no spawn point com offset
                    newSegment.transform.position = position;
                }

                GameObject enemySetInstance = null;
                // Verificar se o conjunto de inimigos existe
                if (scenarioObjects[index].enemySetObject != null)
                {
                    // Instanciar o conjunto de inimigos na mesma posição e rotação do segmento
                    enemySetInstance = Instantiate(scenarioObjects[index].enemySetObject,
                        newSegment.transform.position, newSegment.transform.rotation, _setsContainer.transform);
                    enemySetInstance.SetActive(true);
                }
                var newSegmentPosition = activeObjects.Count > 0
                    ? activeObjects[^1].absolutePosition + previousSegmentLength + offset.z
                    : spawnPoint.position.z + offset.z;
                
                // Adicionar à lista de objetos ativos
                var newScenarioData =
                    new ScenarioObjectData(newSegment, enemySetInstance, newSegmentPosition);
                activeObjects.Add(newScenarioData);
            }

            // Instanciar o segmento final no fim do último segmento
            if (activeObjects.Count >= scenarioObjects.Length && finalSegmentPrefab != null && activeObjects.Count > 0)
            {
                FinalSegment();
            }
        }


        private void FinalSegment()
        {
            var lastSegment = activeObjects[^1].segmentObject;
            var lastSegmentBounds = CalculateRealLength.GetBounds(lastSegment);
            var lastSegmentLength = lastSegmentBounds.size.z;
            var finalSegmentInstance = Instantiate(finalSegmentPrefab,
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

            for (int i = startIndex; i < startIndex + count; i++)
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
        }


        private GameObject FindSegmentAtPosition(int zPosition)
        {
            GameObject closestSegment = null;
            var closestDistance = float.MaxValue;

            foreach (var data in activeObjects)
            {
                var distance = Mathf.Abs(data.absolutePosition - zPosition);
                if (!(distance < closestDistance)) continue;
                closestSegment = data.segmentObject;
                closestDistance = distance;
            }

            return closestSegment;
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
            // **Lembre-se:** Descomente o código quando os scripts de comportamento dos inimigos estiverem prontos.
        }
    }
}