namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type SortableSetFixture () =

    [<TestMethod>]
    member this.makeAllBits () =
        let degree = Degree.create 16 |> Result.ExtractOrThrow


        Assert.IsTrue(true);


    [<TestMethod>]
    member this.makeExplicitBp64 () =
        let degree = Degree.create 16 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (RandomSeed.fromNow())
        let perm = Permutation.createRandom degree randy
        let orbit = Permutation.powers perm
                        |> Seq.toArray


        //let byteData =  orbit |> Permutation.arrayToBytes
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.makeOrbit () =
        let degree = Degree.create 16 |> Result.ExtractOrThrow
        let randy = Rando.create rngType.Lcg (RandomSeed.fromNow())
        let perm = Permutation.createRandom degree randy
        //let byteStore = perm |> Permutation.toBytes |> Result.ExtractOrThrow
        //let sortableSet = SortableSet.makeOrbiInts degree byteStore |> Result.ExtractOrThrow
        Assert.IsTrue(true);


    //[<TestMethod>]
    //member this.makeStack () =
    //    let degree = Degree.create 16 |> Result.ExtractOrThrow
    //    let degrees = [|Degree.create 8; Degree.create 4; Degree.create 2; Degree.create 2|]
    //                  |> Array.toList
    //                  |> Result.sequence
    //                  |> Result.ExtractOrThrow
    //                  |> List.toArray
    //    let offset = 7
    //    let mutable byteStore = Array.zeroCreate<byte> (offset + 8)
    //    byteStore <- ByteArray.degreeArrayToBytes byteStore offset degrees
    //                 |> Result.ExtractOrThrow
    //    let degreeSection = byteStore.[offset ..]
    //    let res = SortableSet.makeStackInts degree degreeSection
    //              |> Result.ExtractOrThrow

    //    Assert.IsTrue(true);