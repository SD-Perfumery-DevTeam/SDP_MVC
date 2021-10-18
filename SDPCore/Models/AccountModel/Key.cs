using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDPCore.Models.AccountModel
{
    public class Key
    {
        public Key(string publicKey, string descirption, decimal? amount, string currency)
        {
            this.publicKey = publicKey;
            this.descirption = descirption;
            this.amount = amount;
            this.currency = currency;
        }

        public Key()
        {
        }

        public string publicKey { get; set; }
        public string descirption { get; set; }
        public decimal? amount { get; set; }
        public string currency { get; set; }
    }
}
