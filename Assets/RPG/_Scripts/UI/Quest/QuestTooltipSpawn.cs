using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestTooltipSpawn : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            Quest quest = GetComponent<QuestItemUI>().Quest;
            tooltip.GetComponent<QuestTooltipUI>().SetupTooltip(quest);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}
