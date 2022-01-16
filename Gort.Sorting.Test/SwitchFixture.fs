namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SwitchFixture () =

    [<TestMethod>]
    member this.IsThisStatic () =

        let yab = Switch.switchMap
        let yab2 = Switch.switchMap
        Assert.IsTrue(true);
