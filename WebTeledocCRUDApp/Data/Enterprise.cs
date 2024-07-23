using System.ComponentModel.DataAnnotations;

namespace WebTeledocCRUDApp.Data
{
    public class Enterprise
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(12)]
        public string INN { get; set; }
        /// <summary>
        /// ЮЛ - true, ИП = false
        /// </summary>
        [Required]
        public bool IsIndividual { get; set; } 
        public List<User> Users { get; set; } = new List<User>();
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? RevokedOn { get; set; }

    }
}
