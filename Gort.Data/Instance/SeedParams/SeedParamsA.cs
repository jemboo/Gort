using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.SeedParams
{

    public class SeedParamsA : SeedParamsBase
    {
        public SeedParamsA()
        {
            ImportRecordPath = MakeParam(ParamTypes.RecordPath, "");
            ImportTableName = MakeParam(ParamTypes.TableName, "SortableSet");
            RngCount = MakeParam(ParamTypes.RngCount, 3);
            RngSeed = MakeParam(ParamTypes.RngSeed, 765);
            RngType = MakeParam(ParamTypes.RngType, RandGenType.Lcg);
            Order = MakeParam(ParamTypes.Order, 16);
            OrderStack = MakeParam(ParamTypes.OrderStack, new[] { 4, 4, 8 });
            SortableCount = MakeParam(ParamTypes.SortableCount, 20);
            SortableFormat = MakeParam(ParamTypes.SortableFormat, DataModel.SortableFormat.b64);
        }

        public Param ImportRecordPath { get; private set; }
        public Param ImportTableName { get; private set; }
        public Param Order { get; private set; }
        public Param OrderStack { get; private set; }
        public Param RngCount { get; private set; }
        public Param RngSeed { get; private set; }
        public Param RngType { get; private set; }
        public Param SortableCount { get; private set; }
        public Param SortableFormat { get; private set; }

    }
}
