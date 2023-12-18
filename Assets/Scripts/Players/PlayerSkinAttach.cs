using UnityEngine;
namespace RiverAttack
{
    public class PlayerSkinAttach : MonoBehaviour
    {
        //todos os prefabs de skin devem possuir esse componente
        //Apenas para identificar se o objeto é uma skin
        public void ToggleSkin(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}

