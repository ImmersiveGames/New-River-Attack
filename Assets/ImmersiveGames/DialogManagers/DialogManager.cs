using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ImmersiveGames.DialogManagers
{
    public class DialogManager: MonoBehaviour
    {
        [SerializeField] private TMP_Text dialogText;
        [SerializeField] private List<DialogData> dialogDatas;
    }
}