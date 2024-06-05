using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace NewRiverAttack.DialogManagers
{
    [Serializable]
    public struct DialogData
    {
        public List<LocalizedString> sentences;
        public float startTimeSentences;
        public LocalizedString nameSpeaker;
    }
}