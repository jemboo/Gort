namespace Gort.Cause.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CauseOpsFixture () =

    let gu = Guid.Parse("fd100b52-f74e-8930-3cef-bc1f657a82a5");
    
    let texto = new Gort.Data.DataModel.GortContext()

    [<TestMethod>]
    member this.GetAllCausesForWorkspace () =
        let czs = CauseOps.GetAllCausesForWorkspace texto "WorkspaceRand" 
                   |> Result.ExtractOrThrow

        Assert.IsTrue(czs.Length > 0);


    [<TestMethod>]
    member this.GetCauseById () =
        let czs = CauseOps.GetCauseById texto gu 
                   |> Result.ExtractOrThrow
        Assert.IsTrue(czs.Index > 0);


    [<TestMethod>]
    member this.GetCauseTypeGroupAncestry () =
        let cz = CauseOps.GetCauseById texto gu 
                   |> Result.ExtractOrThrow
        let cestry = CauseOps.GetCauseTypeGroupAncestry texto cz 
                   |> Result.ExtractOrThrow
        Assert.IsTrue(cestry.Length > 0);


    [<TestMethod>]
    member this.GetCauseDispatcherInfo () =
        let yark = Gort.Data.DataModel.CauseTypeName.Rng.ToString()
        let cz, cestry = CauseOps.GetCauseDispatcherInfo texto gu 
                           |> Result.ExtractOrThrow
        Assert.IsTrue(cestry.Length > 0);