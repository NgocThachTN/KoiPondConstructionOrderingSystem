namespace KoiPond.DTOs
{
    public class UserDtoV1
    {
        public AccountDto Account { get; set; } // Assuming AccountDto is defined


        // Define the AccountDto class if you haven't done so
        public class AccountDto
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
