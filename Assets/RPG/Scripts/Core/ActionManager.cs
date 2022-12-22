using UnityEngine;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        private IAction CurrentAction { get; set; }

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
