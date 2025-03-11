using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace up_second_part.Models
{
    public partial class User
    {
        public string UserInitials => UserSurname + " " + UserName + " " + UserPatronymic;
    }
}
