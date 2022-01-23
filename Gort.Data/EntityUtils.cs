using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Gort.Data
{
    public static class EntityUtils
    {
        private static Guid GuidFromString(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes(input));
                return new Guid(hash);
            }
        }

        public static CauseTypeGroup AddId(this CauseTypeGroup ctg)
        {
            var id = $"{ctg.Name} {ctg.ParentId}".GuidFromString();
            return new CauseTypeGroup() { CauseTypeGroupId = id, Name = ctg.Name, Parent = ctg.Parent };
        }

        public static CauseType AddId(this CauseType ct)
        {
            var id = $"{ct.Name} {ct.CauseTypeGroup.CauseTypeGroupId}".GuidFromString();
            return new CauseType() { CauseTypeId = id, Name = ct.Name, CauseTypeGroup = ct.CauseTypeGroup };
        }

        public static CauseTypeParam AddId(this CauseTypeParam ctp)
        {
            var id = $"{ctp.Name} {ctp.CauseType.CauseTypeId}".GuidFromString();
            return new CauseTypeParam() { CauseTypeParamId = id, Name = ctp.Name, CauseType = ctp.CauseType, CauseTypeId = ctp.CauseTypeId, DataType = ctp.DataType };
        }

        public static Cause AddId(this Cause cs)
        {
            var id = $"{cs.Index} {cs.WorkspaceId}".GuidFromString();
            return new Cause() { CauseId = id, Description = cs.Description, CauseType = cs.CauseType, CauseParams = cs.CauseParams, Index=cs.Index, WorkspaceId = cs.WorkspaceId };
        }

        public static CauseParam AddId(this CauseParam cp)
        {
            var id = $"{cp.CauseTypeParamId} {cp.CauseId}".GuidFromString();
            return new CauseParam() { CauseParamId = id, CauseId = cp.CauseId, CauseTypeParamId = cp.CauseTypeParamId, Value = cp.Value };
        }

        public static RndGen AddId(this RndGen rg)
        {
            var id = $"{rg.Type} {rg.Seed}".GuidFromString();
            return new RndGen() { RndGenId = id, Type = rg.Type, Seed = rg.Seed };
        }
    }
}
