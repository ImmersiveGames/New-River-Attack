using System.Globalization;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.GameStatisticsSystem
{
    public class ItemCardDisplayHolder : MonoBehaviour
    {
        [SerializeField] public TMP_Text itemNameText;
        [SerializeField] public TMP_Text itemValueText;

        public void Init(string playerName, int score, int rank)
        {
            itemNameText.text = $"{rank} - {playerName}";
            itemValueText.text = score.ToString();
        }

        private static string FormatRank(int rank, CultureInfo cultureInfo)
        {
            // Adiciona a ordem correspondente ao número do rank
            if (rank % 100 >= 11 && rank % 100 <= 13)
            {
                return $"{rank}th";
            }
            return (rank % 10) switch
            {
                1 => $"{rank}st",
                2 => $"{rank}nd",
                3 => $"{rank}rd",
                _ => $"{rank}th"
            };
        }
    }
}

