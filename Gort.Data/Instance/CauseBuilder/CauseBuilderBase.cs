using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance.CauseBuilder
{
    public class CauseBuilderBase
    {
        public CauseBuilderBase(string workspaceName, int causeIndex)
        {
            WorkspaceName = workspaceName;
            CauseIndex = causeIndex;
            Workspace = new Workspace() { Name = workspaceName }.AddId();
        }

        public Cause Cause { get; protected set; }
        public string WorkspaceName { get; }
        public int CauseIndex { get; }

        public string? CauseDescription { get; protected set; }

        public Workspace Workspace { get; private set; }

        #region Params

        private readonly List<Param> _memberParams = new List<Param>();
        protected Param MakeParam(ParamType paramType, object v)
        {
            try
            {
                var pram = new Param()
                    {
                        ParamTypeId = paramType.ParamTypeId,
                        Value = paramType.ParamDataType.ToBytes(v)
                    }.AddId();
                _memberParams.Add(pram);
                return pram;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MakeParam", ex);
            }
        }

        protected Param AddParam(Param pram)
        {
            _memberParams.Add(pram);
            return pram;
        }
        public IEnumerable<Param> Params
        {
            get { return _memberParams; }
        }

        #endregion

        #region Causes

        private readonly List<Cause> _memberCauses = new List<Cause>();

        protected Cause MakeCause(CauseType causeType)
        {
            var cs = new Cause()
            {
                CauseDescr = CauseDescription,
                CauseTypeID = causeType.CauseTypeId,
                WorkspaceId = Workspace.WorkspaceId,
                Index = CauseIndex,
                CauseStatus = CauseStatus.Pending
            }.AddId();

            _memberCauses.Add(cs);
            return cs;
        }
        public IEnumerable<Cause> Causes
        {
            get { return _memberCauses; }
        }

        #endregion

        #region CauseParams

        private readonly List<CauseParam> _memberCauseParams = new List<CauseParam>();

        protected CauseParam MakeCauseParam(CauseParamType causeParamType, Cause cause, Param pram)
        {
            var cs = new CauseParam()
            {
                CauseParamTypeId = causeParamType.CauseParamTypeId,
                CauseId = cause.CauseId,
                ParamId = pram.ParamId
            }.AddId();

            _memberCauseParams.Add(cs);
            return cs;
        }
        public IEnumerable<CauseParam> CauseParams
        {
            get { return _memberCauseParams; }
        }

        #endregion

    }
}
