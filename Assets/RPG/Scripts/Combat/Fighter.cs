using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        private Transform TargetTransform { get; set; }
        private Mover MoverRef { get; set; }

        [field : SerializeField]
        public float WeaponRange { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            MoverRef = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (TargetTransform != null)
            {
                float distance = Vector3.Distance(TargetTransform.position, gameObject.transform.position);

                if (distance >= WeaponRange)
                {
                    MoverRef.MoveTo(TargetTransform.position);
                }
                else
                {
                    MoverRef.Stop();
                }
            }
        }

        public void Attack(Target target)
        {
            TargetTransform = target.transform;
        }
    }
}
