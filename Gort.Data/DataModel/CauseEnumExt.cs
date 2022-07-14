namespace Gort.Data.DataModel
{
    public static class CauseEnumExt
    {
        public static ParamDataType GetDataType(this ParamTypeName paramTypeName)
        {
            switch (paramTypeName)
            {
                case ParamTypeName.Generation:
                    return ParamDataType.Int32;
                case ParamTypeName.MutationRate:
                    return ParamDataType.Double;
                case ParamTypeName.Order:
                    return ParamDataType.Int32;
                case ParamTypeName.OrderStack:
                    return ParamDataType.IntArray;
                case ParamTypeName.RecordId:
                    return ParamDataType.Guid;
                case ParamTypeName.RecordPath:
                    return ParamDataType.String;
                case ParamTypeName.RngCount:
                    return ParamDataType.Int32;
                case ParamTypeName.RandGenId:
                    return ParamDataType.Guid;
                case ParamTypeName.RandGenSeed:
                    return ParamDataType.Int32;
                case ParamTypeName.RandGenType:
                    return ParamDataType.Int32;
                case ParamTypeName.SortableCount:
                    return ParamDataType.Int32;
                case ParamTypeName.SortableFormat:
                    return ParamDataType.Int32;
                case ParamTypeName.SortableSetId:
                    return ParamDataType.Guid;
                case ParamTypeName.SortableSetOrbitMaxCount:
                    return ParamDataType.Int32;
                case ParamTypeName.SortableSetOrbitPerm:
                    return ParamDataType.IntArray;
                case ParamTypeName.SorterCount:
                    return ParamDataType.Int32;
                case ParamTypeName.SorterExtent:
                    return ParamDataType.Int32;
                case ParamTypeName.SorterId:
                    return ParamDataType.Guid;
                case ParamTypeName.SorterMutationId:
                    return ParamDataType.Guid;
                case ParamTypeName.SorterPosition:
                    return ParamDataType.Int32;
                case ParamTypeName.SorterSaveMode:
                    return ParamDataType.Int32;
                case ParamTypeName.SorterSetId:
                    return ParamDataType.Guid;
                case ParamTypeName.SorterGroupName:
                    return ParamDataType.String;
                case ParamTypeName.StageBuddyCount:
                    return ParamDataType.Int32;
                case ParamTypeName.StageCount:
                    return ParamDataType.Int32;
                case ParamTypeName.SwitchCount:
                    return ParamDataType.Int32;
                case ParamTypeName.SwitchOrStage:
                    return ParamDataType.Int32;
                case ParamTypeName.TableName:
                    return ParamDataType.String;
                case ParamTypeName.Temperature:
                    return ParamDataType.Double;
                case ParamTypeName.WorkspaceId:
                    return ParamDataType.Guid;
                default:
                    throw new Exception($"{paramTypeName} not handled in GetParamType");
            }
        }

        public static byteWidth ToByteWidth(this Param pram)
        {
            return null;
        }
    }

}
