using Gort.Data.DataModel;
using Gort.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Instance
{
    public static class ParamsA
    {
        static ParamsA()
        {
            RngSeedA = MakeParam(ParamTypes.RngSeed, 123);
            RngSeedB = MakeParam(ParamTypes.RngSeed, 9123);
            RngSeedC = MakeParam(ParamTypes.RngSeed, 1239);
            RngSeedD = MakeParam(ParamTypes.RngSeed, 1230);
            RngSeedE = MakeParam(ParamTypes.RngSeed, 12300);
            RngSeedF = MakeParam(ParamTypes.RngSeed, 123123);
            RngSeedG = MakeParam(ParamTypes.RngSeed, 123456);
            RngSeedH = MakeParam(ParamTypes.RngSeed, 123321);
            RngSeedI = MakeParam(ParamTypes.RngSeed, 123654);

            RngTypeLcg = MakeParam(ParamTypes.RngType, RndGenType.Lcg);

            RngIdA = MakeParam(ParamTypes.RngId, 212);
            RngIdB = MakeParam(ParamTypes.RngId, 9123);
            RngIdC = MakeParam(ParamTypes.RngId, 1239);
            RngIdD = MakeParam(ParamTypes.RngId, 1230);
            RngIdE = MakeParam(ParamTypes.RngId, 12300);
            RngIdF = MakeParam(ParamTypes.RngId, 123123);
            RngIdG = MakeParam(ParamTypes.RngId, 123456);
            RngIdH = MakeParam(ParamTypes.RngId, 123321);
            RngIdI = MakeParam(ParamTypes.RngId, 123654);

            Order8 = MakeParam(ParamTypes.Order, 16);
            Order10 = MakeParam(ParamTypes.Order, 16);
            Order12 = MakeParam(ParamTypes.Order, 16);
            Order14 = MakeParam(ParamTypes.Order, 16);
            Order16 = MakeParam(ParamTypes.Order, 16);
            Order18 = MakeParam(ParamTypes.Order, 16);
            Order20 = MakeParam(ParamTypes.Order, 16);
            Order22 = MakeParam(ParamTypes.Order, 16);
            Order24 = MakeParam(ParamTypes.Order, 16);
            Order32 = MakeParam(ParamTypes.Order, 16);
            Order64 = MakeParam(ParamTypes.Order, 16);
        }


        public static Param RngSeedA { get; private set; }
        public static Param RngSeedB { get; private set; }
        public static Param RngSeedC { get; private set; }
        public static Param RngSeedD { get; private set; }
        public static Param RngSeedE { get; private set; }
        public static Param RngSeedF { get; private set; }
        public static Param RngSeedG { get; private set; }
        public static Param RngSeedH { get; private set; }
        public static Param RngSeedI { get; private set; }

        public static Param RngTypeLcg { get; private set; }

        public static Param RngIdA { get; private set; }
        public static Param RngIdB { get; private set; }
        public static Param RngIdC { get; private set; }
        public static Param RngIdD { get; private set; }
        public static Param RngIdE { get; private set; }
        public static Param RngIdF { get; private set; }
        public static Param RngIdG { get; private set; }
        public static Param RngIdH { get; private set; }
        public static Param RngIdI { get; private set; }




        public static Param Order8 { get; private set; }
        public static Param Order10 { get; private set; }
        public static Param Order12 { get; private set; }
        public static Param Order14 { get; private set; }
        public static Param Order16 { get; private set; }
        public static Param Order18 { get; private set; }
        public static Param Order20 { get; private set; }
        public static Param Order22 { get; private set; }
        public static Param Order24 { get; private set; }
        public static Param Order32 { get; private set; }
        public static Param Order64 { get; private set; }





        private static readonly List<Param> _members = new List<Param>();

        static Param MakeParam(ParamType paramType, object v)
        {
            try
            {
                var pram = new Param() { ParamTypeId = paramType.ParamTypeId, Value = paramType.DataType.ToBytes(v) }.AddId();
                _members.Add(pram);
                return pram;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MakeParam", ex);
            }
        }
        public static IEnumerable<Param> Members
        {
            get { return _members; }
        }

    }
}
