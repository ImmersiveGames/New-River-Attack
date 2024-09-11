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

        /* Uncomment and refactor when needed
        public void PlayBGM(LevelData levelData, int levelIndexBgm)
        {
            var audioEventForLevel = levelData.levelType != LevelTypes.Multi ?
                GetAudioEventForState(levelData.levelType.ToString(), _mapStateBgm) :
                GetAudioEventForState(levelData.startBgm.ToString(), _mapStateBgm);

            if (audioEventForLevel == null)
            {
                DebugManager.Log<AudioManager>($"Audio not found for type: {levelData.levelType}");
                return;
            }
            audioEventForLevel.Play(bgmAudioSource, this, fadeSoundDuration);
        }*/
    }
}