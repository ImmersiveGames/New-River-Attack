using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class TimeLineManager : MonoBehaviour
{
    PlayableDirector playableDirector;
    protected float animStartTime;
    protected float animEndTime;

    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation(float animStartTime)
    {
        if (playableDirector.state == PlayState.Playing)
        {
            playableDirector.Stop();
        }

        playableDirector.time = animStartTime;
        playableDirector.Play();
    }

}
