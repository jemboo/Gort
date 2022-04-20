namespace Gort.Data.Instance
{
    public static class ParamTypes
    {
        static ParamTypes()
        {
            Generation = MakeParamType(ParamTypeName.Generation, DataType.Int32);
            MutationRate = MakeParamType(ParamTypeName.MutationRate, DataType.Double);
            Order = MakeParamType(ParamTypeName.Order, DataType.Int32);
            OrderStack = MakeParamType(ParamTypeName.OrderStack, DataType.IntArray);
            RecordId = MakeParamType(ParamTypeName.RecordId, DataType.Guid);
            RecordPath = MakeParamType(ParamTypeName.RecordPath, DataType.String);
            RngCount = MakeParamType(ParamTypeName.RngCount, DataType.Int32);
            RngId = MakeParamType(ParamTypeName.RngId, DataType.Guid);
            RngSeed = MakeParamType(ParamTypeName.RngSeed, DataType.Int32);
            RngType = MakeParamType(ParamTypeName.RngType, DataType.Int32);
            SortableCount = MakeParamType(ParamTypeName.SortableCount, DataType.Int32);
            SortableFormat = MakeParamType(ParamTypeName.SortableFormat, DataType.Int32);
            SortableSetId = MakeParamType(ParamTypeName.SortableSetId, DataType.Guid);
            SortableSetOrbitMaxCount = MakeParamType(ParamTypeName.SortableSetOrbitMaxCount, DataType.Int32);
            SortableSetOrbitPerm = MakeParamType(ParamTypeName.SortableSetOrbitPerm, DataType.IntArray);
            SwitchOrStage = MakeParamType(ParamTypeName.SwitchOrStage, DataType.String);
            SorterCount = MakeParamType(ParamTypeName.SorterCount, DataType.Int32);
            SorterExtent = MakeParamType(ParamTypeName.SorterExtent, DataType.Int32);
            SorterId = MakeParamType(ParamTypeName.SorterId, DataType.Guid);
            SorterMutationId = MakeParamType(ParamTypeName.SorterMutationId, DataType.Guid);
            SorterPosition = MakeParamType(ParamTypeName.SorterPosition, DataType.Int32);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterSaveMode, DataType.Int32);
            SorterSetId = MakeParamType(ParamTypeName.SorterSetId, DataType.Guid);
            SorterGroupName = MakeParamType(ParamTypeName.SorterSetName, DataType.String);
            StageBuddyCount = MakeParamType(ParamTypeName.StageBuddyCount, DataType.Int32);
            StageCount = MakeParamType(ParamTypeName.StageCount, DataType.Int32);
            SwitchCount = MakeParamType(ParamTypeName.SwitchCount, DataType.Int32);
            SwitchOrStage = MakeParamType(ParamTypeName.SwitchOrStage, DataType.Int32);
            TableName = MakeParamType(ParamTypeName.TableName, DataType.String);
            Temperature = MakeParamType(ParamTypeName.Temperature, DataType.Double);
            WorkspaceId = MakeParamType(ParamTypeName.WorkspaceId, DataType.Guid);
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

        static ParamType MakeParamType(ParamTypeName ptn, DataType dt)
        {
            return new ParamType() { Name = ptn, DataType = dt }.AddId();
        }
    }
}
