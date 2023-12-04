using UnityEngine;

public class OceanMovement : MonoBehaviour
{
    public float tileMoveSpeed = 5f; // Ajuste a velocidade conforme necessário
    public float boundaryZ = -10f; // Ajuste o ponto onde os tiles voltarão para o início

    private float tileSizeZ;

    void Start()
    {
        // Obtém o tamanho da malha do primeiro filho no eixo Z
        MeshRenderer meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        tileSizeZ = meshRenderer.bounds.size.z;
    }

    void Update()
    {
        // Move cada tile para baixo no eixo Z
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tile = transform.GetChild(i);
            tile.Translate(Vector3.back * tileMoveSpeed * Time.deltaTime);

            // Verifica se o tile ultrapassou o limite definido
            if (tile.position.z < boundaryZ)
            {
                // Reposiciona apenas o tile que ultrapassou o limite
                ResetTilePosition(tile);
            }
        }
    }

    void ResetTilePosition(Transform tile)
    {
        // Move o tile para o final da fila
        tile.Translate(Vector3.forward * tileSizeZ * (transform.childCount - 1));
    }
}
