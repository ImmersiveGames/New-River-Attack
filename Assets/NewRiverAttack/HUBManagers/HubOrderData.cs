using NewRiverAttack.HUBManagers.UI;
using NewRiverAttack.LevelBuilder;

namespace NewRiverAttack.HUBManagers
{
    [System.Serializable]
    public struct HubOrderData
    {
        public UiHubIcons icon;
        public float position;
        public LevelData levelData;
        public UiHubBridges bridge;

        public HubOrderData(UiHubIcons hubIcon, float iconPosition, LevelData level, UiHubBridges bridges)
        {
            icon = hubIcon;
            position = iconPosition;
            levelData = level;
            bridge = bridges;
        }
    }
}