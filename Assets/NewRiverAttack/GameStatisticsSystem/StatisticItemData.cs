using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace NewRiverAttack.GameStatisticsSystem
{
    [Serializable]
    public class StatisticItemData
    {
        public EnumGameStatistics itemReference;
        public LocalizedString itemLocalizedString;
        [HideInInspector]
        public string itemString;
        [HideInInspector]
        public string itemValue;
        public IEnumerable<StatisticItemData> ItemStatisticsList;
    }
}