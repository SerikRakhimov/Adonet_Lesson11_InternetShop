using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop
{
    public class User : Entity
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public virtual ICollection<Basket> Baskets { get; set; }
    }
}
