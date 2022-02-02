using System.Security.Cryptography;
using System.Text;

namespace Gort.Data
{
    internal static class CauseSeed
    {
        public static void TryThis()
        {
            var ctxt = new GortContext();
            var wrkSpace = new Workspace() { Name = "workspace1" };
            var cause1 = new Cause() { Workspace = wrkSpace };
            ctxt.Workspaces.Add(wrkSpace);
            ctxt.Causes.Add(cause1);
            ctxt.SaveChanges();
            var wsBack = ctxt.Workspaces.Where(x => x.Name == "workspace1");
            //var s = new Sorter() { CauseId}

        }


        public static void Init()
        {
            var ctxt = new GortContext();
            //GetRndGens(ctxt);
            //GetCauseTypeGroup(ctxt);
            //GetCauseTypes(ctxt);
            //GetCauseTypeParams(ctxt);
            AddRndGens(ctxt);
            AddCauseTypeGroups(ctxt);
            AddCauseTypes(ctxt);
            AddCauseTypeParams(ctxt);

            ctxt.SaveChanges();
        }

        public static RndGen? rndGenA;
        public static RndGen? rndGenB;
        public static RndGen? rndGenC;
        public static RndGen? rndGenD;

        public static CauseTypeGroup? ctgRoot;
        public static CauseTypeGroup? ctgSortableSets;
        public static CauseTypeGroup? ctgSorterSets;
        public static CauseTypeGroup? ctgSorterPerfBins;
        public static CauseTypeGroup? ctgSorterGa;
        public static CauseTypeGroup? ctgRndSorterSets;

        public static CauseType? causeTypeStdSortables;
        public static CauseType? causeTypeRefSorterSets;
        public static CauseType? causeTypeRndSorterSetsBySwitch;
        public static CauseType? causeTypeRndSorterSetsByStage;
        public static CauseType? causeTypeRndSorterSetsBySymetricStage;
        public static CauseType? causeTypeRndSorterSetsBySymetricStageBuddies;
        public static CauseType? causeTypeSorterSetPerfBinsBySortableSet;

        public static CauseTypeParam? causeTypeParamStdSortables_MinDegree;
        public static CauseTypeParam? causeTypeParamStdSortables_MaxDegree;

        public static CauseTypeParam? causeTypeParamStdSortables_Name;

        public static CauseTypeParam? causeTypeParamRefSorterSets_Name;

        public static CauseTypeParam? causeTypeParamRndSorterSetsBySwitch_Degree;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySwitch_RndGen;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySwitch_SwitchCount;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySwitch_SorterCount;

        public static CauseTypeParam? causeTypeParamRndSorterSetsByStage_Degree;
        public static CauseTypeParam? causeTypeParamRndSorterSetsByStage_StageCount;
        public static CauseTypeParam? causeTypeParamRndSorterSetsByStage_RndGen;
        public static CauseTypeParam? causeTypeParamRndSorterSetsByStage_SorterCount;

        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStage_Degree;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStage_StageCount;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStage_RndGen;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStage_SorterCount;

        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStageBuddies_Degree;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStageBuddies_StageCount;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStageBuddies_RndGen;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStageBuddies_SorterCount;
        public static CauseTypeParam? causeTypeParamRndSorterSetsBySymmetricStageBuddies_BuddyCount;

        public static CauseTypeParam? causeTypeParamSorterSetPerfBinsBySortableSet_SorterSet;
        public static CauseTypeParam? causeTypeParamSorterSetPerfBinsBySortableSet_SortableSet;
        public static CauseTypeParam? causeTypeParamSorterSetPerfBinsBySortableSet_SorterSaveMode;

        public static void GetRndGens(GortContext gortContext)
        {
            rndGenA = gortContext.RndGens.Where(g => g.Seed == 1234).First();
            rndGenB = gortContext.RndGens.Where(g => g.Seed == 1334).First();
            rndGenC = gortContext.RndGens.Where(g => g.Seed == 1434).First();
            rndGenD = gortContext.RndGens.Where(g => g.Seed == 1534).First();
        }

        public static void AddRndGens(GortContext gortContext)
        {
            rndGenA = new RndGen() { Seed = 1234, RndGenType = RndGenType.Lcg }.AddId();
            rndGenB = new RndGen() { Seed = 1334, RndGenType = RndGenType.Lcg }.AddId();
            rndGenC = new RndGen() { Seed = 1434, RndGenType = RndGenType.Lcg }.AddId();
            rndGenD = new RndGen() { Seed = 1534, RndGenType = RndGenType.Lcg }.AddId();

            gortContext.RndGens.Add(rndGenA);
            gortContext.RndGens.Add(rndGenB);
            gortContext.RndGens.Add(rndGenC);
            gortContext.RndGens.Add(rndGenD);
        }

