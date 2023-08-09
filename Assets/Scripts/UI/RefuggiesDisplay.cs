using TMPro;
using UnityEngine;

namespace RiverAttack 
{
    public class RefuggiesDisplay : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
        [SerializeField] TMP_Text refuggiesText;

        void OnEnable()
        {
    
        }

        void OnDisable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateRefuggieDisplay();
        }

        void UpdateRefuggieDisplay() 
        {
            refuggiesText.text = playerSettings.wealth.ToString();
        }
    }
}

