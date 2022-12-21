using RPG.Controller;
using RPG.Core;
using RPG.Save;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cutscene
{
    public class CutsceneController : MonoBehaviour
    {
        private PlayableDirector PlayableDirector { get; set; }
        private GameObject Player { get; set; }

        private void Start()
        {
            PlayableDirector = GetComponent<PlayableDirector>();
            Player = GetPlayer();

            PlayableDirector.played += DisableControl;
            PlayableDirector.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            Player.GetComponent<ActionManager>().CancelAction();
            Player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            Player.GetComponent<PlayerController>().enabled = true;
        }

        private GameObject GetPlayer()
        {
            return Player == null ? GameObject.FindGameObjectWithTag("Player") : Player;
        }
    }
}
