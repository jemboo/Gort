using Gort.Data.DataModel;
using Gort.Data.Utils;

namespace Gort.Data.Instance
{
    public static class CauseTypeGroups
    {
        static CauseTypeGroups()
        {
            Root = MakeCauseTypeGroup(CauseTypeGroupName.Root, null);
            Utils = MakeCauseTypeGroup(CauseTypeGroupName.Utils, Root.CauseTypeGroupId);
            Sortable = MakeCauseTypeGroup(CauseTypeGroupName.Sortable, Root.CauseTypeGroupId);
            SortableSetDef = MakeCauseTypeGroup(CauseTypeGroupName.SortableSetDef, Sortable.CauseTypeGroupId);
            SortableSetRnd = MakeCauseTypeGroup(CauseTypeGroupName.SortableSetRnd, Sortable.CauseTypeGroupId);
            Sorter = MakeCauseTypeGroup(CauseTypeGroupName.Sorter, Root.CauseTypeGroupId);
            SorterSetDef = MakeCauseTypeGroup(CauseTypeGroupName.SorterSetDef, Sorter.CauseTypeGroupId);
            SorterSetRnd = MakeCauseTypeGroup(CauseTypeGroupName.SorterSetRnd, Sorter.CauseTypeGroupId);
            SwitchList = MakeCauseTypeGroup(CauseTypeGroupName.SwitchList, Root.CauseTypeGroupId);
            SorterPerf = MakeCauseTypeGroup(CauseTypeGroupName.SorterPerf, Root.CauseTypeGroupId);
            SorterShc = MakeCauseTypeGroup(CauseTypeGroupName.SorterShc, Root.CauseTypeGroupId);
        }

        public static CauseTypeGroup Root { get; private set; }
        public static CauseTypeGroup Utils { get; private set; }
        public static CauseTypeGroup Sortable { get; private set; }
        public static CauseTypeGroup SortableSetDef { get; private set; }
        public static CauseTypeGroup SortableSetRnd { get; private set; }
        public static CauseTypeGroup Sorter { get; private set; }
        public static CauseTypeGroup SorterSetDef { get; private set; }
        public static CauseTypeGroup SorterSetRnd { get; private set; }
        public static CauseTypeGroup SwitchList { get; private set; }
        public static CauseTypeGroup SorterPerf { get; private set; }
        public static CauseTypeGroup SorterShc { get; private set; }

        static CauseTypeGroup MakeCauseTypeGroup(CauseTypeGroupName ptn, Guid? parentId)
        {
            var ctg = new CauseTypeGroup() { Name = ptn.ToString(), ParentId = parentId }.AddId();
            _members.Add(ctg);
            return ctg;
        }

        private static readonly List<CauseTypeGroup> _members = new List<CauseTypeGroup>();
        public static IEnumerable<CauseTypeGroup> Members
        {
            get { return _members; }
        }
    }
}
