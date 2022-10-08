namespace Gort.Cause.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.Data.Utils

[<TestClass>]
type ctrRootFixture() =

    let rndWorkspaceName = "WorkspaceRand"
    let causeOneRngID = Guid.Parse("fd100b52-f74e-8930-3cef-bc1f657a82a5")
    let rndSortableWorkspaceName = "WorkspaceRandSortable"
    let texto = new Gort.Data.DataModel.GortContext()

    [<TestMethod>]
    member this.RunCause() =
        let cz, cestry =
            CauseOps.GetCauseDispatcherInfo texto causeOneRngID |> Result.ExtractOrThrow

        let res = Run.RunCause cz (cestry |> Array.toList) texto
        Assert.IsTrue(true)

    [<TestMethod>]
    member this.RunNextCause() =
        let nextCause = CauseQuery.GetPendingCauseForWorkspace rndSortableWorkspaceName

        let gr =
            if nextCause <> null then
                let cz, cestry =
                    CauseOps.GetCauseDispatcherInfo texto nextCause.CauseId |> Result.ExtractOrThrow

                Run.RunCause cz (cestry |> Array.toList) texto
            else
                1 |> Result.Ok

        Assert.IsTrue(true)


    [<TestMethod>]
    member this.GetCauseById() =
        let czs = CauseOps.GetCauseById texto causeOneRngID |> Result.ExtractOrThrow
        Assert.IsTrue(czs.Index > 0)


    [<TestMethod>]
    member this.GetCauseTypeGroupAncestry() =
        let cz = CauseOps.GetCauseById texto causeOneRngID |> Result.ExtractOrThrow
        let cestry = CauseOps.GetCauseTypeGroupAncestry texto cz |> Result.ExtractOrThrow
        Assert.IsTrue(cestry.Length > 0)


    [<TestMethod>]
    member this.GetCauseDispatcherInfo() =
        let cz, cestry =
            CauseOps.GetCauseDispatcherInfo texto causeOneRngID |> Result.ExtractOrThrow

        Assert.IsTrue(cestry.Length > 0)
