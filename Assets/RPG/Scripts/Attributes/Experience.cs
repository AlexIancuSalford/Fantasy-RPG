using RPG.Save;
using UnityEngine;

namespace RPG.Attributes
{
    public class Experience : MonoBehaviour, ISaveableEntity
    {
        [field : SerializeField] public float ExperiencePoints { get; private set; }

        public void GainExperiencePoints(float experiencePoints)
        {
            ExperiencePoints += experiencePoints;
        }

        public object SaveState()
        {
            return ExperiencePoints;
        }

        public void LoadState(object obj)
        {
            ExperiencePoints = (float)obj;
        }
    }
}
