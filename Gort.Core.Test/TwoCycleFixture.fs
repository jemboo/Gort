namespace Gort.Core.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TwoCycleFixture () =

    [<TestMethod>]
    member this.TwoCyclePerm_makeFromTupleSeq() =        
        let seed = RandomSeed.fromNow()
        let iRando = Rando.fromRngGen (RngGen.createNet seed)
        let dg = Degree.createNr 16
        let stageTupes = seq {(0,1)}
        let twoCycle = TwoCycle.makeFromTupleSeq dg stageTupes
        Assert.AreEqual((TwoCycle.getArray twoCycle).Length, dg |> Degree.value)
        Assert.IsTrue(twoCycle |> TwoCycle.isATwoCycle)
        let stageTupes2 = seq {(0,1); (2,1)}
        let twoCycle2 = TwoCycle.makeFromTupleSeq dg stageTupes2
        Assert.AreEqual(twoCycle |> TwoCycle.getArray |> Array.toList, 
                        twoCycle2 |> TwoCycle.getArray |> Array.toList)


    [<TestMethod>]
    member this.TwoCyclePerm_MakeAllMonoCycles() =
        let seed = RandomSeed.fromNow()
        let dg = Degree.createNr 16
        let tcA = TwoCycle.makeAllMonoCycles dg
                  |> Seq.map(TwoCycle.toPerm)
                  |> Seq.toArray
        let allgood = tcA |> Array.forall(Permutation.isTwoCycle)
        Assert.AreEqual(Degree.switchCount dg, tcA.Length)
        Assert.IsTrue(allgood)


    [<TestMethod>]
    member this.TwoCyclePerm_rndTwoCycle() =
       let seed = RandomSeed.fromNow()
       let iRando = Rando.fromRngGen (RngGen.createNet seed)
       let dg = Degree.createNr 16
       let switchFreq = 0.5
       for i in {0 .. 20} do
                let tcp = TwoCycle.rndTwoCycle dg switchFreq iRando 
                Assert.IsTrue(tcp |> TwoCycle.toPerm |> Permutation.isTwoCycle)


    [<TestMethod>]
    member this.TwoCyclePerm_makeMode1() =
       let dg = Degree.createNr 16
       let aa = TwoCycle.oddMode dg
       let ac = TwoCycle.oddModeWithCap dg
       let acv = TwoCycle.getArray ac
       Assert.IsTrue(acv.Length > 0)

       
    [<TestMethod>]
    member this.TwoCyclePerm_makeCoConjugateEvenOdd() =
       let dg = Degree.createNr 16
       let permLst1 = [Permutation.identity dg; Permutation.identity dg]
       let aa = TwoCycle.makeCoConjugateEvenOdd permLst1
                |> Result.ExtractOrThrow
       let al = aa |> Seq.toList
       Assert.IsTrue(al.Length > 0)


    [<TestMethod>]
    member this.TwoCyclePerm_makeReflSymmetric() =
        let seed = RandomSeed.fromNow()
        let iRando = Rando.fromRngGen (RngGen.createNet seed)
        let dg = Degree.createNr 16
        let symTwoCs = Array.init 100 (fun _ -> TwoCycle.rndSymmetric dg iRando)
        let rr = symTwoCs |> Array.map(TwoCycle.isRflSymmetric)
                          |> Array.countBy(id)
        Assert.AreEqual(rr.Length, 1)

        let rflCmps = symTwoCs |> Array.map(fun tc -> tc = (TwoCycle.reflect tc))
                               |> Seq.countBy(id)
                               |> Seq.toArray
        Assert.AreEqual(rflCmps.Length, 1)



    [<TestMethod>]
    member this.TwoCyclePerm_mutateReflSymmetric() =
        let seed = RandomSeed.fromNow()
        let iRando = Rando.fromRngGen (RngGen.createNet seed)
        let dg = Degree.createNr 16
        let refSyms = Array.init 1000 (fun _ -> TwoCycle.rndSymmetric dg  iRando)

        let refMuts = 
            refSyms |> Array.map(fun p ->
                let tcp = seq { 
                            while true do 
                                yield RandGen.drawTwoWithoutRep dg iRando }
                TwoCycle.mutateByReflPair tcp p)

        let mutReflBins = 
            refMuts |> Array.map(fun tcp -> 
                 (tcp, tcp = (tcp |> TwoCycle.reflect)))
                 |> Array.countBy(snd)

        Assert.AreEqual(mutReflBins.Length, 1)


    [<TestMethod>]
    member this.TwoCycleGen_evenMode() =
     let evenDegree = Degree.createNr 16
     let resE = TwoCycle.evenMode evenDegree
     Assert.IsTrue ((TwoCycle.getArray resE).Length = (Degree.value evenDegree))
     let oddDegree = Degree.createNr 15
     let resO = TwoCycle.evenMode oddDegree
     Assert.IsTrue ((TwoCycle.getArray resO).Length = (Degree.value oddDegree))



    [<TestMethod>]
    member this.TestMethodPassing () =
        Assert.IsTrue(true);
