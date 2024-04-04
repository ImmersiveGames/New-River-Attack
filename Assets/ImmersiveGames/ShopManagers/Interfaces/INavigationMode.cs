using UnityEngine;

namespace ImmersiveGames.ShopManagers.Interfaces
{
    public interface INavigationMode
    {
        int SelectedItemIndex { get; }
        void MoveContent(RectTransform content, bool forward, MonoBehaviour monoBehaviour = null);
        void UpdateSelectedItem(RectTransform content, int selectedIndex);
        int CalculateSelectedItemIndex(RectTransform content, bool forward);

    }
}