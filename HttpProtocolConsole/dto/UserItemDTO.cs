﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HttpProtocolConsole.dto
{
    public class UserItemDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
    }
}
