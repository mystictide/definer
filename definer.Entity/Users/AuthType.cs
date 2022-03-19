namespace definer.Entity.Users
{
    public class AuthType
    {
        public Auth Type { get; set; }

    }
    public enum Auth
    {
        admin = 1,
        moderator = 2,
        author = 3,
        newbie = 4
    }
}
