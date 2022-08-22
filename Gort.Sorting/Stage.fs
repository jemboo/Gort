namespace global
open System

type stageCover = {stageDex:int; lineCovs: int list}
module StageCover = 
    let collidesWith (sc:stageCover) (sw:switch) =
        (sc.lineCovs |> List.contains sw.low)
                ||
        (sc.lineCovs |> List.contains sw.hi)


    let rec digForCov (topCovs:stageCover list) (cov:stageCover) (scL:stageCover list) (sw:switch) =
        let _gatherRes (topCovs:stageCover list) (cov:stageCover) (scL:stageCover list) (sw:switch) =
            let nextDex = cov.stageDex
            let nextCov = { stageCover.stageDex = nextDex; lineCovs = sw.low :: sw.hi :: cov.lineCovs }
            let covLst = scL |> List.append [nextCov]
                             |> List.append topCovs
            (covLst, (sw, nextDex))

        match scL with
        | [] -> _gatherRes topCovs cov scL sw
        | h::t -> 
           if (sw |> collidesWith h) then
              _gatherRes topCovs cov scL sw
           else
              let newTop = [cov] |> List.append topCovs
              digForCov newTop h t sw


    let addNewCovOrDig (cov:stageCover) (scL:stageCover list) (sw:switch) =
        if (sw |> collidesWith cov) then
            let nextDex = cov.stageDex + 1
            let nextCov = { stageCover.stageDex = nextDex; lineCovs = [sw.low; sw.hi] }
            (nextCov::cov::scL, (sw, nextDex))
        else
            digForCov [] cov scL sw


    let addStageIndexes (sws:switch seq) =
        let _initOrRun (scL:stageCover list) (sw:switch) =
            match scL with
            | [] -> ([{stageCover.stageDex=0; lineCovs = [sw.low; sw.hi]}], (sw, 0))
            | h::t -> addNewCovOrDig h t sw

        let mutable stgCvrs = []
        let swEnumer = sws.GetEnumerator();
        seq {while swEnumer.MoveNext() do
                let (nuCvr, dexed) = _initOrRun stgCvrs swEnumer.Current
                stgCvrs <- nuCvr
                yield dexed }

    let getStageCount (sws:switch seq) =
        sws |> addStageIndexes |> Seq.groupBy(snd) |> Seq.length |> StageCount.create



