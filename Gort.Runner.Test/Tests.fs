namespace Gort.Runner.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Gort.Data.Utils

[<TestClass>]
type TestClass() =

    [<TestMethod>]
    member this.TestMethodPassing() =
        let nextCause = CauseQuery.GetPendingCauseForWorkspace "WorkspaceRand"
        Assert.IsTrue(true)
