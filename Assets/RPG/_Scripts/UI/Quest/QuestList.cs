using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestList : MonoBehaviour
    {
        [field : SerializeField] public QuestStatus[] QuestStatuses { get; private set; } = null;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
