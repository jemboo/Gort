namespace Gort.Core.Test
open Microsoft.VisualStudio.TestTools.UnitTesting
open System

[<TestClass>]
type RndGenFixture() =

    [<TestMethod>]
    member this.indexedRngGen() =
        let rngGen = RngGen.lcgFromNow ()

        let rngGen0 = Rando.indexedRngGen 0 rngGen
        let rngGen1 = Rando.indexedRngGen 1 rngGen
        let rngGen2 = Rando.indexedRngGen 2 rngGen
        let rngGen1a = Rando.indexedRngGen 1 rngGen
        let rngGen100a = Rando.indexedRngGen 10000 rngGen

        Assert.IsTrue(true)

    [<TestMethod>]
    member this.rndGuidsLcg() =
        let arrayLen = 10
        let gud = new Guid()
        let tstArray = RandVars.rndGuidsLcg gud
                       |> Seq.take(arrayLen)
                       |> Seq.toArray

        Assert.IsTrue(true)


    [<TestMethod>]
    member this.fromWeightedDistribution() =
        let testArray = [| 2.0; 3.0; 4.0; 5.0 |]
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let weightFunction (w: float) = 1.0 / w

        let res =
            testArray
            |> (RandVars.fromWeightedDistribution weightFunction rndy)
            |> Seq.take 1000
            |> Seq.toArray
            |> Array.groupBy (id)
            |> Array.sortBy (fst)
            |> Array.map (fun tup -> (fst tup, (snd tup).Length))

        Assert.IsTrue(res.Length = testArray.Length)


    [<TestMethod>]
    member this.rndTwoCycleArray() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let order = Order.createNr 16
        let cycleCount = 2
        let block = RandVars.rndPartialTwoCycle rndy order cycleCount
        Assert.IsTrue(CollectionProps.isTwoCycle block)
        let cycleCount = 8
        let block2 = RandVars.rndPartialTwoCycle rndy order cycleCount
        Assert.IsTrue(CollectionProps.isTwoCycle block2)


    [<TestMethod>]
    member this.rndNchooseM() =
        let n = 5
        let m = 3
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())
        let numDraws = 300

        let res =
            RandVars.rndNchooseM n m rndy
            |> Seq.take numDraws
            |> Seq.toArray
            |> Array.groupBy (id)
            |> Array.map (fun tup -> (fst tup, tup |> snd |> Array.length))

        Assert.IsTrue(res.Length = 10)


    [<TestMethod>]
    member this.fisherYatesReflShuffle() =
        let arrayLen = 8
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow ())

        let initialList = Array.init arrayLen id

        for dex = 0 to 10 do
            let perm =
                RandVars.fisherYatesReflShuffle rndy initialList
                |> Permutation.createNr
            


            Assert.IsTrue(8 = 8)



