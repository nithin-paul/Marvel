using Marvel.Model;

namespace Marvel.DAC.Interfaces
{
    public interface IUserDAL
    {
        UserDTO GetUser(long userId);
    }
}
