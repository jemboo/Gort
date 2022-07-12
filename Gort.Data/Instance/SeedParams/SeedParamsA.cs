using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.SeedParams
{

    public class SeedParamsA : SeedParamsBase
    {
        public SeedParamsA()
        {
            RngSeed = MakeParam(ParamTypes.RngSeed, 123);
            RngType = MakeParam(ParamTypes.RngType, RandGenType.Lcg);
            Order = MakeParam(ParamTypes.Order, 16);
            SortableCount = MakeParam(ParamTypes.SortableCount, 20);
            SortableFormat = MakeParam(ParamTypes.SortableFormat, DataModel.SortableFormat.b64);
        }

        public Param RngSeed { get; private set; }
        public Param SortableCount { get; private set; }
        public Param SortableFormat { get; private set; }
        public Param RngType { get; private set; }
        public Param Order { get; private set; }

    }
}
