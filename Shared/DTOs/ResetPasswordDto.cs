using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs
{
    public record ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }

        // The reset token issued by the forgot-password endpoint.
        [Required]
        public string Token { get; init; }

        [Required]
        public string NewPassword { get; init; }
    }
}
