namespace Gort.Cause.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.Data.Utils

[<TestClass>]
type ctrRootFixture () =

    let gu = Guid.Parse("fd100b52-f74e-8930-3cef-bc1f657a82a5");
    let workspaceName = "WorkspaceRand"
    let texto = new Gort.Data.DataModel.GortContext()

    [<TestMethod>]
    member this.RunCause () =
        let cz, cestry = CauseOps.GetCauseDispatcherInfo gu texto
                          |> Result.ExtractOrThrow
        let res = ctrRoot.RunCause cz (cestry |> Array.toList) texto
        Assert.IsTrue(true);

    [<TestMethod>]
    member this.RunNextCause () =
        let cauz = MetaDataUtils.GetNextCauseForWorkspace(workspaceName)
        let cz, cestry = CauseOps.GetCauseDispatcherInfo cauz.CauseId texto
                          |> Result.ExtractOrThrow
        let res = ctrRoot.RunCause cz (cestry |> Array.toList) texto
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.GetCauseById () =
        let czs = CauseOps.GetCauseById gu texto
                   |> Result.ExtractOrThrow
        Assert.IsTrue(czs.Index > 0);


    [<TestMethod>]
    member this.GetCauseTypeGroupAncestry () =
        let cz = CauseOps.GetCauseById gu texto
                   |> Result.ExtractOrThrow
        let cestry = CauseOps.GetCauseTypeGroupAncestry cz texto
                   |> Result.ExtractOrThrow
        Assert.IsTrue(cestry.Length > 0);


    [<TestMethod>]
    member this.GetCauseDispatcherInfo () =
        let cz, cestry = CauseOps.GetCauseDispatcherInfo gu texto
                           |> Result.ExtractOrThrow
        Assert.IsTrue(cestry.Length > 0);