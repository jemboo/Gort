namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClasse () =

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