type stage = {switches:switch list; order:order}
module Stage =

    // returns a list of switches found in all of the stages
    let switchIntersection (stages:stage seq) =
        stages |> Seq.map(fun st -> (Set.ofList st.switches))
               |> Set.intersectMany
               |> Set.toList

    // returns the switches that are found more than once in the stages
    let switchPairwiseIntersections (stages:stage seq) =
        seq { for stage in stages do yield! stage.switches }
        |> CollectionProps.itemsOccuringMoreThanOnce


    let fromSwitches (order:order) 
                     (switches:seq<switch>) =
        let mutable stageTracker = Array.init (Order.value order) 
                                              (fun _ -> false)
        let switchesForStage = new ResizeArray<switch>()
        seq { 
              for sw in switches do
                 if (sw.hi <> sw.low) then
                     if (stageTracker.[sw.hi] || stageTracker.[sw.low] ) then
                        yield { stage.switches = switchesForStage |> Seq.toList; 
                                stage.order = order }
                        stageTracker <- Array.create (Order.value order) false
                        switchesForStage.Clear()
                     stageTracker.[sw.hi] <- true
                     stageTracker.[sw.low] <- true
                     switchesForStage.Add sw
              if switchesForStage.Count > 0 then
                yield { stage.switches = switchesForStage |> Seq.toList; 
                        order = order }
            }


    let getStageIndexesFromSwitches (order:order) 
                                    (switches:seq<switch>) =
        let mutable stageTracker = Array.init (Order.value order) 
                                              (fun _ -> false)
        let mutable curDex = 0
        seq { 
             yield curDex
             for sw in switches do
                if (stageTracker.[sw.hi] || stageTracker.[sw.low] ) then
                    yield curDex
                    stageTracker <- Array.create (Order.value order) false
                stageTracker.[sw.hi] <- true
                stageTracker.[sw.low] <- true
                curDex <- curDex + 1
             yield curDex
           }


    let getStageCount (order:order) 
                      (switches:seq<switch>) =
            fromSwitches order switches 
                    |> Seq.length
                    |> StageCount.create


    let convertToTwoCycle (stage:stage) =
        stage.switches |> Seq.map(fun s -> (s.low, s.hi))
                       |> TwoCycle.makeFromTupleSeq stage.order

                       
    let mutateStageByPair (stage:stage) 
                          (pair:int*int) =
        let tcpM = stage |> convertToTwoCycle 
                         |> TwoCycle.mutateByPair pair
        let sA = Switch.extractFromInts (tcpM |> TwoCycle.getArray) |> Seq.toList
        {switches=sA; order=stage.order}


    let mutateStageReflByPair (stage:stage) 
                              (pairs:seq<int*int>) =
        let tcpM = stage |> convertToTwoCycle 
                         |> TwoCycle.mutateByReflPair pairs
        let sA = Switch.extractFromInts (tcpM |> TwoCycle.getArray) |> Seq.toList
        {switches=sA; order=stage.order}


    // IRando dependent
    let rndSeq (order:order) 
               (switchFreq:switchFrequency) 
               (rnd:IRando) =

        let _aa (rnd:IRando)  = 
            {
                switches = TwoCycle.rndTwoCycle 
                                    order
                                    (SwitchFrequency.value switchFreq)
                                    rnd
                               |> Switch.fromTwoCycle
                               |> Seq.toList;
                order = order
            }
        seq { while true do yield (_aa rnd) }


    let rndSymmetric (order:order) (rnd:IRando) =
        let _aa (rnd:IRando)  = 
            { 
                stage.switches = 
                    TwoCycle.rndSymmetric 
                            order
                            rnd
                    |> Switch.fromTwoCycle
                    |> Seq.toList
                order = order
            }

        seq { while true do yield (_aa rnd) }


    let randomMutate (rnd:IRando) 
                     (mutationRate:stageMutationRate) 
                     (stage:stage) = 
        match rnd.NextFloat with
            | k when k < (StageMutationRate.value mutationRate) -> 
                        let symbolSetSize = stage.order |> Order.value |> uint64 |> SymbolSetSize.createNr
                        let tcp = RandVars.drawTwoWithoutRep symbolSetSize rnd
                        mutateStageByPair stage tcp
            | _ -> stage


    let randomReflMutate (rnd:IRando) 
                         (mutationRate:stageMutationRate) 
                         (stage:stage) = 
        match rnd.NextFloat with
            | k when k < (StageMutationRate.value mutationRate) -> 
                        let sc = stage.order |> Order.value |> uint64 |> SymbolSetSize.createNr
                        let tcp = seq { 
                                while true do 
                                yield RandVars.drawTwoWithoutRep sc rnd }
                        mutateStageReflByPair stage tcp 
            | _ -> stage


    let toBuddyStages  (stagesPfx: stage list)
                       (stageWindowSize: stageCount)
                       (stageSeq: seq<stage>)
                       (targetStageCount: stageCount)
                       (trialStageCount: stageCount) =

        let maxWindow = (StageCount.value stageWindowSize)
        let mutable window = stagesPfx |> CollectionProps.last maxWindow
        let trim() =
            if window.Length = maxWindow then
                window |> CollectionProps.first (maxWindow - 1)
            else    
                window

        let buddyCount (stage:stage) = 
            let testWin = stage::window
            switchPairwiseIntersections testWin
                            |> Seq.length
        let mutable stagesFound = 0
        let mutable stagesTested = 0
        let appendedStageCount = (StageCount.value targetStageCount) 
                                    - stagesPfx.Length

        let stager = stageSeq.GetEnumerator()
        seq { while ((stagesFound < appendedStageCount) 
                      && 
                     (stagesTested < (StageCount.value trialStageCount) )) 
               do
               window <- trim()
               stager.MoveNext() |> ignore
               stagesTested <- stagesTested + 1
               if (buddyCount stager.Current) = 0 then
                    window <- window |> List.append [stager.Current]
                    stagesFound <- stagesFound + 1
                    yield stager.Current
             }


    let rndBuddyStages (stageWindowSz:stageWindowSize)
                        (switchFreq:switchFrequency) 
                        (order:order) 
                        (rnd:IRando) 
                        (stagesPfx:stage list)  =
        let stageSeq = rndSeq order switchFreq rnd
        let maxWindow = (StageWindowSize.value stageWindowSz)
        let mutable window = stagesPfx |> CollectionProps.last maxWindow
        let trim() =
            if window.Length = maxWindow then
               window |> CollectionProps.first (maxWindow - 1)
            else    
               window

        let buddyCount (stage:stage) = 
            let testWin = stage::window
            let ahay = switchPairwiseIntersections testWin
                                    |> Seq.toArray
            let lenny = ahay |> Seq.length
            lenny

        seq { for stage in stageSeq do
                    window <- trim()
                    if (buddyCount stage) = 0 then
                        window <- window |> List.append [stage]
                        yield stage }
                    |> Seq.append
                           (stagesPfx |> List.toSeq)


    let rec rndSymmetricBuddyStages 
                (stageWindowSize:stageCount)
                (switchFreq:switchFrequency) 
                (order:order) 
                (rnd:IRando) 
                (stagesPfx:stage list)
                (trialStageCount:stageCount) 
                (stageCount:stageCount) =

         let trial = toBuddyStages stagesPfx
                        stageWindowSize
                        (rndSymmetric order rnd)
                        stageCount
                        trialStageCount
                        |> Seq.toArray
         if (trial.Length >= (StageCount.value stageCount)) then
            trial |> Array.take (StageCount.value stageCount)
         else rndSymmetricBuddyStages
                    stageWindowSize
                    switchFreq
                    order
                    rnd
                    stagesPfx
                    trialStageCount
                    stageCount



