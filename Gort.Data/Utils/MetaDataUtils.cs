using Gort.Data.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Gort.Data.Utils
{
    public static class MetaDataUtils
    {
        public static byte[] ToBytes(this DataType dataType, object val)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.Int32:
                        return BitConverter.GetBytes((int)val);
                    case DataType.IntArray:
                        var ia = (int[])val;
                        var blIa = new byte[ia.Length * 4];
                        Buffer.BlockCopy(ia, 0, blIa, 0, blIa.Length);
                        return blIa;
                    case DataType.Double:
                        return BitConverter.GetBytes((double)val);
                    case DataType.DoubleArray:
                        var da = (double[])val;
                        var blDa = new byte[da.Length * 8];
                        Buffer.BlockCopy(da, 0, blDa, 0, blDa.Length);
                        return blDa;
                    case DataType.String:
                        return Encoding.ASCII.GetBytes((string)val);
                    case DataType.StringArray:
                        var fs = String.Join("\n", (string[])val);
                        return Encoding.ASCII.GetBytes(fs);
                    case DataType.Guid:
                        return ((Guid)val).ToByteArray();
                    case DataType.GuidArray:
                        var ga = (Guid[])val;
                        var blGa = new byte[ga.Length * 16];
                        for (var dex = 0; dex < ga.Length; dex++)
                        {
                            var gbs = ga[dex].ToByteArray();
                            Buffer.BlockCopy(gbs, 0, blGa, dex * 16, gbs.Length);
                        }
                        return blGa;
                    case DataType.ByteArray:
                        return (byte[])val;
                    default:
                        throw new Exception($"{dataType} not handled");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MetaDataUtils.ToBytes()", ex);
            }
        }

        public static object FromBytes(this DataType dataType, byte[] bVals)
        {
            try
            {
                switch (dataType)
                {
                    case DataType.Int32:
                        return BitConverter.ToInt32(bVals, 0);
                    case DataType.IntArray:
                        var iB = new int[bVals.Length / 4];
                        Buffer.BlockCopy(bVals, 0, iB, 0, bVals.Length);
                        return iB;
                    case DataType.Double:
                        return BitConverter.ToDouble(bVals, 0);
                    case DataType.DoubleArray:
                        var dB = new double[bVals.Length / 8];
                        Buffer.BlockCopy(bVals, 0, dB, 0, bVals.Length);
                        return dB;
                    case DataType.String:
                        return Encoding.Default.GetString(bVals);
                    case DataType.StringArray:
                        var concated = Encoding.Default.GetString(bVals);
                        return concated.Split("\n".ToCharArray());
                    case DataType.Guid:
                        return new Guid(bVals);
                    case DataType.GuidArray:
                        var guRets = new Guid[bVals.Length / 16];
                        for (var i = 0; i < guRets.Length; i++)
                        {
                            var gSl = bVals.AsSpan().Slice(i * 16, 16);
                            guRets[i] = new Guid(gSl);
                        }
                        return guRets;
                    case DataType.ByteArray:
                        return bVals;
                    default:
                        throw new Exception($"{dataType} not handled");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MetaDataUtils.FromBytes()", ex);
            }
        }

        public static RndGenType RndGenTypeFromInt(int val)
        {
            try
            {
                return (RndGenType)val;
            }
            catch { throw; }
        }

        public static Cause[] GetAllCausesForWorkspace(string workspaceName,
            IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ws = ctxt.Workspace.SingleOrDefault(c => c.Name == workspaceName);
                if (ws is null)
                {
                    throw new Exception($"Workspace \"{workspaceName}\" not found");
                }
                return ctxt.Cause.Where(c => c.Workspace == ws).ToArray();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static Cause GetCauseById(Guid causeId, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var qua = ctxt.Cause.Where(c => c.CauseId == causeId)
                                    .Include(s=>s.CauseParams).SingleOrDefault();
                if (qua is null)
                {
                    throw new Exception($"cause {causeId} not found");
                }
                ctxt.CauseType.Where(P => P.CauseTypeId == qua.CauseTypeID).Load();

                foreach (var cp in qua.CauseParams)
                {
                    ctxt.Param.Where(p => p.ParamId == cp.ParamId).Load();
                    ctxt.ParamType.Where(P => P.ParamTypeId == cp.Param.ParamTypeId).Load();
                    ctxt.CauseParamType.Where(p => p.CauseParamTypeId == cp.CauseParamTypeId).Load();
                }
                return qua;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static CauseType GetCauseType(this Cause cause, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ct = ctxt.CauseType.SingleOrDefault(ct => ct.CauseTypeId == cause.CauseTypeID);
                if(ct is null)
                {
                    throw new Exception($"causetype {cause.CauseTypeID} not found");
                }
                return ct;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static CauseTypeGroup GetCauseTypeGroup(this CauseType ct, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ctg = ctxt.CauseTypeGroup.SingleOrDefault(ct => ct.CauseTypeGroupId == ct.CauseTypeGroupId);
                if (ctg is null)
                {
                    throw new Exception($"CauseTypeGroup {ct.CauseTypeGroupId} not found");
                }
                return ctg;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static CauseTypeGroup GetCauseTypeGroupParent(this CauseTypeGroup ctGroup, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ctgp = ctxt.CauseTypeGroup.SingleOrDefault(ct => ct.CauseTypeGroupId == ctGroup.ParentId);
                if (ctgp is null)
                {
                    throw new Exception($"ctGroup {ctGroup.ParentId} not found");
                }
                return ctgp;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static ParamType GetParamType(this ParamTypeName name, IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                return ctxt.ParamType.SingleOrDefault(pt => pt.Name == name.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"ParamType with name {name} not found", ex);
            }
        }

        public static IEnumerable<CauseTypeGroup> GetCauseTypeGroupAncestors(this CauseType ct, 
            IGortContext? gortContext = null)
        {
            var ctxt = gortContext ?? new GortContext();
            Guid? ctgId = ct.CauseTypeGroupId;
            while (ctgId is not null)
            {
                var ctGroup = ctxt.CauseTypeGroup.SingleOrDefault(ctg => ctg.CauseTypeGroupId == ctgId);
                yield return ctGroup;
                ctgId = ctGroup?.ParentId;
            }
        }

        public static CauseTypeGroup[] GetCauseTypeGroupAncestry(this Cause cause, 
            IGortContext? gortContext = null)
        {
            try
            {
                var ctxt = gortContext ?? new GortContext();
                var ct = cause.GetCauseType(gortContext);
                var ancestors = ct.GetCauseTypeGroupAncestors().ToArray().Reverse().ToArray();
                return ancestors;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in GetCauseTypeGroupAncestry for cause {cause.CauseId}", ex);
            }
        }
    }
}
