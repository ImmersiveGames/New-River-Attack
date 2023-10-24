using System;
using System.Collections;
using System.Collections.Generic;
using RiverAttack;
using UnityEngine;
using Utils;

public class GameHubManager : Singleton<GameHubManager>
{
    ListLevels m_MissionLevels;
    [SerializeField] Levels hudLevels;
    #region UNITYMETHODS
    void Awake()
    {
        m_MissionLevels = GameManager.instance.missionLevels;
    }

    void Start()
    {
        
    }
    void OnDisable()
    {
        
    }
  #endregion
    
}
