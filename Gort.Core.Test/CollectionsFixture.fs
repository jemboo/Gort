namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CollectionsFixture () =

    [<TestMethod>]
    member this.isSorted_idiom () =
        let aSrted = [|0;1;2;3|]
        let aRes = aSrted |> Collections.isSorted_idiom
        Assert.IsTrue(aRes)

        let aUnSrted = [|1; 2; 3; 0|]
        let aUnRes = aUnSrted |> Collections.isSorted_idiom
        Assert.IsFalse(aUnRes)

        let aUnSrted2 = [|1; 2; 0; 3;|]
        let aUnRes2 = aUnSrted2 |> Collections.isSorted_idiom
        Assert.IsFalse(aUnRes2)



    [<TestMethod>]
    member this.isSorted () =
        let aSrted = [|0;1;2;3|]
        let aRes = aSrted |> Collections.isSorted
        Assert.IsTrue(aRes)

        let aUnSrted = [|1; 2; 3; 0|]
        let aUnRes = aUnSrted |> Collections.isSorted
        Assert.IsFalse(aUnRes)

        let aUnSrted2 = [|1; 2; 0; 3;|]
        let aUnRes2 = aUnSrted2 |> Collections.isSorted
        Assert.IsFalse(aUnRes2)

