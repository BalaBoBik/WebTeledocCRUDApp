using System.ComponentModel.DataAnnotations;

namespace WebTeledocCRUDApp.Data
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        [Required]
        public string FamilyName { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        [Required]
        public string Patronymic { get; set; }
        [Required]
        [StringLength(10)]
        public string INN { get; set; }
        public List<Enterprise> Enterprises { get; set; } = new List<Enterprise>();
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? RevokedOn { get; set; }

    }
}
