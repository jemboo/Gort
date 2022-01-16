namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type RandoFixture () =

    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
