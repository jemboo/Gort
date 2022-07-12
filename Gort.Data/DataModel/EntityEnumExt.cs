namespace Gort.Data.DataModel
{
    public static class EntityEnumExt
    {

        public static DataType GetDataType(this ParamTypeName paramTypeName)
        {
            switch (paramTypeName)
            {
                case ParamTypeName.Generation:
                    return DataType.Int32;
                case ParamTypeName.MutationRate:
                    return DataType.Double;
                case ParamTypeName.Order:
                    return DataType.Int32;
                case ParamTypeName.OrderStack:
                    return DataType.IntArray;
                case ParamTypeName.RecordId:
                    return DataType.Guid;
                case ParamTypeName.RecordPath:
                    return DataType.String;
                case ParamTypeName.RngCount:
                    return DataType.Int32;
                case ParamTypeName.RngId:
                    return DataType.Guid;
                case ParamTypeName.RngSeed:
                    return DataType.Int32;
                case ParamTypeName.RngType:
                    return DataType.Int32;
                case ParamTypeName.SortableCount:
                    return DataType.Int32;
                case ParamTypeName.SortableFormat:
                    return DataType.Int32;
                case ParamTypeName.SortableSetId:
                    return DataType.Guid;
                case ParamTypeName.SortableSetOrbitMaxCount:
                    return DataType.Int32;
                case ParamTypeName.SortableSetOrbitPerm:
                    return DataType.IntArray;
                case ParamTypeName.SorterCount:
                    return DataType.Int32;
                case ParamTypeName.SorterExtent:
                    return DataType.Int32;
                case ParamTypeName.SorterId:
                    return DataType.Guid;
                case ParamTypeName.SorterMutationId:
                    return DataType.Guid;
                case ParamTypeName.SorterPosition:
                    return DataType.Int32;
                case ParamTypeName.SorterSaveMode:
                    return DataType.Int32;
                case ParamTypeName.SorterSetId:
                    return DataType.Guid;
                case ParamTypeName.SorterGroupName:
                    return DataType.String;
                case ParamTypeName.StageBuddyCount:
                    return DataType.Int32;
                case ParamTypeName.StageCount:
                    return DataType.Int32;
                case ParamTypeName.SwitchCount:
                    return DataType.Int32;
                case ParamTypeName.SwitchOrStage:
                    return DataType.Int32;
                case ParamTypeName.TableName:
                    return DataType.String;
                case ParamTypeName.Temperature:
                    return DataType.Double;
                case ParamTypeName.WorkspaceId:
                    return DataType.Guid;
                default:
                    throw new Exception($"{paramTypeName} not handled in GetParamType");
            }
        }


    }

}
