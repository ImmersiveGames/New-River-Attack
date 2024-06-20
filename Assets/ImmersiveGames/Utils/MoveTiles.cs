using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class MoveTiles : MonoBehaviour
    {
        private bool _activeTile;
        private Vector2 _textureOffset;
        private Material _material;
        private PlayerMaster _playerMaster;


        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _textureOffset = _material.mainTextureOffset;
        }

        private void Update()
        {
            if (!_activeTile) return;
            var movement = new Vector2(0,-1 * _playerMaster.ActualSkin.playerSpeed/7);
            // Aplicar o movimento ao offset da textura
            _textureOffset += movement * Time.deltaTime;
            // Atualizar o offset da textura do material
            _material.mainTextureOffset = _textureOffset;
        }

        private void OnDisable()
        {
            _playerMaster = null;
        }
        
        public void ActiveTiles(PlayerMaster playerMaster)
        {
            _playerMaster = playerMaster;
            _activeTile = true;
        }
    }
}