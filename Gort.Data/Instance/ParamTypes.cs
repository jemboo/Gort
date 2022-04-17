namespace Gort.Data.Instance
{
    public static class ParamTypes
    {
        static ParamTypes()
        {
            WorkspaceId = MakeParamType(ParamTypeName.WorkspaceId, DataType.Guid);
            RngSeed = MakeParamType(ParamTypeName.RngSeed, DataType.Int32);
            RngType = MakeParamType(ParamTypeName.RngType, DataType.Int32);
            RndGenId = MakeParamType(ParamTypeName.RndGenId, DataType.Guid);
            RndGenCount = MakeParamType(ParamTypeName.RndGenCount, DataType.Guid);
            Order = MakeParamType(ParamTypeName.Order, DataType.Int32);
            OrderStack = MakeParamType(ParamTypeName.OrderStack, DataType.IntArray);
            SortableFormat = MakeParamType(ParamTypeName.SortableFormat, DataType.Int32);
            TableName = MakeParamType(ParamTypeName.TableName, DataType.String);
            RecordId = MakeParamType(ParamTypeName.RecordId, DataType.Guid);
            SortableSetId = MakeParamType(ParamTypeName.SortableSetId, DataType.Guid);
            RecordPath = MakeParamType(ParamTypeName.RecordPath, DataType.String);
            SortableSetOrbitPerm = MakeParamType(ParamTypeName.SortableSetOrbitPerm, DataType.IntArray);
            SortableSetOrbitMaxCount = MakeParamType(ParamTypeName.SortableSetOrbitMaxCount, DataType.Int32);
            SwitchOrStage = MakeParamType(ParamTypeName.SwitchOrStage, DataType.String);
            SorterPosition = MakeParamType(ParamTypeName.SorterPosition, DataType.Int32);
            SorterExtent = MakeParamType(ParamTypeName.SorterExtent, DataType.Int32);
            SwitchCount = MakeParamType(ParamTypeName.SorterExtent, DataType.Int32);
            SorterCount = MakeParamType(ParamTypeName.SorterCount, DataType.Int32);
            StageCount = MakeParamType(ParamTypeName.StageCount, DataType.Int32);
            StageBuddyCount = MakeParamType(ParamTypeName.StageBuddyCount, DataType.Int32);
            SorterId = MakeParamType(ParamTypeName.SorterId, DataType.Guid);
            SorterSetName = MakeParamType(ParamTypeName.SorterSetName, DataType.String);
            SorterSetId = MakeParamType(ParamTypeName.SorterSetId, DataType.Guid);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterSaveMode, DataType.Int32);

            SorterSaveMode = MakeParamType(ParamTypeName.MutationRate, DataType.Double);
            SorterSaveMode = MakeParamType(ParamTypeName.Temperature, DataType.Double);
            SorterSaveMode = MakeParamType(ParamTypeName.SwitchOrStage, DataType.Int32);
            SorterSaveMode = MakeParamType(ParamTypeName.Generation, DataType.Int32);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterMutationId, DataType.Guid);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterSaveMode, DataType.Int32);
            SorterSaveMode = MakeParamType(ParamTypeName.SorterSaveMode, DataType.Int32);
        }

        public static ParamType WorkspaceId { get; private set; }
        public static ParamType RngSeed { get; private set; }
        public static ParamType RngType { get; private set; }
        public static ParamType RndGenId { get; private set; }
        public static ParamType RndGenCount { get; private set; }
        public static ParamType Order { get; private set; }
        public static ParamType OrderStack { get; private set; }
        public static ParamType SortableFormat { get; private set; }
        public static ParamType TableName { get; private set; }
        public static ParamType RecordId { get; private set; }
        public static ParamType RecordPath { get; private set; }
        public static ParamType SortableSetId { get; private set; }
        public static ParamType SortableSetOrbitPerm { get; private set; }
        public static ParamType SortableSetOrbitMaxCount { get; private set; }
        public static ParamType SwitchOrStage { get; private set; }
        public static ParamType SorterPosition { get; private set; }
        public static ParamType SorterExtent { get; private set; }
        public static ParamType SwitchCount { get; private set; }
        public static ParamType SorterCount { get; private set; }
        public static ParamType StageCount { get; private set; }
        public static ParamType StageBuddyCount { get; private set; }
        public static ParamType SorterId { get; private set; }
        public static ParamType SorterSetName { get; private set; }
        public static ParamType SorterSetId { get; private set; }
        public static ParamType SorterSaveMode { get; private set; }

        static ParamType MakeParamType(ParamTypeName ptn, DataType dt)
        {
            return new ParamType() { Name = ptn, DataType = dt }.AddId();
        }
    }
}
