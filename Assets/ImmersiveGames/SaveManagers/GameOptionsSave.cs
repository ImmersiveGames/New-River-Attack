using System;
using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.ShopProducts;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.Localization;
using Object = UnityEngine.Object;
using ShopProduct = ImmersiveGames.ShopManagers.Abstracts.ShopProduct;

namespace ImmersiveGames.SaveManagers
{
    [CreateAssetMenu(fileName = "GameOptionsSave", menuName = "ImmersiveGames/GameOptionsSave", order = 1)]
    public class GameOptionsSave : SingletonScriptable<GameOptionsSave>
    {
        [Header("Options Localization")]
        public Locale startLocale;
        
        [Header("Options Sound And Music")]
        public float bgmVolume;
        public float sfxVolume;
        
        [Header("Options Graphics")]
        public int frameRate;
        public Vector2Int actualResolution;
        public int selectedQualityIndex;

        [Header("Shopping")]
        public int wallet;
        public ShopProductSkin actualSkin;
        //[HideInInspector]
        public List<ProductStock> listPlayerProductStocks;

        public void UpdateWallet(int price)
        {
            wallet += price;
        }

        private int FindIndex(Object product)
        {
            for (var i = 0; i < listPlayerProductStocks.Count; i++)
            {
                if (listPlayerProductStocks[i].shopProduct == product)
                {
                    return i; // Retorna o índice se o produto for encontrado
                }
            }
            return -1; // Retorna -1 se o produto não for encontrado
        }

        public void AddInventory(ShopProduct product, int quantity)
        {
            var stockIndex = FindIndex(product);
            if (stockIndex != -1)
            {
                // Encontrou o produto no estoque
                var updatedStock = listPlayerProductStocks[stockIndex];
                updatedStock.quantityInStock += quantity;
                listPlayerProductStocks[stockIndex] = updatedStock;
                DebugManager.LogWarning("Product is found in inventory, Update Quantity.");
            }
            else
            {
                var productStock = new ProductStock
                {
                    shopProduct = product,
                    quantityInStock = quantity
                };
                listPlayerProductStocks.Add(productStock);
                // Produto não encontrado no estoque
                DebugManager.Log("Product not found in inventory, adicionando um novo.");
            }
        }

        #region Options Settings
        
        public float GetVolumeLog10(AudioMixGroup type, float volumeDefault = 1f)
        {
            var volume = GetVolume(type, volumeDefault);
            return AudioUtils.SoundBase10(volume);
        }
        public float GetVolume(AudioMixGroup type, float volumeDefault = 1f)
        {
            return type switch
            {
                AudioMixGroup.BgmVolume => bgmVolume > 0.0f ? bgmVolume : volumeDefault,
                AudioMixGroup.SfxVolume => sfxVolume > 0.0f ? sfxVolume : volumeDefault,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        // Define o volume com base no tipo de volume.
        public void SetVolume(AudioMixGroup type, float volume)
        {
            switch (type)
            {
                case AudioMixGroup.BgmVolume:
                    bgmVolume = volume;
                    break;
                case AudioMixGroup.SfxVolume:
                    sfxVolume = volume;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
        
    }
}