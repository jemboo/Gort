namespace Gort.Core.Test
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type RndGenFixture () =

    [<TestMethod>]
    member this.fromWeightedDistribution() =
        let testArray = [|2.0; 3.0; 4.0; 5.0;|]
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let weightFunction (w:float) =
            1.0 / w
        let res0 = testArray 
                    |> (RandVars.fromWeightedDistribution weightFunction rndy)
                    |> Seq.take 1000 |> Seq.toArray
        let res = testArray 
                    |> (RandVars.fromWeightedDistribution weightFunction rndy)
                    |> Seq.take 1000 |> Seq.toArray
                    |> Array.groupBy(id)
                    |> Array.sortBy(fst)
                    |> Array.map(fun tup -> (fst tup, (snd tup).Length) )

        Assert.IsTrue (res.Length = testArray.Length)


    [<TestMethod>]
    member this.rndTwoCycleArray() =
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let arraySize = 16
        let cycleCount = 2
        let block = RandVars.rndTwoCycle rndy arraySize cycleCount
        Assert.IsTrue (CollectionProps.isTwoCycle block)
        let cycleCount = 8
        let block2 = RandVars.rndTwoCycle rndy arraySize cycleCount
        Assert.IsTrue (CollectionProps.isTwoCycle block2)


    [<TestMethod>]
    member this.rndNchooseM() =
        let n = 5
        let m = 3
        let rndy = Rando.fromRngGen (RngGen.lcgFromNow())
        let numDraws = 300
        let res = RandVars.rndNchooseM n m rndy
                  |> Seq.take numDraws
                  |> Seq.toArray
                  |> Array.groupBy(id)
                  |> Array.map(fun tup -> (fst tup, tup |> snd |> Array.length))
        Assert.IsTrue (res.Length = 10)
