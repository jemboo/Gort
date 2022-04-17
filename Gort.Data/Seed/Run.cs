using System.Security.Cryptography;
using System.Text;

namespace Gort.Data.Seed
{
    public static partial class CauseSeed
    {
        public static string GetThat()
        {
            return "yark! it's Batman";
        }

        public static void RunIt()
        {
            var ctxt = new GortContext();
            //GetAllCauseDescr(ctxt);
            //GetWorkspace1(ctxt);
            //GetRndgen(ctxt);
            //AddCauseRndGenSet(ctxt);

            //AddAllCauseDescr(ctxt);
            //AddWorkspace1(ctxt);
            //AddCauseSortableSetAllForOrderA(ctxt);

            //GetWorkspace1(ctxt);
            //GetAllCauseDescr(ctxt);
            //GetWorkspace1(ctxt);
            //AddCauseRndGen(ctxt);
            //GetRndgen(ctxt);
            //GetCauseSortableSetAllForOrderA(ctxt);


            //context.Fabrics.Attach(product.Fabric);
            //context.Products.Add(product);
            ctxt.SaveChanges();
        }

        public static void AddParamTables(IGortContext ctxt)
        {
            AddParamTypes(ctxt);
            AddParams(ctxt);

        }

        public static void AddAllCauseDescr(IGortContext ctxt)
        {
            AddCauseTypeGroups(ctxt);
            AddCauseTypes(ctxt);
            AddParamTypes(ctxt);
            AddParamTypesToCauseTypes(ctxt);
        }
        public static void GetAllCauseDescr(IGortContext ctxt)
        {
            GetCauseTypeGroups(ctxt);
            GetCauseTypes(ctxt);
            GetParamTypes(ctxt);
        }

        public static void AddWorkspace1(IGortContext ctxt)
        {
            workspace1 = new Workspace() { Name = "workspace1" };
            ctxt.Workspace.Add(workspace1);
        }

        public static void GetWorkspace1(IGortContext ctxt)
        {
            workspace1 = ctxt.Workspace.Where(g => g.Name == "workspace1").First();
        }

        public static void GetRndgen(IGortContext ctxt)
        {
            GetCauseRndGenSet(ctxt);
        }
    }

}
