namespace DevNet_BusinessLayer.Interfaces
{
    public interface IJwtService
    {
        String GenerateToken(string username, string name);
    }
}
