using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ImmersiveGames.Utils;
using RiverAttack;

namespace ImmersiveGames.LevelBuilder
{
    public class LevelBuilderManager : MonoBehaviour
    {
        public ScenarioObjectData[] scenarioObjects; // Alteração 1: Renomear e mudar para array readonly
        public GameObject initialSegmentPrefab;
        public GameObject finalSegmentPrefab;
        public Transform spawnPoint;
        public int maxScenarioObjects = 5;
        public Vector3 offset; // Adição: Offset para correção de posição

        [SerializeField] private List<ScenarioObjectData> activeObjects = new List<ScenarioObjectData>();
        private GameObject initialSegmentObject;
        private bool initialSegmentInstantiated;

        private void Start()
        {
            var spawnPosition = spawnPoint.position;

            // Instanciar o segmento inicial se ainda não foi instanciado
            if (!initialSegmentInstantiated && initialSegmentPrefab != null)
            {
                initialSegmentObject = Instantiate(initialSegmentPrefab, spawnPosition, Quaternion.identity);
                var initialSegmentLength =
                    CalculateRealLength.CalculateCollidersLength(
                        initialSegmentObject.GetComponentsInChildren<Collider>());
                var initialSegmentPosition = spawnPosition - Vector3.forward * initialSegmentLength + offset;
                initialSegmentObject.transform.position = initialSegmentPosition;
                initialSegmentInstantiated = true;
            }

            // Instanciar os objetos do cenário
            for (var i = 0; i < maxScenarioObjects; i++)
            {
                AddNextSegment();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                AddNextSegment();
            }

            if (Input.GetKeyDown(KeyCode.R) && activeObjects.Count > 0)
            {
                RemoveSegments(0); // Remove o primeiro objeto do cenário
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

        private void AddNextSegment()
        {
            if (activeObjects.Count >= scenarioObjects.Length) // Alteração 2: Usar o array scenarioObjects
            {
                Debug.LogWarning("Não é possível adicionar mais objetos ao cenário. Limite máximo atingido.");
                return;
            }

            var index = activeObjects.Count; // Próximo índice é o tamanho atual da lista
            var segmentPrefab =
                scenarioObjects[index].segmentObject; // Alteração 3: Usar o objeto de segmento do ScenarioObjectData
            var newSegment = Instantiate(segmentPrefab, Vector3.zero, Quaternion.identity, transform);

            // Obter os WallsMaster dentro do novo objeto de segmento
            var wallsMasters = newSegment.GetComponentsInChildren<WallsMaster>();

            var previousSegmentLength = (from wallsMaster in wallsMasters
                where wallsMaster != null
                select wallsMaster.GetComponentsInChildren<Collider>()
                into colliders
                select CalculateRealLength.CalculateCollidersLength(colliders)).Sum();

            // Posicionar o segmento ao longo do eixo Z com offset
            Vector3 position;
            if (activeObjects.Count > 0)
            {
                var previousSegment = activeObjects[^1].segmentObject;
                var newSegmentPositionZ =
                    previousSegment.transform.position.z + previousSegmentLength +
                    offset.z; // Adição: Adicionar offset na posição Z
                position = newSegment.transform.position;
                position = new Vector3(position.x + offset.x, position.y + offset.y, newSegmentPositionZ);
                newSegment.transform.position = position;
            }
            else
            {
                // Posicionar o primeiro segmento com offset
                position = newSegment.transform.position;
                position = new Vector3(position.x + offset.x, position.y + offset.y,
                    spawnPoint.position.z + offset.z); // Adição: Adicionar offset na posição Z
                newSegment.transform.position = position;
            }

            // Adicionar à lista de objetos ativos
            var newScenarioData = new ScenarioObjectData(newSegment, null, newSegment.transform.position.z);
            activeObjects.Add(newScenarioData);

            // Instanciar o segmento final no fim do último segmento
            if (index == scenarioObjects.Length - 1 && finalSegmentPrefab != null && activeObjects.Count > 0)
            {
                var lastSegment = activeObjects[^1].segmentObject;
                var lastSegmentLength =
                    CalculateRealLength.CalculateCollidersLength(
                        lastSegment.GetComponentsInChildren<Collider>());
                Debug.Log($"Distancia: {lastSegment.transform.position.z}, {lastSegmentLength}" );
                var finalSegmentInstance = Instantiate(finalSegmentPrefab,
                    lastSegment.transform.position + Vector3.forward * lastSegmentLength + offset,
                    Quaternion.identity);
            }
        }

        private void RemoveSegments(int index)
        {
            if (index < 0 || index >= activeObjects.Count)
            {
                Debug.LogWarning("Índice inválido para remover objeto do cenário.");
                return;
            }

            var scenarioData = activeObjects[index];
            scenarioData.RemoveObjectScene();
            activeObjects.RemoveAt(index);

            // Verificar se o objeto inicial ainda está na cena e removê-lo se estiver
            if (initialSegmentObject != null &&
                !activeObjects.Exists(data => data.segmentObject == initialSegmentObject))
            {
                Destroy(initialSegmentObject);
                initialSegmentInstantiated = false;
            }
        }

        private GameObject FindSegmentAtPosition(int zPosition)
        {
            GameObject closestSegment = null;
            var closestDistance = float.MaxValue;

            foreach (var data in activeObjects)
            {
                var distance = Mathf.Abs(data.spawnDistance - zPosition);
                if (!(distance < closestDistance)) continue;
                closestSegment = data.segmentObject;
                closestDistance = distance;
            }

            return closestSegment;
        }
    }
}
