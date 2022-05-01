using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance
{
    public class GortInstBase
    {
        public GortInstBase(string workspaceName)
        {
            Workspace = new Workspace() { Name = workspaceName }.AddId();
        }
        public Workspace Workspace { get; private set; }


        #region Params

        private readonly List<Param> _memberParams = new List<Param>();
        protected Param MakeParam(ParamType paramType, object v)
        {
            try
            {
                var pram = new Param() { ParamTypeId = paramType.ParamTypeId, Value = paramType.DataType.ToBytes(v) }.AddId();
                _memberParams.Add(pram);
                return pram;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in MakeParam", ex);
            }
        }
        public IEnumerable<Param> Params
        {
            get { return _memberParams; }
        }

        #endregion

        #region Causes

        private readonly List<Cause> _memberCauses = new List<Cause>();

        protected Cause MakeCause(int index, string descr, CauseType causeType)
        {
            var cs = new Cause()
            {
                Description = descr,
                CauseTypeID = causeType.CauseTypeId,
                WorkspaceId = Workspace.WorkspaceId,
                Index = index,
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

    public static class GortInstLoader
    {
        public static void LoadInst(GortInstBase gib, IGortContext ctxt)
        {
            ctxt.Workspace.Add(gib.Workspace);
            foreach (var pt in gib.Params)
            {
                ctxt.Param.Add(pt);
            }

            foreach (var pt in gib.Causes)
            {
                ctxt.Cause.Add(pt);
            }

            foreach (var pt in gib.CauseParams)
            {
                ctxt.CauseParam.Add(pt);
            }
            ctxt.SaveChanges();
        }
    }
}
