namespace Gort.Core.Test

open System
open SysExt
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CollectionsFixture () =


    [<TestMethod>]
    member this.takeUpto () =
        let a1 = [|1;2;3;4;5;6;7;8|]
        let yab = a1 |> Collections.takeUpto 3 |> Seq.toArray
        let zab = a1 |> Collections.takeUpto 30 |> Seq.toArray
        Assert.IsTrue(yab.Length = 3)
        Assert.IsTrue(zab.Length = 8)

    [<TestMethod>]
    member this.arrayEquals () =
        let a1 = [|1;2;3;|]
        let a2 = [|1;2;3;|]
        let a3 = [|1;2;4;|]
        
        Assert.IsTrue(Collections.arrayEquals a1 a2)
        Assert.IsFalse(Collections.arrayEquals a1 a3)


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

