namespace LMS_Elibrary.Data
{
    public class AccountWithRolesDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}
