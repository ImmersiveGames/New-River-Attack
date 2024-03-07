using UnityEngine;
using UnityEngine.Playables;

namespace ImmersiveGames.Panels
{
    public class PanelPrincipal : PanelBase
    {
        [SerializeField] 
        private PlayableDirector playableDirector;
        private TimelineManager.TimelineManager _timelineManager;

        #region Unity_Methods

        protected override void Awake()
        {
            _timelineManager = new TimelineManager.TimelineManager(playableDirector);
            base.Awake();
        }

        private void OnDestroy()
        {
            _timelineManager = null;
        }

        #endregion

        // Remova o Start se não for necessário

        // Remova ou faça implementações apropriadas para os métodos não utilizados
        public override void Close()
        {
            Interact();
        }

        public override void OnSelect()
        {
            // Implementação, se necessário
        }

        public override void OnDeselect()
        {
            // Implementação, se necessário
        }

        public override void OnNavigateUp()
        {
            // Implementação, se necessário
        }

        public override void OnNavigateDown()
        {
            // Implementação, se necessário
        }

        // Mantenha a propriedade apenas leitura se não precisa ser definida externamente
        public override bool isOpen { get; set; }
        public override bool isSelectable { get; set; } = true;
        public override bool isNavigable { get; set; } = true;

        public void TimelinePlayAnimation(float animationTimeStart)
        {
            _timelineManager?.PlayAnimation(animationTimeStart);
        }

        public void ButtonExit()
        {
            AudioManager.PlayMouseClick();
            Application.Quit();
        }
    }
}