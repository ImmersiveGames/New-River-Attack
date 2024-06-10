using System;
using NewRiverAttack.GameManagers;
using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using UnityEngine;

namespace ImmersiveGames.Utils
{
    public class MoveTiles : MonoBehaviour
    {
        private GamePlayManager _gamePlayManager;
        private bool _activeTile;
        private Vector2 _textureOffset;
        private Material _material;
        private PlayerMaster _playerMaster;
        

        private void Start()
        {
            _material = GetComponent<Renderer>().material;
            _textureOffset = _material.mainTextureOffset;
        }
        private void OnEnable()
        {
            SetInitialReferences();
        }

        private void Update()
        {
            if (_activeTile){
                var movement = new Vector2(0,-1 * _playerMaster.ActualSkin.playerSpeed/5);
                // Aplicar o movimento ao offset da textura
                _textureOffset += movement * Time.deltaTime;
                //Debug.Log($"Speed Tile {movement}");
                // Atualizar o offset da textura do material
                _material.mainTextureOffset = _textureOffset;
                
            }
        }

        private void OnDisable()
        {
            _playerMaster = null;
        }

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
        }
        
        public void ActiveTiles(PlayerMaster playerMaster)
        {
            _playerMaster = playerMaster;
            _activeTile = true;
        }
    }
}