using Gort.DataStore.DataModel;

namespace Gort.DataStore.CauseBuild
{
    public class CauseBuildBase
    {
        public CauseBuildBase(Workspace workspace,
                                int causeIndex,
                                string genus,
                                string species)
        {
            Workspace = workspace;
            CauseIndex = causeIndex;
            CauseGenus = genus;
            CauseSpecies = species;
            CauseComments = String.Empty;
        }

        Cause _cause;
        public Cause Cause 
        { 
            get
            {
                if(_cause == null)
                {
                    _cause = new Cause()
                    {
                        Genus = CauseGenus,
                        Species = CauseSpecies,
                        Comments = CauseComments,
                        Workspace = Workspace,
                        Index = CauseIndex,
                        CauseStatus = CauseStatus.Pending
                    };
                }

                return _cause;
            }
        }
        public Workspace Workspace { get; }
        public int CauseIndex { get; }
        public string CauseComments { get; protected set; }
        public string CauseGenus { get; }
        public string CauseSpecies { get; }
        public string? CauseDescription { get; protected set; }


        #region Params

        private readonly List<Param> _memberParams = new List<Param>();

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

        #region CauseParams

        private readonly List<CauseParam> _memberCauseParams 
            = new List<CauseParam>();

        protected CauseParam MakeCauseParam(Param pram, string causeParamName)
        {
            var cs = new CauseParam()
            {
                Cause = Cause,
                Param = pram,
                Name = causeParamName
            };

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
