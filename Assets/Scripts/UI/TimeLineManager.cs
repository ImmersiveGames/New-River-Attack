using UnityEngine;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    PlayableDirector m_PlayableDirector;

    void Start()
    {
        m_PlayableDirector = GetComponent<PlayableDirector>();
    }

    public void PlayAnimation(float startTimer)
    {
        if (m_PlayableDirector.state == PlayState.Playing)
        {
            m_PlayableDirector.Stop();
        }
        m_PlayableDirector.time = startTimer;
        m_PlayableDirector.Play();
    }

}
