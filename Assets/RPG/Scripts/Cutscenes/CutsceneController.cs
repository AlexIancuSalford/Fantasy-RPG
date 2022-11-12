using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{

    public class CutsceneController : MonoBehaviour
    {
        private PlayableDirector PlayableDirector { get; set; }

        private void Start()
        {
            PlayableDirector = GetComponent<PlayableDirector>();

            PlayableDirector.played += DisableControl;
            PlayableDirector.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            Debug.Log("Disable Control");
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            Debug.Log("Enable Control");
        }
    }
}
