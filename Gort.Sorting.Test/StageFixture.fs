namespace Gort.Sorting.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type StageFixture () =

    [<TestMethod>]
    member this.tst () =
        Assert.IsTrue(true);


    [<TestMethod>]
    member this.addStageIndexes () =
        let deg = Degree.createNr 32
        let l = [1 .. 200]
        let newRes = l |> Switch.fromIndexesNonDeg 
                  |> StageCover.addStageIndexes
                  |> Seq.groupBy(snd)
                  |> Seq.toArray

        let oldRes = l |> Switch.fromIndexesNonDeg 
                   |> Stage.fromSwitches deg
                   |> Seq.toArray


        Assert.IsTrue(newRes.Length < oldRes.Length);


    [<TestMethod>]
    member this.Stage_switchIntersection() =
        let degree = Degree.createNr 16
        let randy = RngGen.createLcg (RandomSeed.create 1234) 
                        |> Rando.fromRngGen
        let stageCount = StageCount.create 2

        let startingStages() = 
            Stage.rndSymmetric
                        degree
                        randy
                |> Seq.take (StageCount.value stageCount)
                |> Stage.switchIntersection
                |> List.length

        for i=0 to 100 do
            Console.WriteLine (sprintf "%d" (startingStages()))

        Assert.IsTrue(1 > 0);



    [<TestMethod>]
    member this.Stage_switchPairwiseIntersections() =
        let degree = Degree.createNr 16
        let randy = RngGen.createLcg (RandomSeed.create 1234) |> Rando.fromRngGen
        let stageCount = StageCount.create 4

        let startingStages() = 
            Stage.rndSymmetric degree randy
                |> Seq.take (StageCount.value stageCount)
                |> Stage.switchPairwiseIntersections
                |> Seq.length

        let res = Seq.init 1000 (fun _ -> startingStages())

        let hist = CollectionOps.histogram (id) res
        Assert.IsTrue(hist.Count > 0);
    

    //[<TestMethod>]
    //member this.Stage_windowBuddies() =
    //    let degree = Degree.create 16
    //    let randy = RngGen.createLcg 1234 |> Rando.fromRngGen
    //    let stageCount = StageCount.create 10
    //    let windowSize = 4

    //    let startingStages = 
    //        Stage.makeRandomReflSymmetricStages
    //                    degree
    //                    randy
    //            |> Seq.take (StageCount.value stageCount)
    //            |> Seq.toArray

    //    let stageWindows = 
    //        startingStages 
    //            |> Stage.windowBuddies windowSize
    //            |> Seq.toArray

    //    Assert.IsTrue(stageWindows.Length > 0);


//[<TestMethod>]
//member this.Stage_buddyStages() =
//    let degree = Degree.createNr 16
//    let randy = RngGen.createLcg 1234 |> Rando.fromRngGen
//    let stageWindowSize = StageCount.fromInt 10
//    let windowSize = 4

//    let buddyStages = Stage.makeBuddyStages 
//                        stageWindowSize
//                        SwitchFrequency.max
//                        degree
//                        randy
//                        List.Empty 
//                     |> Seq.take 100
//                     |> Seq.toArray

//    let buddySwitches = 
//        buddyStages 
//            |> Stage.windowBuddies windowSize
//            |> Seq.toArray
//    let count = buddySwitches |> Array.sumBy(List.length)

//    Assert.AreEqual(count, 0);


//[<TestMethod>]
//member this.Stage_buddyStages2() =
//    let degree = Degree.createNr 10
//    let randy = RngGen.createLcg 1234 |> Rando.fromRngGen
//    let stageWindowSize = StageCount.fromInt 4
//    let maxStageTry = (StageCount.fromInt 10000)
//    let sampleCount = 1000

//    let (occCum, totCum) = 
//                    Stage.makeReflBuddyStats
//                        stageWindowSize
//                        degree
//                        randy
//                        maxStageTry
//                        sampleCount
                             

//    let occCumTegral = occCum |> Seq.scan (fun c v -> c + (v |> float) / (sampleCount |> float) ) 0.0
//                              |> Seq.toArray

//    totCum |> Array.iteri(fun i v -> Console.WriteLine (
//                                            sprintf "%d\t%d\t%d\t%d"
//                                                (Degree.value degree) 
//                                                (StageCount.value stageWindowSize)
//                                                i
//                                                v))
//    Assert.AreEqual(1, 1);



    [<TestMethod>]
    member this.rndSymmetricBuddyStages() =
        let degree = Degree.createNr 10
        let randy = RngGen.createLcg (RandomSeed.create 7234) |> Rando.fromRngGen
        let stageWindowSize = StageCount.create 4
        let maxStageTry = (StageCount.create 1200)
        let stageCount = (StageCount.create 100)
        let buddyStages() = 
            Stage.rndSymmetricBuddyStages
                            stageWindowSize
                            SwitchFrequency.max
                            degree
                            randy
                            List.Empty 
                            maxStageTry
                            stageCount
        let aa = Array.init 100 (fun _ -> buddyStages())
        Assert.AreEqual(aa.Length, (StageCount.value stageCount));



    //[<TestMethod>]
    //member this.BuddyTrack_makeQualifier() =
    //    let degree = Degree.createNr 16
    //    let buffSz = StageCount.create 5
    //    let testDepth = StageCount.create 3
    //    let bt = BuddyTrack.make degree buffSz
    //    bt |> BuddyTrack.prepNextStage |> ignore
    //    let quali = BuddyTrack.makeQualifier testDepth
    //    let res = bt.traces.[1] |> quali
    //    Assert.IsTrue(res)
    //    bt |> BuddyTrack.updateCb 1 true false |> ignore
    //    let res2 = bt.traces.[1] |> quali
    //    Assert.IsFalse(res2)


    //[<TestMethod>]
    //member this.BuddyTrack_makeNextStage() =
    //    let randy = RngGen.createLcg (RandomSeed.create 234) 
    //                    |> Rando.fromRngGen
    //    let degree = Degree.createNr 24
    //    let buffSz = StageCount.create 4
    //    let testDepth = StageCount.create 4
    //    let bt = BuddyTrack.make degree buffSz

    //    let newStage() =
    //        {
    //            stage.degree = degree;
    //            switches = BuddyTrack.makeNextStage bt testDepth randy
    //                        |> Seq.toList
    //        }

    //    let stages = seq {0 .. 300} |> Seq.map(fun _ -> newStage())
    //                    |> Seq.toArray
    //    stages |> Seq.iter(fun t -> Console.WriteLine t.switches.Length)
    //    //let pairs = Stage.switchPairwiseIntersections stages
    //    //            |> Seq.toArray
    //    Assert.IsFalse(false)
