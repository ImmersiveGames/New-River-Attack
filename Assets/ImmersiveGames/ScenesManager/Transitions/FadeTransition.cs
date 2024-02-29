using System.Threading.Tasks;
using UnityEngine;

namespace ImmersiveGames
{
    public class FadeTransition : ITransition
    {
        public async Task InTransitionAsync()
        {
            // Implement your fade-in or fade-out logic here
            Debug.Log("Performing fade IN transition");
            await FadeManager.instance.FadeInAsync().ConfigureAwait(false);
        }
        public async Task OutTransitionAsync()
        {
            // Implement your fade-in or fade-out logic here
            Debug.Log("Performing fade Out transition");
            await FadeManager.instance.FadeOutAsync().ConfigureAwait(false);
        }
    }
}