using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    public class TriggerCutscene : MonoBehaviour
    {
        private static bool _isPlaying = true;

        private void OnTriggerEnter(Collider other)
        {
            if (_isPlaying != true || !other.CompareTag("Player")) { return; }
            
            _isPlaying = false;
            GetComponent<PlayableDirector>().Play();
        }
    }
}