        public static void GetCauseTypeGroup(GortContext gortContext)
        {
            ctgRoot = gortContext.CauseTypeGroups.Where(g => g.Name == "root").First();
            ctgSortableSets = gortContext.CauseTypeGroups.Where(g => g.Name == "SortableSets").First();
            ctgSorterSets = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterSets").First();
            ctgSorterPerfBins = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterPerfBins").First();
            ctgSorterGa = gortContext.CauseTypeGroups.Where(g => g.Name == "SorterGa").First();
            ctgRndSorterSets = gortContext.CauseTypeGroups.Where(g => g.Name == "RndSorterSets").First();
        }

        public static void AddCauseTypeGroups(GortContext gortContext)
        {
            ctgRoot = new CauseTypeGroup() { Name = "root", Parent = null }.AddId();
            ctgSortableSets = new CauseTypeGroup() { Name = "SortableSets", Parent = ctgRoot }.AddId();
            ctgSorterSets = new CauseTypeGroup() { Name = "SorterSets", Parent = ctgRoot }.AddId();
            ctgSorterGa = new CauseTypeGroup() { Name = "SorterGa", Parent = ctgRoot }.AddId();
            ctgRndSorterSets = new CauseTypeGroup() { Name = "RndSorterSets", Parent = ctgSorterSets }.AddId();
            ctgSorterPerfBins = new CauseTypeGroup() { Name = "SorterPerfBins", Parent = ctgRoot }.AddId();


            gortContext.CauseTypeGroups.Add(ctgSortableSets);
            gortContext.CauseTypeGroups.Add(ctgSorterSets);
            gortContext.CauseTypeGroups.Add(ctgSorterPerfBins);
            gortContext.CauseTypeGroups.Add(ctgSorterGa);
            gortContext.CauseTypeGroups.Add(ctgRndSorterSets);

            /// delete from causetypegroups;
        }

        public static void GetCauseTypes(GortContext gortContext)
        {
            causeTypeStdSortables = gortContext.CauseTypes.Where(g => g.Name == "standard").First();
            causeTypeRefSorterSets = gortContext.CauseTypes.Where(g => g.Name == "reference").First();
            causeTypeRndSorterSetsBySwitch = gortContext.CauseTypes.Where(g => g.Name == "randomBySwitch").First();
            causeTypeRndSorterSetsByStage = gortContext.CauseTypes.Where(g => g.Name == "randomByStage").First();
            causeTypeRndSorterSetsBySymetricStage = gortContext.CauseTypes.Where(g => g.Name == "randomBySymetricStage").First();
            causeTypeRndSorterSetsBySymetricStageBuddies = gortContext.CauseTypes.Where(g => g.Name == "randomBySymetricStageBuddies").First();
            causeTypeSorterSetPerfBinsBySortableSet = gortContext.CauseTypes.Where(g => g.Name == "sorterSetPerfBinsBySortableSet").First();
        }

        public static void AddCauseTypes(GortContext gortContext)
        {
            causeTypeStdSortables = new CauseType() { Name = "standard", CauseTypeGroup = ctgSortableSets }.AddId();
            causeTypeRefSorterSets = new CauseType() { Name = "reference", CauseTypeGroup = ctgSorterSets }.AddId();
            causeTypeRndSorterSetsBySwitch = new CauseType() { Name = "randomBySwitch", CauseTypeGroup = ctgRndSorterSets }.AddId();
            causeTypeRndSorterSetsByStage = new CauseType() { Name = "randomByStage", CauseTypeGroup = ctgRndSorterSets }.AddId();
            causeTypeRndSorterSetsBySymetricStage = new CauseType() { Name = "randomBySymetricStage", CauseTypeGroup = ctgRndSorterSets }.AddId();
            causeTypeRndSorterSetsBySymetricStageBuddies = new CauseType() { Name = "randomBySymetricStageBuddies", CauseTypeGroup = ctgRndSorterSets }.AddId();
            causeTypeSorterSetPerfBinsBySortableSet = new CauseType() { Name = "sorterSetPerfBinsBySortableSet", CauseTypeGroup = ctgSorterPerfBins }.AddId();


            gortContext.CauseTypes.Add(causeTypeStdSortables);
            gortContext.CauseTypes.Add(causeTypeRefSorterSets);
            gortContext.CauseTypes.Add(causeTypeRndSorterSetsBySwitch);
            gortContext.CauseTypes.Add(causeTypeRndSorterSetsByStage);
            gortContext.CauseTypes.Add(causeTypeRndSorterSetsBySymetricStage);
            gortContext.CauseTypes.Add(causeTypeRndSorterSetsBySymetricStageBuddies);
            gortContext.CauseTypes.Add(causeTypeSorterSetPerfBinsBySortableSet);
        }



