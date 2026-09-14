using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_admon.core.helpers.hashear
{
    public interface IHash
    {
        public string Hashear(string value);
        public bool VerifyHash(string value,string hash);
    }
}