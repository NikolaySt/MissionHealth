namespace SocialNet.Backend.Passwords
{
    public interface IPasswordProcessor
    {
	    string Process(string password);

	    bool Validate(string password, string hash);
    }
}
