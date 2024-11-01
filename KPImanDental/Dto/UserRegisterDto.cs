﻿using System.ComponentModel.DataAnnotations;

namespace KPImanDental.Dto.UserDto
{
    public class UserRegisterDto : UserDto
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string Password { get; set; }
    }
}
