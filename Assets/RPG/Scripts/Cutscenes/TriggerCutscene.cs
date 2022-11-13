using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    public class TriggerCutscene : MonoBehaviour
    {
        private bool _IsPlaying = true;

        private void OnTriggerEnter(Collider other)
        {
            if (_IsPlaying != true || !other.CompareTag("Player")) { return; }
            
            _IsPlaying = false;
            GetComponent<PlayableDirector>().Play();
        }
    }
}
