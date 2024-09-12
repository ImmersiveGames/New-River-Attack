using ImmersiveGames.AudioEvents;
using NewRiverAttack.AudioManagers;

namespace ImmersiveGames
{
    public partial class AudioManager
    {
        public AudioEvent GetAudioSfxEvent(EnumSfxSound sfxSound)
        {
            return GetAudioEventForState(sfxSound.ToString(), _mapMenuSfx);
        }

        public void PlayMouseClick()
        {
            PlaySfx(EnumSfxSound.SfxMouseClick.ToString());
        }

        public void PlayNotifications()
        {
            PlaySfx(EnumSfxSound.SfxNotification.ToString());
        }

        public void PlayMouseOver()
        {
            PlaySfx(EnumSfxSound.SfxMouseOver.ToString());
        }
    }
}