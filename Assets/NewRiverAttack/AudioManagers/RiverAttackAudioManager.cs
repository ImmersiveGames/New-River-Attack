using ImmersiveGames.AudioEvents;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.LevelBuilder;
using NewRiverAttack.AudioManagers;
using NewRiverAttack.LevelBuilder;

namespace ImmersiveGames
{
    public partial class AudioManager
    {
        public static AudioEvent GetAudioSfxEvent(EnumSfxSound sfxSound)
        {
            return GetAudioEventForState(sfxSound.ToString(), _mapMenuSfx);
        }
        /*public static AudioEvent GetAudioBGMEvent(EnumBgmTypes bgmTypes)
        {
            return GetAudioEventForState(bgmTypes.ToString(), _mapStateBgm);
        }*/
        public static void PlayMouseClick()
        {
            PlayOneShot(EnumSfxSound.SfxMouseClick.ToString());
        }
        public static void PlayNotifications()
        {
            PlayOneShot(EnumSfxSound.SfxNotification.ToString());
        }
        public static void PlayMouseOver()
        {
            PlayOneShot(EnumSfxSound.SfxMouseOver.ToString());
        }
        /*public void PlayBGM(LevelData levelData, int levelIndexBgm)
        {
            DebugManager.Log<AudioManager>($"Procurando: {levelData.levelType}");
            
            var audioEventForLevel = levelData.levelType != LevelTypes.Multi ?
                GetAudioEventForState(levelData.levelType.ToString(), _mapStateBgm):
                GetAudioEventForState(levelData.startBgm.ToString(), _mapStateBgm);
            if (audioEventForLevel == null)
            {
                DebugManager.Log<AudioManager>($"Não Encontrou um audio relativo ao Tipo: {levelData.levelType.ToString()}");
            }
            DebugManager.Log<AudioManager>($"AudioEvent: {audioEventForLevel}");
            audioEventForLevel.Play(_bgmAudioSource, this, fadeSoundDuration);
        }*/
    }
}