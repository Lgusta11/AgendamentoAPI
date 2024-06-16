namespace AgendamentosWEB.Requests
{
    public class UserRequestEdit
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // Constructor that takes three parameters
        public UserRequestEdit(string id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }
    }
}
