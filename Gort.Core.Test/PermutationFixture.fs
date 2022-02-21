namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type PermutationFixture () =

    [<TestMethod>]
    member this.Permutation_Identity() =
      let dg = Degree.createNr 16
      let expectedLen = (Degree.value dg)
      let expectedSum = ( expectedLen * (expectedLen - 1)) / 2
      let permutes = Permutation.identity  dg
      Assert.AreEqual(expectedLen, permutes |> Permutation.getArray |> Array.length)
      Assert.AreEqual(expectedSum, permutes |> Permutation.getArray |> Array.sum)


    [<TestMethod>]
    member this.Permutation_powers() =
        let dg = Degree.createNr 16
        let perm = Permutation.rotate dg 1
        let arA = perm |> Permutation.powers
                       |> Seq.toArray
        Assert.AreEqual (arA.Length, 16)


    [<TestMethod>]
    member this.Permutation_PowerDist() =
        let seed = RandomSeed.fromNow()
        let iRando = Rando.fromRngGen (RngGen.createNet seed)
        let dg = Degree.createNr 16
        let permCount = 1000
        let randPerms = Permutation.createRandoms dg iRando
                        |> CollectionOps.takeUpto permCount
                        |> Seq.map((Permutation.powers) >> Seq.toArray)
                        |> Seq.toArray

        let yabs = randPerms |> Array.countBy(fun po->po.Length)
                             |> Array.sortBy(snd)
        yabs |> Array.iter(fun tup -> Console.WriteLine (sprintf "%d\t%d" (fst tup) (snd tup)))
        
        Assert.IsTrue (randPerms.Length > 0)


    [<TestMethod>]
    member this.Permutation_Inverse() =
       let dg = Degree.createNr 16
       let perm = Permutation.rotate dg 1
       let inv = Permutation.inverse perm
       let prod = Permutation.productNr perm inv 
       let id = Permutation.identity dg
       Assert.AreEqual(id, prod)



    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
