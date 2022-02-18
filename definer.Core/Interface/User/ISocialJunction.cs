using definer.Entity.Users;

namespace definer.Core.Interface.User
{
    public interface ISocialJunction : IBaseInterface<SocialJunction>
    {
        SocialJunction Manage(SocialJunction model);
    }
}
