// See https://aka.ms/new-console-template for more information
using Gort.Data;
using Gort.Data.DataModel;
using Gort.Data.Instance;
using Gort.Data.Instance.CauseBuilder;
using Gort.Data.Instance.SeedParams;

const string WorkspaceName = "WorkspaceName";

var ctx = new GortContext();
var seedParams = new SeedParamsA();
WorkspaceLoad.LoadStatics(ctx);
WorkspaceLoad.LoadSeedParams(seedParams, ctx);

int rndGenCauseIndex = 1;
string rndGenCauseDescr = $"RndGen_{rndGenCauseIndex}";
var cbRand = new CbRand(
    workspaceName: WorkspaceName,
    causeIndex: rndGenCauseIndex,
    descr: rndGenCauseDescr,
    paramSeed: seedParams.RngSeed,
    paramRngType: seedParams.RngType);
WorkspaceLoad.LoadCauseBuilder(cbRand, ctx) ;

//var pramRngId = cbRand.GetParamRngId(ctx);

Console.WriteLine("Goodbye, World!");