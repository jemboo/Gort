using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance.StandardTypes
{
    public static class ParamTypes
    {
        static ParamTypes()
        {
            Generation = MakeParamType(ParamTypeName.Generation);
            MutationRate = MakeParamType(ParamTypeName.MutationRate);
            Order = MakeParamType(ParamTypeName.Order);
            OrderStack = MakeParamType(ParamTypeName.OrderStack);
            RecordId = MakeParamType(ParamTypeName.RecordId);
            RecordPath = MakeParamType(ParamTypeName.RecordPath);
            RngCount = MakeParamType(ParamTypeName.RngCount);
            RngId = MakeParamType(ParamTypeName.RngId);
            RngSeed = MakeParamType(ParamTypeName.RngSeed);
            RngType = MakeParamType(ParamTypeName.RngType);
            SortableCount = MakeParamType(ParamTypeName.SortableCount);
            SortableFormat = MakeParamType(ParamTypeName.SortableFormat);
            SortableSetId = MakeParamType(ParamTypeName.SortableSetId);
            SortableSetOrbitMaxCount = MakeParamType(ParamTypeName.SortableSetOrbitMaxCount);
            SortableSetOrbitPerm = MakeParamType(ParamTypeName.SortableSetOrbitPerm);
            SorterCount = MakeParamType(ParamTypeName.SorterCount);
            SorterExtent = MakeParamType(ParamTypeName.SorterExtent);
            SorterId = MakeParamType(ParamTypeName.SorterId);
            SorterMutationId = MakeParamType(ParamTypeName.SorterMutationId);
            SorterPosition = MakeParamType(ParamTypeName.SorterPosition);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterSaveMode);
            SorterSetId = MakeParamType(ParamTypeName.SorterSetId);
            SorterGroupName = MakeParamType(ParamTypeName.SorterGroupName);
            StageBuddyCount = MakeParamType(ParamTypeName.StageBuddyCount);
            StageCount = MakeParamType(ParamTypeName.StageCount);
            SwitchCount = MakeParamType(ParamTypeName.SwitchCount);
            SwitchOrStage = MakeParamType(ParamTypeName.SwitchOrStage);
            TableName = MakeParamType(ParamTypeName.TableName);
            Temperature = MakeParamType(ParamTypeName.Temperature);
            WorkspaceId = MakeParamType(ParamTypeName.WorkspaceId);
        }

        public static ParamType Generation { get; private set; }
        public static ParamType MutationRate { get; private set; }
        public static ParamType Order { get; private set; }
        public static ParamType OrderStack { get; private set; }
        public static ParamType RecordId { get; private set; }
        public static ParamType RecordPath { get; private set; }
        public static ParamType RngCount { get; private set; }
        public static ParamType RngId { get; private set; }
        public static ParamType RngSeed { get; private set; }
        public static ParamType RngType { get; private set; }
        public static ParamType SortableCount { get; private set; }
        public static ParamType SortableFormat { get; private set; }
        public static ParamType SortableSetId { get; private set; }
        public static ParamType SortableSetOrbitMaxCount { get; private set; }
        public static ParamType SortableSetOrbitPerm { get; private set; }
        public static ParamType SorterCount { get; private set; }
        public static ParamType SorterExtent { get; private set; }
        public static ParamType SorterId { get; private set; }
        public static ParamType SorterMutationId { get; private set; }
        public static ParamType SorterPosition { get; private set; }
        public static ParamType SorterSaveMode { get; private set; }
        public static ParamType SorterSetId { get; private set; }
        public static ParamType SorterGroupName { get; private set; }
        public static ParamType StageBuddyCount { get; private set; }
        public static ParamType StageCount { get; private set; }
        public static ParamType SwitchCount { get; private set; }
        public static ParamType SwitchOrStage { get; private set; }
        public static ParamType TableName { get; private set; }
        public static ParamType Temperature { get; private set; }
        public static ParamType WorkspaceId { get; private set; }

        static ParamType MakeParamType(ParamTypeName ptn)
        {
            var pt = new ParamType() { 
                Name = ptn.ToString(), 
                DataType = ptn.GetDataType() }.AddId();
            _members.Add(pt);
            return pt;
        }

        private static readonly List<ParamType> _members = new List<ParamType>();
        public static IEnumerable<ParamType> Members
        {
            get { return _members; }
        }

        public static ParamType GetParamType(this ParamTypeName paramTypeName)
        {
            switch (paramTypeName)
            {
                case ParamTypeName.Generation:
                    return Generation;
                case ParamTypeName.MutationRate:
                    return MutationRate;
                case ParamTypeName.Order:
                    return Order;
                case ParamTypeName.OrderStack:
                    return OrderStack;
                case ParamTypeName.RecordId:
                    return RecordId;
                case ParamTypeName.RecordPath:
                    return RecordPath;
                case ParamTypeName.RngCount:
                    return RngCount;
                case ParamTypeName.RngId:
                    return RngId;
                case ParamTypeName.RngSeed:
                    return RngSeed;
                case ParamTypeName.RngType:
                    return RngType;
                case ParamTypeName.SortableCount:
                    return SortableCount;
                case ParamTypeName.SortableFormat:
                    return SortableFormat;
                case ParamTypeName.SortableSetId:
                    return SortableSetId;
                case ParamTypeName.SortableSetOrbitMaxCount:
                    return SortableSetOrbitMaxCount;
                case ParamTypeName.SortableSetOrbitPerm:
                    return SortableSetOrbitPerm;
                case ParamTypeName.SorterCount:
                    return SorterCount;
                case ParamTypeName.SorterExtent:
                    return SorterExtent;
                case ParamTypeName.SorterId:
                    return SorterId;
                case ParamTypeName.SorterMutationId:
                    return SorterMutationId;
                case ParamTypeName.SorterPosition:
                    return SorterPosition;
                case ParamTypeName.SorterSaveMode:
                    return SorterSaveMode;
                case ParamTypeName.SorterSetId:
                    return SorterSetId;
                case ParamTypeName.SorterGroupName:
                    return SorterGroupName;
                case ParamTypeName.StageBuddyCount:
                    return StageBuddyCount;
                case ParamTypeName.StageCount:
                    return StageCount;
                case ParamTypeName.SwitchCount:
                    return SwitchCount;
                case ParamTypeName.SwitchOrStage:
                    return SwitchOrStage;
                case ParamTypeName.TableName:
                    return TableName;
                case ParamTypeName.Temperature:
                    return Temperature;
                case ParamTypeName.WorkspaceId:
                    return WorkspaceId;
                default:
                    throw new Exception($"{paramTypeName} not handled in GetParamType");
            }
        }
    }
}
