namespace MapBul.Web.Auth
{
    public interface IAuthProvider
    {
        bool Login(string email,string password);
        void Logout();
        void RefreshPrincipal();
        bool IsAuthenticated { get; }
        string UserType { get; }
        string UserGuid { get; }
    }
}