type indexedSelector<'V> = { array:('V * int)[] }

module IndexedSelector =

    let nextIndex<'V> 
            (selector:indexedSelector<'V>) 
            (qualifier:'V->bool)
            (rnd:IRando) =

        let _choose (rando:IRando) (items:'T[]) =
            if(items.Length > 0) then
                Some items.[rando.NextPositiveInt % items.Length]
            else
                None

        let candies = selector.array |> Array.filter(fun tup -> tup |> fst |> qualifier)
        candies |> _choose rnd |> Option.map snd



//type buddyTrack = { order:order;
//                    traces:CircularBuffer<bool*bool>[]; 
//                    buffSz:stageCount; }

//module BuddyTrack =

//    let make (order:order) 
//             (buffSz:stageCount) =
//        let tSide = (Order.value order)
//        let arrayLen = (tSide) * (tSide + 1) / 2
//        let cbs = Array.init 
//                    arrayLen 
//                    (fun _ -> CircularBuffer<bool*bool>(
//                                (false,false), 
//                                (StageCount.value buffSz)))
//        {
//            order = order;
//            buddyTrack.traces = cbs;
//            buffSz = buffSz;
//        }


//    let updateCb (index:int) 
//                 (lowVal:bool) 
//                 (hiVal:bool)
//                 (bt:buddyTrack) =
//        let low, hi = bt.traces.[index].Current
//        bt.traces.[index].SetCurrent (lowVal || low, hiVal || hi)


//    let update (bt:buddyTrack) 
//               (swDex:int) =
//        let switch = Switch.switchMap.[swDex]
//        let lds = Switch.lowOverlapping bt.order switch.low |> Seq.toArray
//        let hds = Switch.hiOverlapping bt.order switch.hi |> Seq.toArray
//        lds |> Array.map(fun dex -> updateCb dex true false bt) |> ignore
//        hds |> Array.map(fun dex -> updateCb dex false true bt) |> ignore
        

//    let prepNextStage (bt:buddyTrack) =
//        for dex = 0 to (bt.traces.Length - 1) do
//            let sw = Switch.switchMap.[dex]
//            if(sw.hi = sw.low) then
//                bt.traces.[dex].Push (true, true)
//            else
//                bt.traces.[dex].Push (false, false)
//        bt

//    let toSelector (bt:buddyTrack) =
//        { indexedSelector.array = bt.traces 
//                                  |> Array.mapi (fun dex v -> (v, dex)) }


//    let makeQualifier (depth:stageCount) = 
//        let stageDepth = (StageCount.value depth)
//        fun (cb:CircularBuffer<bool*bool>) ->
//            let lv, hv = cb.GetTick(0)
//            if (lv || hv) then false
//            else 
//               cb.LastNticks(stageDepth) 
//               |> Array.forall(fun (lv, hv) -> not (lv && hv))


//    let makeNextStage (bt:buddyTrack) 
//                      (depth:stageCount) 
//                      (randy:IRando) =

//        let _nextW (bt:buddyTrack) =
//            let selector = toSelector bt 
//            let wDex = IndexedSelector.nextIndex selector (makeQualifier depth) randy
//            match wDex with
//            | Some d ->  update bt d
//                         Some Switch.switchMap.[d]
//            | None -> None

//        bt |> prepNextStage |> ignore
//        seq { for dex = 0 to ((bt.order |> Order.maxSwitchesPerStage) - 1) do
//                   let wNx = _nextW bt
//                   if (wNx |> Option.isSome) then
//                    yield (wNx |> Option.get)  }

