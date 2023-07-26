using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack 
{
    public class GasDisplay : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
        [SerializeField] Image gasBarImage;
 
        [SerializeField] Color highGasColor;        
        [SerializeField] Color mediumGasColor;        
        [SerializeField] Color lowGasColor;

        float mediumGasValue = 0.6f;
        float lowGasValue = 0.2f;

        void Update()
        {
            UpdateDisplay();
        }

        void UpdateDisplay() 
        {
            float gasAmount = ((float)playerSettings.actualHp) / 100;
            gasBarImage.fillAmount = gasAmount ;

            if (gasAmount < mediumGasValue && gasAmount > lowGasValue) gasBarImage.color = mediumGasColor;
            else if (gasAmount <= lowGasValue) gasBarImage.color = lowGasColor;
            else gasBarImage.color = highGasColor;
        }
    }
}

