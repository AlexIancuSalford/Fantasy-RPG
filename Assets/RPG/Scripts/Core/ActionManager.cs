using UnityEngine;

namespace RPG.Core
{
    public class ActionManager : MonoBehaviour
    {
        private MonoBehaviour CurrentAction { get; set; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartAction(MonoBehaviour action)
        {
            if (CurrentAction == action) { return; }

            if (CurrentAction != null)
            {
                Debug.Log($"Canceling {CurrentAction}");
            }

            CurrentAction = action;
        }
    }
}
