using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.Data.Instance
{
    public static class CauseTypeGroups
    {
        static CauseTypeGroups()
        {
            Root = MakeCauseTypeGroup(CauseTypeGroupName.Root, null);
            Utils = MakeCauseTypeGroup(CauseTypeGroupName.RandGen, Root.CauseTypeGroupId);
            Sortable = MakeCauseTypeGroup(CauseTypeGroupName.Sortable, Root.CauseTypeGroupId);
            SortableSetDef = MakeCauseTypeGroup(CauseTypeGroupName.SortableSetDef, Sortable.CauseTypeGroupId);
            SortableSetRnd = MakeCauseTypeGroup(CauseTypeGroupName.SortableSetDef, Sortable.CauseTypeGroupId);
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
            return new CauseTypeGroup() { Name = ptn, ParentId = parentId }.AddId();
        }
    }
}
