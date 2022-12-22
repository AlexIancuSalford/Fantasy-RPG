using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour
    {
        [field : SerializeField] private float ExperiencePoints { get; set; } = 0;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GainExperiencePoints(float experiencePoints)
        {
            this.ExperiencePoints += experiencePoints;
        }
    }
}
