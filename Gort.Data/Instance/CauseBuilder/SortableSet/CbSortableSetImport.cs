using Gort.Data.DataModel;
using Gort.Data.Instance.StandardTypes;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder.SortableSet
{
    public class CbSortableSetImport : CauseBuilderBase
    {
        public CbSortableSetImport(
            string workspaceName,
            int causeIndex,
            string descr,
            Param workspaceId,
            Param tableName,
            Param recordId,
            Param recordPath) : base(workspaceName, causeIndex)
        {

            Cause = MakeCause(CauseTypes.SortableSetImport);
            Pram_WorkspaceId = AddParam(workspaceId);
            Pram_TableName = AddParam(tableName);
            Pram_RecordId = AddParam(recordId);
            Pram_RecordPath = AddParam(recordPath);

            WorkspaceId = Guid.Empty;
            TableName = "";
            RecordId = Guid.Empty;
            RecordPath = "";

            CauseDescription = $"SortableSetAllForOrder({descr},{WorkspaceId},{TableName},{RecordId},{RecordPath})";

            CausePram_WorkspaceId =
                MakeCauseParam(
                    CauseParamTypes.SortableSetImport_WorkspaceId,
                    Cause,
                    Pram_WorkspaceId);

            CausePram_TableName =
                MakeCauseParam(
                    CauseParamTypes.SortableSetImport_TableName,
                    Cause,
                    Pram_TableName);

            CausePram_RecordId =
                MakeCauseParam(
                    CauseParamTypes.SortableSetImport_RecordId,
                    Cause,
                    Pram_RecordId);

            CausePram_RecordPath =
                MakeCauseParam(
                    CauseParamTypes.SortableSetImport_RecordPath,
                    Cause,
                    Pram_RecordPath);
        }
        public Guid WorkspaceId { get; }
        public string TableName { get; }
        public Guid RecordId { get; }
        public string RecordPath { get; }

        public Param Pram_WorkspaceId { get; private set; }
        public Param Pram_TableName { get; private set; }
        public Param Pram_RecordId { get; private set; }
        public Param Pram_RecordPath { get; private set; }

        public CauseParam CausePram_WorkspaceId { get; private set; }
        public CauseParam CausePram_TableName { get; private set; }
        public CauseParam CausePram_RecordId { get; private set; }
        public CauseParam CausePram_RecordPath { get; private set; }
    }
}
