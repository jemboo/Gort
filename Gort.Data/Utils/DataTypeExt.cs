using Gort.Data.DataModel;
using System.Text;

namespace Gort.Data.Utils
{
    public static class DataTypeExt
    {
        public static byte[] ToBytes(this ParamDataType dataType, object val)
        {
            try
            {
                switch (dataType)
                {
                    case ParamDataType.Int32:
                        return BitConverter.GetBytes((int)val);
                    case ParamDataType.IntArray:
                        var ia = (int[])val;
                        var blIa = new byte[ia.Length * 4];
                        Buffer.BlockCopy(ia, 0, blIa, 0, blIa.Length);
                        return blIa;
                    case ParamDataType.Double:
                        return BitConverter.GetBytes((double)val);
                    case ParamDataType.DoubleArray:
                        var da = (double[])val;
                        var blDa = new byte[da.Length * 8];
                        Buffer.BlockCopy(da, 0, blDa, 0, blDa.Length);
                        return blDa;
                    case ParamDataType.String:
                        return Encoding.ASCII.GetBytes((string)val);
                    case ParamDataType.StringArray:
                        var fs = String.Join("\n", (string[])val);
                        return Encoding.ASCII.GetBytes(fs);
                    case ParamDataType.Guid:
                        return ((Guid)val).ToByteArray();
                    case ParamDataType.GuidArray:
                        var ga = (Guid[])val;
                        var blGa = new byte[ga.Length * 16];
                        for (var dex = 0; dex < ga.Length; dex++)
                        {
                            var gbs = ga[dex].ToByteArray();
                            Buffer.BlockCopy(gbs, 0, blGa, dex * 16, gbs.Length);
                        }
                        return blGa;
                    case ParamDataType.ByteArray:
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

        public static object FromBytes(this ParamDataType dataType, byte[] bVals)
        {
            try
            {
                switch (dataType)
                {
                    case ParamDataType.Int32:
                        return BitConverter.ToInt32(bVals, 0);
                    case ParamDataType.IntArray:
                        var iB = new int[bVals.Length / 4];
                        Buffer.BlockCopy(bVals, 0, iB, 0, bVals.Length);
                        return iB;
                    case ParamDataType.Double:
                        return BitConverter.ToDouble(bVals, 0);
                    case ParamDataType.DoubleArray:
                        var dB = new double[bVals.Length / 8];
                        Buffer.BlockCopy(bVals, 0, dB, 0, bVals.Length);
                        return dB;
                    case ParamDataType.String:
                        return Encoding.Default.GetString(bVals);
                    case ParamDataType.StringArray:
                        var concated = Encoding.Default.GetString(bVals);
                        return concated.Split("\n".ToCharArray());
                    case ParamDataType.Guid:
                        return new Guid(bVals);
                    case ParamDataType.GuidArray:
                        var guRets = new Guid[bVals.Length / 16];
                        for (var i = 0; i < guRets.Length; i++)
                        {
                            var gSl = bVals.AsSpan().Slice(i * 16, 16);
                            guRets[i] = new Guid(gSl);
                        }
                        return guRets;
                    case ParamDataType.ByteArray:
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

    }
}