        public static void GetCauseTypeParams(GortContext gortContext)
        {
            causeTypeParamStdSortables_MinDegree = gortContext.CauseTypeParams.Where(g => g.Name == "minDegree").First();
            causeTypeParamStdSortables_MaxDegree = gortContext.CauseTypeParams.Where(g => g.Name == "maxDegree").First();
            causeTypeParamRefSorterSets_Name = gortContext.CauseTypeParams.Where(g => (g.Name == "name") && (g.CauseTypeId == causeTypeRefSorterSets.CauseTypeId)).First();
            causeTypeParamStdSortables_Name = gortContext.CauseTypeParams.Where(g => (g.Name == "name") && (g.CauseTypeId == causeTypeStdSortables.CauseTypeId)).First();

            causeTypeParamRndSorterSetsBySwitch_Degree = gortContext.CauseTypeParams.Where(g => (g.Name == "degree") && (g.CauseTypeId == causeTypeRndSorterSetsBySwitch.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySwitch_SwitchCount = gortContext.CauseTypeParams.Where(g => (g.Name == "switchCount") && (true)).First();
            causeTypeParamRndSorterSetsBySwitch_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "rndGen") && (g.CauseTypeId == causeTypeRndSorterSetsBySwitch.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySwitch_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySwitch.CauseTypeId)).First();

            causeTypeParamRndSorterSetsByStage_Degree = gortContext.CauseTypeParams.Where(g => (g.Name == "degree") && (g.CauseTypeId == causeTypeRndSorterSetsByStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsByStage_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "stageCount") && (g.CauseTypeId == causeTypeRndSorterSetsByStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsByStage_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "rndGen") && (g.CauseTypeId == causeTypeRndSorterSetsByStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsByStage_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterCount") && (g.CauseTypeId == causeTypeRndSorterSetsByStage.CauseTypeId)).First();

            causeTypeParamRndSorterSetsBySymmetricStage_Degree = gortContext.CauseTypeParams.Where(g => (g.Name == "degree") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStage_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "stageCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStage_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "rndGen") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStage.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStage_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStage.CauseTypeId)).First();

            causeTypeParamRndSorterSetsBySymmetricStageBuddies_Degree = gortContext.CauseTypeParams.Where(g => (g.Name == "degree") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_StageCount = gortContext.CauseTypeParams.Where(g => (g.Name == "stageCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_RndGen = gortContext.CauseTypeParams.Where(g => (g.Name == "rndGen") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_SorterCount = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStageBuddies.CauseTypeId)).First();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_BuddyCount = gortContext.CauseTypeParams.Where(g => (g.Name == "buddyCount") && (g.CauseTypeId == causeTypeRndSorterSetsBySymetricStageBuddies.CauseTypeId)).First();

            causeTypeParamSorterSetPerfBinsBySortableSet_SorterSet = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterSet") && (g.CauseTypeId == causeTypeSorterSetPerfBinsBySortableSet.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBinsBySortableSet_SortableSet = gortContext.CauseTypeParams.Where(g => (g.Name == "sortableSet") && (g.CauseTypeId == causeTypeSorterSetPerfBinsBySortableSet.CauseTypeId)).First();
            causeTypeParamSorterSetPerfBinsBySortableSet_SorterSaveMode = gortContext.CauseTypeParams.Where(g => (g.Name == "sorterSaveMode") && (g.CauseTypeId == causeTypeSorterSetPerfBinsBySortableSet.CauseTypeId)).First();
        }


        public static void AddCauseTypeParams(GortContext gortContext)
        {
            causeTypeParamStdSortables_MinDegree = new CauseTypeParam() { Name = "minDegree", CauseType = causeTypeStdSortables, DataType = DataType.Integer }.AddId();
            causeTypeParamStdSortables_MaxDegree = new CauseTypeParam() { Name = "maxDegree", CauseType = causeTypeStdSortables, DataType = DataType.Integer }.AddId();

            causeTypeParamRefSorterSets_Name = new CauseTypeParam() { Name = "name", CauseType = causeTypeRefSorterSets, DataType = DataType.String }.AddId();

            causeTypeParamStdSortables_Name = new CauseTypeParam() { Name = "name", CauseType = causeTypeStdSortables, DataType = DataType.String }.AddId();

            causeTypeParamRndSorterSetsBySwitch_Degree = new CauseTypeParam() { Name = "degree", CauseType = causeTypeRndSorterSetsBySwitch, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySwitch_SwitchCount = new CauseTypeParam() { Name = "switchCount", CauseType = causeTypeRndSorterSetsBySwitch, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySwitch_RndGen = new CauseTypeParam() { Name = "rndGen", CauseType = causeTypeRndSorterSetsBySwitch, DataType = DataType.Rng }.AddId();
            causeTypeParamRndSorterSetsBySwitch_SorterCount = new CauseTypeParam() { Name = "sorterCount", CauseType = causeTypeRndSorterSetsBySwitch, DataType = DataType.Rng }.AddId();

            causeTypeParamRndSorterSetsByStage_Degree = new CauseTypeParam() { Name = "degree", CauseType = causeTypeRndSorterSetsByStage, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsByStage_StageCount = new CauseTypeParam() { Name = "stageCount", CauseType = causeTypeRndSorterSetsByStage, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsByStage_RndGen = new CauseTypeParam() { Name = "rndGen", CauseType = causeTypeRndSorterSetsByStage, DataType = DataType.Rng }.AddId();
            causeTypeParamRndSorterSetsByStage_SorterCount = new CauseTypeParam() { Name = "sorterCount", CauseType = causeTypeRndSorterSetsByStage, DataType = DataType.Integer }.AddId();

            causeTypeParamRndSorterSetsBySymmetricStage_Degree = new CauseTypeParam() { Name = "degree", CauseType = causeTypeRndSorterSetsBySymetricStage, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStage_StageCount = new CauseTypeParam() { Name = "stageCount", CauseType = causeTypeRndSorterSetsBySymetricStage, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStage_RndGen = new CauseTypeParam() { Name = "rndGen", CauseType = causeTypeRndSorterSetsBySymetricStage, DataType = DataType.Rng }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStage_SorterCount = new CauseTypeParam() { Name = "sorterCount", CauseType = causeTypeRndSorterSetsBySymetricStage, DataType = DataType.Integer }.AddId();

            causeTypeParamRndSorterSetsBySymmetricStageBuddies_Degree = new CauseTypeParam() { Name = "degree", CauseType = causeTypeRndSorterSetsBySymetricStageBuddies, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_StageCount = new CauseTypeParam() { Name = "stageCount", CauseType = causeTypeRndSorterSetsBySymetricStageBuddies, DataType = DataType.Integer }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_RndGen = new CauseTypeParam() { Name = "rndGen", CauseType = causeTypeRndSorterSetsBySymetricStageBuddies, DataType = DataType.Rng }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_SorterCount = new CauseTypeParam() { Name = "sorterCount", CauseType = causeTypeRndSorterSetsBySymetricStageBuddies, DataType = DataType.Rng }.AddId();
            causeTypeParamRndSorterSetsBySymmetricStageBuddies_BuddyCount = new CauseTypeParam() { Name = "buddyCount", CauseType = causeTypeRndSorterSetsBySymetricStageBuddies, DataType = DataType.Integer }.AddId();

            causeTypeParamSorterSetPerfBinsBySortableSet_SorterSet = new CauseTypeParam() { Name = "sorterSet", CauseType = causeTypeSorterSetPerfBinsBySortableSet, DataType = DataType.SorterSet }.AddId();
            causeTypeParamSorterSetPerfBinsBySortableSet_SortableSet = new CauseTypeParam() { Name = "sortableSet", CauseType = causeTypeSorterSetPerfBinsBySortableSet, DataType = DataType.SortableSet }.AddId();
            causeTypeParamSorterSetPerfBinsBySortableSet_SorterSaveMode = new CauseTypeParam() { Name = "sorterSaveMode", CauseType = causeTypeSorterSetPerfBinsBySortableSet, DataType = DataType.String }.AddId();

            gortContext.CauseTypeParams.Add(causeTypeParamStdSortables_MinDegree);
            gortContext.CauseTypeParams.Add(causeTypeParamStdSortables_MaxDegree);

            gortContext.CauseTypeParams.Add(causeTypeParamRefSorterSets_Name);

            gortContext.CauseTypeParams.Add(causeTypeParamStdSortables_Name);

            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySwitch_Degree);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySwitch_SwitchCount);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySwitch_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySwitch_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsByStage_Degree);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsByStage_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsByStage_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsByStage_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStage_Degree);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStage_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStage_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStage_SorterCount);

            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStageBuddies_Degree);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStageBuddies_StageCount);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStageBuddies_RndGen);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStageBuddies_SorterCount);
            gortContext.CauseTypeParams.Add(causeTypeParamRndSorterSetsBySymmetricStageBuddies_BuddyCount);

            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBinsBySortableSet_SorterSet);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBinsBySortableSet_SortableSet);
            gortContext.CauseTypeParams.Add(causeTypeParamSorterSetPerfBinsBySortableSet_SorterSaveMode);

        }


    }
}
