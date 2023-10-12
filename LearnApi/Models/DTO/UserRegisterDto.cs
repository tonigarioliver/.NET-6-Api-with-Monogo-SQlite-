﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearnApi.Models.DTO
{
    public class UserRegisterDto
    {
        [Column("name")]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; } = null!;

        [Column("email")]
        [StringLength(100)]
        [Unicode(false)]
        public string Email { get; set; } = null!;

        [Column("phone")]
        [StringLength(20)]
        [Unicode(false)]
        public string? Phone { get; set; }

        [Column("password")]
        [StringLength(50)]
        [Unicode(false)]
        public string? Password { get; set; }
    }
}
