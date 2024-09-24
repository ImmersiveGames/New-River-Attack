using System;
using CarterGames.Assets.SaveManager;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GamePlayManagers.GamePlayLogs;
using Saves;
using UnityEngine;

namespace NewRiverAttack.SaveManagers
{
    public class GameSaveHandler : MonoBehaviour
    {
        [SerializeField] private GemeStatisticsDataLog dataLog;
        [SerializeField] private GameOptionsSave gameOptionsSave;
        [SerializeField] private PlayerSaveSaveObject saveObject;
        
        private readonly DateTime _lastDate = new DateTime(2024, 9, 20); 
        
        public static GameSaveHandler Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
                DebugManager.Log<GameSaveHandler>("Instância criada.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GameSaveHandler>("Instância duplicada destruída.");
            }
        }
        
        private void OnEnable()
        {
            saveObject = SaveManager.GetSaveObject<PlayerSaveSaveObject>();
            LoadGameData();
        }

        private void Start()
        {
            LoadGameLocation();

            // Verifica se o último save foi feito antes da data limite
            if (saveObject.lastDate != null && saveObject.lastDate.Value < _lastDate)
            {
                DebugManager.Log<GameSaveHandler>("Resetando dados, save anterior à data limite.");
                ResetFiles();
            }
        }

        private void OnDisable()
        {
            SaveGameData();
        }

        private void ResetFiles()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();  // Salva a exclusão dos dados no PlayerPrefs
            saveObject.ResetObjectSaveValues();
            SaveManager.Save();  // Salva o novo estado do saveObject
            
            // Limpa os logs também
            dataLog.ResetLogs();
        }

        private void OnApplicationQuit()
        {
            SaveGameData();
        }

        private void LoadGameLocation()
        {
            if (saveObject.startLocale != null)
            {
                gameOptionsSave.startLocale = saveObject.startLocale.Value;
                DebugManager.Log<GameSaveHandler>($"Locale carregado: {gameOptionsSave.startLocale}");
            }
        }
        
        private void SaveGameLocation()
        {
            if (gameOptionsSave.startLocale != null)
            {
                saveObject.startLocale.Value = gameOptionsSave.startLocale;
                DebugManager.Log<GameSaveHandler>($"Locale salvo: {saveObject.startLocale.Value}");
            }
        }

        private void LoadGameData()
        {
            saveObject.Load();  // Carrega os dados do SaveObject
            
            // Exemplo para carregar opções do jogo
            if (saveObject.bgmVolume.Value != 0)
                gameOptionsSave.bgmVolume = saveObject.bgmVolume.Value;
            // Continue carregando os outros valores de save...
        }

        public void SaveGameData()
        {
            if (saveObject == null) return;

            SaveGameLocation();

            // Exemplo para salvar uma opção
            if (!Mathf.Approximately(saveObject.bgmVolume.Value, gameOptionsSave.bgmVolume))
                saveObject.bgmVolume.Value = gameOptionsSave.bgmVolume;
            
            // Atualiza a data do último save
            saveObject.lastDate.Value = DateTime.Now;

            // Salva o estado atualizado
            saveObject.Save();
            SaveManager.Save();
        }
    }
}
