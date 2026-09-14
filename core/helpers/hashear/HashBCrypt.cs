using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.core.helpers.hashear
{
    public  class HashBCrypt : IHash
    {
        public  string Hashear(string value)
        {
            return BCrypt.Net.BCrypt.HashPassword(value);
        }

        public bool VerifyHash(string value,string hash)
        {
            return BCrypt.Net.BCrypt.Verify(value,hash);
        }
    }
}