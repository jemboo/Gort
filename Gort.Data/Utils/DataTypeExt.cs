using Gort.Data.DataModel;
using System.Text;

namespace Gort.Data.Utils
{
    public static class DataTypeExt
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

        public static RandGenType RandGenTypeFromInt(int val)
        {
            try
            {
                return (RandGenType)val;
            }
            catch { throw; }
        }


    }
}
