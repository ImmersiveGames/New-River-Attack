using System;
using UnityEngine;
using UnityEngine.UI;
namespace RiverAttack
{
    
    /// <summary>
    /// Logica do HUB.
    /// primeiro ele precisa setar a --ultima configuração--, o que siginifica que de alguma forma eu devo saber qual foi o ultimo --index-- que o jogador parou,
    /// se não tiver o index é 0, todos os icones antes deles devem estar aberto (ja setados no SO Levels) e todos os a frente setados como locked (ja setados
    /// no SO Level) ==OK==
    ///
    /// se o jogador navegar para missões com index menores que a atual ele deve setar --temporariamente-- o seu estatus como --atual-- (sem alterar sua
    /// propriedade no SO Levels).
    ///
    /// Se o jogador --Tentar-- avançar para um index maior que o atual, o boão de avançar deve impedir. Assim como o botão de começar se o index não estiver
    /// com estaus --atual-- deve ser impedido de acessar a fase.
    ///
    /// Ao retornar ao menu HUB --OnEnable-- verificar se a fase foi encerrada, se não, nada muda.
    ///
    /// Se encerrou a fase o icone deve mudar para --Completo-- disparar uma --animação-- de destruir ponte, enviar para o proximo index que ele esta --Aberto--
    /// ou seja mudar o estatus da proxima missão.
    /// </summary>
    public class UiHubIcons: MonoBehaviour
    {
        GameHubManager m_GameHubManager;
        [SerializeField] internal Levels level;
        [SerializeField] internal int myIndex;
        Image m_MissionIcon;
        
        void Awake()
        {
            m_GameHubManager = GameHubManager.instance;
            m_MissionIcon = GetComponentInChildren<Image>();
        }
        
        void OnEnable()
        {
            m_GameHubManager.MissionIndex += SetupMission;
        }
        void Start()
        {
            SetupMission(GamePlayingLog.instance.lastMissionIndex);
        }
        

        void OnDisable()
        {
            m_GameHubManager.MissionIndex -= SetupMission;
        }

        void SetupMission(int indexMission)
        {
            if (indexMission == myIndex)
            {
                if (level.levelsStates != LevelsStates.Complete)
                {
                    m_MissionIcon.color = m_GameHubManager.SetColorStates(LevelsStates.Actual);
                    level.levelsStates = LevelsStates.Actual;
                }
                else
                {
                    m_MissionIcon.color = m_GameHubManager.SetColorStates(LevelsStates.Complete);
                }
                return;
            }
            m_MissionIcon.color = m_GameHubManager.SetColorStates(LevelsStates.Open);
            level.levelsStates = LevelsStates.Open;
            if (GamePlayingLog.instance.lastMissionFinishIndex >= myIndex)
                return;
            m_MissionIcon.color = m_GameHubManager.SetColorStates(LevelsStates.Locked);
            level.levelsStates = LevelsStates.Locked;
        }
    }
}
