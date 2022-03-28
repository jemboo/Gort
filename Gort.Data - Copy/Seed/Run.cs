using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    internal static partial class CauseSeed
    {
        public static void RunIt()
        {
            var ctxt = new GortContext();
            //AddAllCauseDescr(ctxt);
            //AddWorkspace1(ctxt);
            //AddCauseRndGenSet(ctxt);
            //AddCauseSortableSetAllForOrderA(ctxt);

            GetWorkspace1(ctxt);
            GetAllCauseDescr(ctxt);
            GetWorkspace1(ctxt);
            GetRndgen(ctxt);
            GetCauseSortableSetAllForOrderA(ctxt);


            //context.Fabrics.Attach(product.Fabric);
            //context.Products.Add(product);
            ctxt.SaveChanges();
        }

        public static void AddAllCauseDescr(GortContext ctxt)
        {
            AddCauseTypeGroups(ctxt);
            AddCauseTypes(ctxt);
            AddCauseParamTypes(ctxt);
        }
        public static void GetAllCauseDescr(GortContext ctxt)
        {
            GetCauseTypeGroups(ctxt);
            GetCauseTypes(ctxt);
            GetCauseParamTypes(ctxt);
        }

        public static void AddWorkspace1(GortContext ctxt)
        {
            workspace1 = new Workspace() { Name = "workspace1" };
            ctxt.Workspace.Add(workspace1);
        }

        public static void GetWorkspace1(GortContext ctxt)
        {
            workspace1 = ctxt.Workspace.Where(g => g.Name == "workspace1").First();
        }

        public static void GetRndgen(GortContext ctxt)
        {
            GetCauseRndGenSet(ctxt);
        }
    }

}
