using System.ComponentModel.DataAnnotations;

namespace UserManagement.DTOs
{
    public class RoleDto
    {
        public int Id { get; set; } // Optional for Create

        [Required(ErrorMessage = "Role name is required.")]
        [StringLength(50, ErrorMessage = "Role name must not exceed 50 characters.")]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "Description must not exceed 200 characters.")]
        public string Description { get; set; }
    }
}
