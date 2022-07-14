using Gort.Data.DataModel;
using System.Text;

namespace Gort.Data.Utils
{
    public static class ParamU
    {
        public static Param MakeParam(this ParamType paramType, object pVal)
        {
            var dv = paramType.ParamDataType.ToBytes(pVal);
            return new Param() { ParamTypeId = paramType.ParamTypeId, Value = dv }.AddId();
        }

        public static int IntValue(this Param param)
        {
            try
            {
                return BitConverter.ToInt32(param.Value, 0);
            }
            catch (Exception ex)
            {
                throw new Exception("error in IntValue", ex);
            }
        }

        public static int[] IntArrayValue(this Param param)
        {
            try
            {
                var iB = new int[param.Value.Length / 4];
                Buffer.BlockCopy(param.Value, 0, iB, 0, param.Value.Length);
                return iB;
            }
            catch (Exception ex)
            {
                throw new Exception("error in IntArrayValue", ex);
            }
        }

        public static double DoubleValue(this Param param)
        {
            try
            {
                return BitConverter.ToDouble(param.Value, 0);
            }
            catch (Exception ex)
            {
                throw new Exception("error in DoubleValue", ex);
            }
        }

        public static double[] DoubleArrayValue(this Param param)
        {
            try
            {
                var iB = new double[param.Value.Length / 8];
                Buffer.BlockCopy(param.Value, 0, iB, 0, param.Value.Length);
                return iB;
            }
            catch (Exception ex)
            {
                throw new Exception("error in DoubleArrayValue", ex);
            }
        }

        public static string StringValue(this Param param)
        {
            try
            {
                return Encoding.Default.GetString(param.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("error in StringValue", ex);
            }
        }

        public static string[] StringArrayValue(this Param param)
        {
            try
            {
                var concated = Encoding.Default.GetString(param.Value);
                return concated.Split("\n".ToCharArray());
            }
            catch (Exception ex)
            {
                throw new Exception("error in StringArrayValue", ex);
            }
        }

        public static Guid GuidValue(this Param param)
        {
            try
            {
                return new Guid(param.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("error in GuidValue", ex);
            }
        }

        public static Guid[] GuidArrayValue(this Param param)
        {
            try
            {
                var guRets = new Guid[param.Value.Length / 16];
                for (var i = 0; i < guRets.Length; i++)
                {
                    var gSl = param.Value.AsSpan().Slice(i * 16, 16);
                    guRets[i] = new Guid(gSl);
                }
                return guRets;
            }
            catch (Exception ex)
            {
                throw new Exception("error in GuidArrayValue", ex);
            }
        }

        public static byte[] ByteArrayValue(this Param param)
        {
            try
            {
                return param.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("error in ByteArrayValue", ex);
            }
        }

        public static RandGenType RandGenTypeValue(this Param param)
        {
            try
            {
                return (RandGenType)param.IntValue();
            }
            catch (Exception ex)
            {
                throw new Exception("error in RandGenTypeValue", ex);
            }
        }

        public static SortableFormat SortableFormatValue(this Param param)
        {
            try
            {
                return (SortableFormat)param.IntValue();
            }
            catch (Exception ex)
            {
                throw new Exception("error in SortableFormatValue", ex);
            }
        }
    }
}
