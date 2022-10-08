namespace Gort.Run.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.DataStore.DataModel
open Gort.DataStore.CauseBuild

[<TestClass>]
type LoadFixture() =

    let WorkspaceName = "Ralph4"

    let makeWorkspace () =
        let ws = new Workspace()
        ws.Name <- WorkspaceName
        let wsI = ws.AddId()
        wsI

    let selectWorkSpace (ws: Workspace) = ws.Name = WorkspaceName

    [<TestMethod>]
    member this.LoadCauseBuilder() =

        let ctx = new GortContext2()
        let ws = ctx.Workspace.GetOrMake(selectWorkSpace, makeWorkspace)
        let paramSeed = ParamUtils.MakeParam("Seed", ParamDataType.Int32, 123)
        let paramRngType = ParamUtils.MakeParam("RngType", ParamDataType.Int32, 123)
        let cbRndGen = new CauseBuildRndGen(paramSeed, paramRngType, ws, 1)
        let yow = Load.LoadCauseBuilder cbRndGen ctx
        Assert.IsTrue(true)


    [<TestMethod>]
    member this.TestMethodPassing() = Assert.IsTrue(true)
