namespace WebQueue.Models.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public int AmountOfRequests { get; set; }
    }
}