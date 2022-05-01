using Gort.Data.DataModel;
using Gort.Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Instance
{
    public static class ParamsB
    {
        static ParamsB()
        {

        }


        public static Param RngIdA { get; private set; }
        public static Param RngIdB { get; private set; }
        public static Param RngIdC { get; private set; }
        public static Param RngIdD { get; private set; }
        public static Param RngIdE { get; private set; }
        public static Param RngIdF { get; private set; }
        public static Param RngIdG { get; private set; }
        public static Param RngIdH { get; private set; }
        public static Param RngIdI { get; private set; }


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
