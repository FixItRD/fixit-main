using fixit_main.Models;
public enum Role
{
    Admin,
    Client,
    Worker
}
public interface ITokenService
{
    public Task<string> GenerateToken(int userID, string name, Role role);
    

}