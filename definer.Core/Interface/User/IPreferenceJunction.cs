using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface IPreferenceJunction : IBaseInterface<PreferenceJunction>
    {
        PreferenceJunction Manage(PreferenceJunction model);
    }
}
