using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialog", menuName = "RiverAttack/Dialog", order = 102)]
[System.Serializable]

public class DialogObject : ScriptableObject
{
    [SerializeField] public string[] dialogSentences_PT_BR;
    [SerializeField] public string[] dialogSentences_EN;
}
