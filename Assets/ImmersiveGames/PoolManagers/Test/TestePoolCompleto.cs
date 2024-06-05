using ImmersiveGames.BulletsManagers;
using ImmersiveGames.PoolManagers;
using ImmersiveGames.PoolManagers.Interface;
using UnityEngine;

public class TestePoolCompleto : MonoBehaviour
{
    public GameObject prefabDoObjeto; // Prefab do objeto que será colocado no pool
    public int tamanhoInicialDoPool = 10; // Tamanho inicial do pool
    public Transform pontoDeSpawn; // Ponto onde o objeto será colocado no jogo

    private PoolObjectManager poolManager; // Referência ao gerenciador de pool
    private bool podeUsarObjeto = true; // Controle para evitar uso contínuo de objetos do pool

    void Start()
    {
        // Obtém uma referência ao PoolObjectManager na cena
        poolManager = new PoolObjectManager();

        // Verifica se o PoolObjectManager foi encontrado
        if (poolManager == null)
        {
            Debug.LogError("PoolObjectManager não encontrado na cena!");
            return;
        }

        // Cria o pool de objetos com o prefab especificado e o tamanho inicial
        bool poolCriado = poolManager.CreatePool("TestePool", prefabDoObjeto, tamanhoInicialDoPool, transform);
        if (poolCriado)
        {
            Debug.Log("Pool de objetos criado com sucesso!");
        }
        else
        {
            Debug.LogError("Erro ao criar o pool de objetos!");
        }
    }

    void Update()
    {
        // Verifica se o jogador pressionou a tecla Espaço e se pode usar um objeto do pool
        if (Input.GetKeyDown(KeyCode.Space) && podeUsarObjeto)
        {
            Debug.Log("Pressionou a tecla Espaço para obter objeto do pool.");

            // Obtém um objeto do pool
            GameObject objetoDoPool = poolManager.GetObjectFromPool<IPoolable>("TestePool", pontoDeSpawn, new BulletData());

            // Verifica se o objeto foi obtido com sucesso
            if (objetoDoPool != null)
            {
                Debug.Log("Objeto do pool obtido com sucesso!");

                // Coloca o objeto na posição do ponto de spawn e ativa
                objetoDoPool.transform.position = pontoDeSpawn.position;
                objetoDoPool.SetActive(true);

                // Impede o uso contínuo de objetos do pool
                podeUsarObjeto = false;

                // Inicia um temporizador para permitir o uso posterior de objetos do pool
                Invoke("LiberarObjeto", 1.0f);
            }
            else
            {
                Debug.LogError("Objeto do pool não foi obtido!");
            }
        }
    }

    // Método para liberar o uso de objetos do pool após um certo tempo
    void LiberarObjeto()
    {
        podeUsarObjeto = true;
    }
}
