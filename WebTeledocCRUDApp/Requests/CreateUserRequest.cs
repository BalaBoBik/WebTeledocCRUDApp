using System.ComponentModel.DataAnnotations;

namespace WebTeledocCRUDApp.Requests
{
    public class CreateUserRequest
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string FamilyName { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string INN { get; set; }
    }
}
