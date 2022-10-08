namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type BitPackFixture() =

    [<TestMethod>]
    member this.fromIntArrays() =
        let arrayLen = 7 |> ArrayLength.createNr
        let symbolSetSize = (1111111uL) |> SymbolSetSize.create |> Result.ExtractOrThrow

        let sortedArrays =
            [| [| 0; 1; 3; 4; 5; 6; 9 |]
               [| 110; 221; 333; 444; 5555; 6666; 7779 |]
               [| 10; 21; 43; 64; 85; 96; 109 |]
               [| 1; 11; 31; 41; 51; 61; 91 |] |]

        let bitPack =
            sortedArrays |> BitPack.fromIntArrays symbolSetSize |> Result.ExtractOrThrow

        let sortedArraysBack =
            bitPack |> BitPack.toIntArrays arrayLen |> Result.ExtractOrThrow

        Assert.IsTrue(CollectionProps.areEqual sortedArrays sortedArraysBack)


    [<TestMethod>]
    member this.fromBoolArrays() =
        let arrayLen = 5 |> ArrayLength.createNr

        let boolArrays =
            [| [| true; true; false; false; true |]
               [| false; true; false; false; false |]
               [| false; true; true; false; true |]
               [| true; true; false; false; true |]
               [| false; true; false; false; false |]
               [| false; true; true; false; true |]
               [| true; true; false; false; true |]
               [| false; true; false; false; false |]
               [| false; true; true; false; true |] |]

        let bytePack = boolArrays |> BitPack.fromBoolArrays |> Result.ExtractOrThrow


        let boolArraysBack =
            bytePack |> BitPack.toBoolArrays arrayLen |> Result.ExtractOrThrow

        Assert.IsTrue(CollectionProps.areEqual boolArrays boolArraysBack)
