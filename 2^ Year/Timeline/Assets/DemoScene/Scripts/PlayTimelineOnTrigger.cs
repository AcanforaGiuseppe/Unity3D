using UnityEngine;
using UnityEngine.Playables;

public class PlayTimelineOnTrigger : MonoBehaviour
{
    bool played;
    public PlayableDirector timeline;

    void OnTriggerEnter(Collider other)
    {
        if (played)
            return;

        played = true;
        timeline.Play();
    }

}