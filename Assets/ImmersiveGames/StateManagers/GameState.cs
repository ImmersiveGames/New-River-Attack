﻿using System.Threading.Tasks;
using ImmersiveGames.ScenesManager.Transitions;
using ImmersiveGames.StateManagers.Interfaces;
using UnityEngine.SceneManagement;

namespace ImmersiveGames.StateManagers
{
    public abstract class GameState : IState
    {
        protected GameState(string currentStateName)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            StateName = currentStateName;
        }
        #region IState Interface
        
        public string StateName { get;}
        public bool StateInitialized { get; set; }

        private bool _stateFinalization;
        public bool StateFinalization { get; set; }

        public async Task EnterAsync(IState previousState)
        {
            if (!StateInitialized)
            {
                StateInitialized = true;
            }

            var transitionOut = OutTransition;

            if (transitionOut != null)
            {
                await transitionOut.OutTransitionAsync().ConfigureAwait(false);
            }
            
            await OnEnter(previousState).ConfigureAwait(false);
            StateFinalization = true;
        }

        public abstract void UpdateState();

        public async Task ExitAsync()
        {
            StateFinalization = false;
            await OnExit().ConfigureAwait(false);
            
            var transitionIn = InTransition;
            if (transitionIn != null)
            {
                await transitionIn.InTransitionAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #region Scene Change

        public abstract bool RequiresSceneLoad { get; }
        public abstract string SceneName  { get; }
        public abstract LoadSceneMode LoadMode { get; }
        public abstract bool UnLoadAdditiveScene { get; }

        #endregion

        #region Transitions

        public abstract ITransition InTransition { get; }
        public abstract ITransition OutTransition { get; }
        #endregion
        
        
        #region Enter and Exit Methods

        protected virtual async Task OnEnter(IState previousState)
        {
            // Custom logic to be executed on entering the state
            
            await Task.Yield();
        }

        protected virtual async Task OnExit()
        {
            // Custom logic to be executed on exiting the state
            await Task.Yield();
        }

        #endregion
    }

    public enum StatesNames
    {
        GameStateBriefingRoom,
        GameStateMenuInicial,
        GameStateOpenGame,
        GameStatePause,
        GameStatePlay
    }
}