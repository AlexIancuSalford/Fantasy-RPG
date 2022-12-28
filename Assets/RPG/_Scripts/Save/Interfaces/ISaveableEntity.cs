namespace RPG.Save
{
    public interface ISaveableEntity
    {
        public object SaveState();
        public void LoadState(object obj);
    }
}
