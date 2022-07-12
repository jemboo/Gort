using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance.SeedParams
{
    public class SeedParamsBase
    {
        public SeedParamsBase()
        {
        }

        protected Param MakeParam(ParamType paramType, object v)
        {
            try
            {
                var pram = new Param()
                {
                    ParamTypeId = paramType.ParamTypeId,
                    Value = paramType.DataType.ToBytes(v)
                }.AddId();
                _members.Add(pram);
                return pram;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MakeParam", ex);
            }
        }

        private readonly List<Param> _members = new List<Param>();
        public IEnumerable<Param> Members
        {
            get { return _members; }
        }

    }
}
