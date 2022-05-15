using Gort.Data.DataModel;
using Gort.Data.Instance;

namespace Gort.Data.Load
{
    internal static class InstanceLoader
    {
        public static void LoadStatics(IGortContext ctxt)
        {
            foreach (var pt in CauseTypeGroups.Members)
            {
                ctxt.CauseTypeGroup.Add(pt);
            }
            foreach (var pt in ParamTypes.Members)
            {
                ctxt.ParamType.Add(pt);
            }
            foreach (var pt in CauseTypes.Members)
            {
                ctxt.CauseType.Add(pt);
            }
            foreach (var pt in CauseParamTypes.Members)
            {
                ctxt.CauseParamType.Add(pt);
            }
            ctxt.SaveChanges();
        }

        public static void LoadWorkSpace(IGortContext ctxt)
        {
            GortInstLoader.LoadInst(new WksRand(), ctxt);
        }

    }
}
