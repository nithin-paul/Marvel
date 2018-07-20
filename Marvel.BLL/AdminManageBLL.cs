using Marvel.DAC;
using Marvel.Model;
using System;

namespace Marvel.BLL
{
    public class AdminManageBLL
    {
        public UserManagerDAC userManager = null;

        public AdminManageBLL()
        {
            userManager = new UserManagerDAC();
        }

        public UserDTO AuthenticateUser(string userName, string password)
        {
            try
            {
                UserDTO userDto = userManager.ValidateLoginRequest(userName, password);
                return userDto;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
