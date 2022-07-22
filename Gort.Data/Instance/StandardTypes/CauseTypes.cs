using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance.StandardTypes
{
    public static class CauseTypes
    {
        static CauseTypes()
        {
            Rng = MakeCauseType(CauseTypeName.Rng, CauseTypeGroups.Utils);
            RngSet = MakeCauseType(CauseTypeName.RngSet, CauseTypeGroups.Utils);
            SorterMutation = MakeCauseType(CauseTypeName.SorterMutation, CauseTypeGroups.Sorter);
            SortableImport = MakeCauseType(CauseTypeName.SortableImport, CauseTypeGroups.Sortable);
            SortableSetImport = MakeCauseType(CauseTypeName.SortableSetImport, CauseTypeGroups.Sortable);
            SortableSetRand = MakeCauseType(CauseTypeName.SortableSetRand, CauseTypeGroups.SortableSetRnd);
            SortableSetAllForOrder = MakeCauseType(CauseTypeName.SortableSetAllForOrder, CauseTypeGroups.SortableSetDef);
            SortableSetOrbit = MakeCauseType(CauseTypeName.SortableSetOrbit, CauseTypeGroups.SortableSetDef);
            SortableSetStacked = MakeCauseType(CauseTypeName.SortableSetStacked, CauseTypeGroups.SortableSetDef);
            SorterImport = MakeCauseType(CauseTypeName.SorterImport, CauseTypeGroups.Sorter);
            SorterGroupName = MakeCauseType(CauseTypeName.SorterGroupName, CauseTypeGroups.Sorter);
            SorterSetImport = MakeCauseType(CauseTypeName.SorterSetImport, CauseTypeGroups.SorterSetDef);
            SorterSetRandBySwitch = MakeCauseType(CauseTypeName.SorterSetRandBySwitch, CauseTypeGroups.SorterSetRnd);
            SorterSetRandByStage = MakeCauseType(CauseTypeName.SorterSetRandByStage, CauseTypeGroups.SorterSetRnd);
            SorterSetRandByRflStage = MakeCauseType(CauseTypeName.SorterSetRandByRflStage, CauseTypeGroups.SorterSetRnd);
            SorterSetRandByRflStageBuddies = MakeCauseType(CauseTypeName.SorterSetRandByRflStageBuddies, CauseTypeGroups.SorterSetRnd);
            SorterSetRandByMutation = MakeCauseType(CauseTypeName.SorterSetRandByMutation, CauseTypeGroups.SorterSetRnd);
            SwitchListImport = MakeCauseType(CauseTypeName.SwitchListImport, CauseTypeGroups.SwitchList);
            SorterPerf = MakeCauseType(CauseTypeName.SorterPerf, CauseTypeGroups.SorterPerf);
            SorterSetPerf = MakeCauseType(CauseTypeName.SorterSetPerf, CauseTypeGroups.SorterPerf);
            SorterShc = MakeCauseType(CauseTypeName.SorterShc, CauseTypeGroups.SorterShc);
        }

        public static CauseType Rng { get; private set; }
        public static CauseType RngSet { get; private set; }
        public static CauseType SorterMutation { get; private set; }
        public static CauseType SortableImport { get; private set; }
        public static CauseType SortableSetImport { get; private set; }
        public static CauseType SortableSetAllForOrder { get; private set; }
        public static CauseType SortableSetOrbit { get; private set; }
        public static CauseType SortableSetStacked { get; private set; }
        public static CauseType SortableSetRand { get; private set; }
        public static CauseType SorterImport { get; private set; }
        public static CauseType SorterGroupName { get; private set; }
        public static CauseType SorterSetImport { get; private set; }
        public static CauseType SorterSetRandBySwitch { get; private set; }
        public static CauseType SorterSetRandByStage { get; private set; }
        public static CauseType SorterSetRandByRflStage { get; private set; }
        public static CauseType SorterSetRandByRflStageBuddies { get; private set; }
        public static CauseType SorterSetRandByMutation { get; private set; }
        public static CauseType SwitchListImport { get; private set; }
        public static CauseType SorterPerf { get; private set; }
        public static CauseType SorterSetPerf { get; private set; }
        public static CauseType SorterShc { get; private set; }
        static CauseType MakeCauseType(CauseTypeName ptn, CauseTypeGroup causeTypeGroup)
        {
            var ct = new CauseType() 
                { 
                    Name = ptn.ToString(), 
                    CauseTypeGroupId = causeTypeGroup.CauseTypeGroupId 
                }.AddId();
            _members.Add(ct);
            return ct;
        }

        private static readonly List<CauseType> _members = new List<CauseType>();
        public static IEnumerable<CauseType> Members
        {
            get { return _members; }
        }
    }
}
