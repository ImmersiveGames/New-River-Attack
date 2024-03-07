using System.Collections.Generic;

namespace ImmersiveGames.InputManager
{
    public class ComboSequence
    {
        public string Name { get; private set; }
        public List<string> ActionNames { get; private set; }
        public float MaxTimeBetweenActions { get; private set; }

        public ComboSequence(string name, List<string> actionNames, float maxTimeBetweenActions)
        {
            Name = name;
            ActionNames = actionNames;
            MaxTimeBetweenActions = maxTimeBetweenActions;
        }

        public bool Matches(string currentAction, string previousAction)
        {
            if (ActionNames.Count != 2) return false;

            return ActionNames[0] == previousAction && ActionNames[1] == currentAction;
        }
    }
}