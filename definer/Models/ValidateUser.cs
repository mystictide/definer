using definer.Entity.Users;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace definer.Models
{
    public static class ValidateUser
    {
        public static Users ValidateCurrentUser(this ControllerBase controller)
        {
            var userInfo = controller.User.FindFirst("user")?.Value;
            if (userInfo != null)
            {
                Users user = JsonConvert.DeserializeObject<Users>(userInfo);
                return user;
            }
            else
            {
                return null;
            }
            
        }
    }
}
