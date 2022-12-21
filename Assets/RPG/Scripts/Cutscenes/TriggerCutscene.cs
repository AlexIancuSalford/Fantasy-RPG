using RPG.Save;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    public class TriggerCutscene : MonoBehaviour, ISaveableEntity
    {
        private static bool _isPlaying = true;

        private void OnTriggerEnter(Collider other)
        {
            if (_isPlaying != true || !other.CompareTag("Player")) { return; }
            
            _isPlaying = false;
            GetComponent<PlayableDirector>().Play();
        }

        public object SaveState()
        {
            return _isPlaying;
        }

        public void LoadState(object obj)
        {
            _isPlaying = (bool)obj;
        }
    }
}
