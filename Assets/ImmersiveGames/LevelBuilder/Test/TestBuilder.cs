using ImmersiveGames.Utils;
using UnityEngine;

namespace ImmersiveGames.LevelBuilder.Test
{
    public class TesteCalculadora : MonoBehaviour
    {
        public GameObject prefab; // Referência para o prefab

        private void Start()
        {
            if (prefab != null)
            {
                // Instância o prefab na cena
                GameObject objetoInstanciado = Instantiate(prefab);
            
                // Obtém os colliders do objeto instanciada
                Collider[] colliders = objetoInstanciado.GetComponentsInChildren<Collider>();
                Debug.Log("Quantos colliders?: " + colliders.Length);
                // Calcula o comprimento real do objeto montado com base nos colliders
                Bounds comprimentoTotal = CalculateRealLength.GetBounds(objetoInstanciado);

                // Exibe o comprimento real calculado
                Debug.Log("Comprimento real do objeto montado: " + comprimentoTotal.size.z);

                // Destrói o objeto instanciado após obter o comprimento
                Destroy(objetoInstanciado);
            }
            else
            {
                Debug.LogError("Prefab não atribuído ao objeto.");
            }
        }
    }
}