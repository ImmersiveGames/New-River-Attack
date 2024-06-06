﻿using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class MoveTiles : MonoBehaviour
    {
        public float tileMoveSpeed = 5f; // Ajuste a velocidade conforme necessário
        public float boundaryZ = -10f;   // Ajuste o ponto onde os tiles voltarão para o início

        private float _tileSizeZ;

        private void Start()
        {
            // Obtém o tamanho da malha do primeiro filho no eixo Z
            var meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            _tileSizeZ = meshRenderer.bounds.size.z;
        }

        private void Update()
        {
            // Move cada tile para baixo no eixo Z
            for (var i = 0; i < transform.childCount; i++)
            {
                var tile = transform.GetChild(i);
                tile.Translate(Vector3.back * (tileMoveSpeed * Time.deltaTime));

                // Verifica se o tile ultrapassou o limite definido
                if (tile.position.z < boundaryZ)
                {
                    // Reposiciona apenas o tile que ultrapassou o limite
                    ResetTilePosition(tile);
                }
            }
        }

        private void ResetTilePosition(Transform tile)
        {
            // Move o tile para o final da fila
            tile.Translate(Vector3.forward * (_tileSizeZ * (transform.childCount - 1)));
        }
    }
}