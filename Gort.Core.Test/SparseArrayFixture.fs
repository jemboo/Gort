namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SparseArrayFixture() =

    [<TestMethod>]
    member this.Convert() = 
        let arrayIn = [|1;2;0;0;3|]
        let sparsed = SparseArray.fromArray 0 arrayIn
        let arrayOut = SparseArray.toArray sparsed
        Assert.IsTrue(CollectionProps.areEqual arrayIn arrayOut)
