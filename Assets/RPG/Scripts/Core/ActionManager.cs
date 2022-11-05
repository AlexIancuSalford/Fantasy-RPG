using UnityEngine;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        private IAction CurrentAction { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartAction(IAction action)
        {
            if (CurrentAction == action) { return; }

            CurrentAction?.Cancel();

            CurrentAction = action;
        }

        public void CancelAction()
        {
            StartAction(null);
        }
    }
}
