using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetShop
{
    public class Basket : Entity
    {
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Count { get; set; }
        public bool Pay { get; set; }
        public DateTime? PayDate { get; set; }
        public string CardNumber { get; set; }
    }
}
