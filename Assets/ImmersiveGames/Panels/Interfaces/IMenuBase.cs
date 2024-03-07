namespace ImmersiveGames.Panels.Interfaces
{
    public interface IMenuBase
    {
        bool isOpen { get; set; }
        bool isSelectable { get; set; }
        bool isNavigable { get; set; }

        void Open();
        void Close();
        void OnSelect();
        void OnDeselect();
        void OnNavigateUp();
        void OnNavigateDown();
    } 
}

