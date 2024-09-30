using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiPondConstructionManagement.Data
{
    public class SignInModel
    {
        [Required, EmailAddress]
        public string Email {get; set;} = null!;

        public string Password {get; set;} = null!;
    }
}