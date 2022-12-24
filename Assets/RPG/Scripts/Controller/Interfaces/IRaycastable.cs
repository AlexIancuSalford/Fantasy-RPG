using RPG.Helper;
using CursorType = RPG.Helper.CursorHelper.CursorType; 

namespace RPG.Controller
{
    public interface IRaycastable
    {
        public bool HandleRaycast(PlayerController caller);
        public CursorType GetCursorType();
    }
}
