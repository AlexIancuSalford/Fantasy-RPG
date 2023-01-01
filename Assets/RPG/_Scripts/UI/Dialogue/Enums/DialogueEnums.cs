namespace RPG.Dialogue
{
    public static class DialogueEnums
    {
        /// <summary>
        /// Enum used to specify the type of action being performed.
        /// </summary>
        public enum ActionType
        {
            Add,
            Delete,
        }

        /// <summary>
        /// Enum used to specify the speaker of the dialogue in the node.
        /// </summary>
        public enum Speaker
        {
            Player,
            Other,
        }

        /// <summary>
        /// Enum used to specify the action of the speaker of the dialogue.
        /// </summary>
        public enum DialogueAction
        {
            None,
            Attack,
        }
    }
}
