namespace Gort.Core.Test

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type IntSetFixture () =

    [<TestMethod>]
    member this.product() =
        let permA = Permutation.create [|0;1;2;3;4;5;6;7;8;9;10|] |> Result.ExtractOrThrow
        let permB = Permutation.create [|10;9;2;3;4;5;6;7;8;1;0|] |> Result.ExtractOrThrow
        let bsa = Permutation.product permA permB |> Result.ExtractOrThrow
        let bsa2 = Permutation.productNr permA permB
        Assert.AreEqual(1, 1)


    [<TestMethod>]
    member this.randomCycles () =
        let maxSz = 100000
        let _cycT (perm:permutation) =
            perm |> Permutation.powers None
                 |> CollectionOps.takeUpto maxSz
                 |> Seq.length
                    
        let randy = Rando.create rngType.Lcg (RandomSeed.fromNow())
        let ord = Order.createNr 64
        let permCt = 100
        let randPerms = [| 1 .. permCt|] |> Array.map(fun _ -> Permutation.createRandom ord randy)
        let cts = randPerms |> Array.map(_cycT)
        Assert.AreEqual(1, 1)


    [<TestMethod>]
    member this.randomCycles2 () =
        let maxSz = 100
        let _cycT (perm:permutation) =
            perm |> Permutation.powers None
                 |> CollectionOps.takeUpto maxSz
                 |> Seq.map(fun p -> ByteUtils.allUint64s (Permutation.getArray p))
                 |> Seq.concat
                 |> Seq.toArray
        let _ctups (perm:permutation) =
            let pps = _cycT perm
            let cds = pps |> Array.distinct
            (pps.Length, cds.Length)

        let randy = Rando.create rngType.Lcg (RandomSeed.fromNow())
        let ord = Order.createNr 16
        let permCt = 1000
        let randPerms = [| 1 .. permCt|] |> Array.map(fun _ -> Permutation.createRandom ord randy)
        let cts = randPerms |> Array.map(_ctups)
        cts |> Array.iter(fun tup -> System.Diagnostics.Debug.WriteLine(sprintf "%d\t%d" (fst tup) (snd tup) ))
        Assert.AreEqual(1, 1)


    [<TestMethod>]
    member this.compIntArraysNr() =

        Assert.AreEqual(1, 1)

