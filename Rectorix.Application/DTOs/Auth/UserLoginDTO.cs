﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rectorix.Application.DTOs.Auth
{
    public class UserLoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

}
