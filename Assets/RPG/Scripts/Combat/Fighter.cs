using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Attack(Target target)
        {
            Debug.Log("Player Attacking " + target.name);
        }
    }
}
