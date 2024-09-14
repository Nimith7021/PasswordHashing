using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PasswordHashingFluentMVC.Models
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }

        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
    }
}