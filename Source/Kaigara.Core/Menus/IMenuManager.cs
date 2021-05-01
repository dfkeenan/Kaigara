namespace Kaigara.Menus
{
    public interface IMenuManager
    {
        IMenuItem FindMenuItem(MenuPath path);
        void Register(IMenuItem menu);
        void Remove(IMenuItem menu);
    }
}