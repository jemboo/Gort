using Gort.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Utils
{
    public static class DomainQuery
    {
        public static RandGen GetRandGen(Guid rndGenId, IGortContext ctxt) 
        {
            var res = ctxt.RandGen.SingleOrDefault(c => c.RandGenId == rndGenId);
            if (res == null) throw new Exception($"rndGenId {rndGenId} not found");
            return res;
        }
    }
}
