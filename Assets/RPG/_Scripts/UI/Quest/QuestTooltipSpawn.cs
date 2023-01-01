using RPG.UI.Inventory;
using UnityEngine;

namespace RPG.UI.Quest
{
    public class QuestTooltipSpawn : TooltipSpawner
    {
        public override void UpdateTooltip(GameObject tooltip)
        {
            QuestStatus status = GetComponent<QuestItemUI>().QuestStatus;
            tooltip.GetComponent<QuestTooltipUI>().SetupTooltip(status);
        }

        public override bool CanCreateTooltip()
        {
            return true;
        }
    }
}
