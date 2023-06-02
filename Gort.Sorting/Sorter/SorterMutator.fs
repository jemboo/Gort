namespace global

open System

type sorterUniformMutator =
    private
        { 
          switchGenMode: switchGenMode
          mutationRate: mutationRate
          switchCountPfx: switchCount Option
          switchCountFinal: switchCount Option
          mutatorFunc: sorter -> sorterId -> IRando -> Result<sorter, string> 
        }

module SorterUniformMutator =

    let private _create sorterUniformMutatorTyp mutationRat
                        switchCtPfx switchCtFinal mutatorFunc =
        { 
          sorterUniformMutator.switchGenMode = sorterUniformMutatorTyp
          mutationRate = mutationRat
          mutatorFunc = mutatorFunc
          switchCountPfx = switchCtPfx
          switchCountFinal = switchCtFinal
        }

    let getSwitchGenMode (sum: sorterUniformMutator) = sum.switchGenMode

    let getMutationRate (sum: sorterUniformMutator) = sum.mutationRate

    let getMutatorFunc (sum: sorterUniformMutator) = sum.mutatorFunc
    
    let getSwitchCountPrefix (sum: sorterUniformMutator) = sum.switchCountPfx
    
    let getSwitchCountFinal (sum: sorterUniformMutator) = sum.switchCountFinal

    let getMutatorId 
                (sum: sorterUniformMutator) 
        = 
        [| (sum.GetType()) :> obj;
           (sum.switchGenMode) :> obj;
           (sum.mutationRate) :> obj;
           (sum.switchCountPfx) :> obj;
           (sum.switchCountFinal) :> obj;
        |] 
            |> GuidUtils.guidFromObjs
            |> SortableSetId.create


    let _switchMutator
            (mutRate:mutationRate) 
            (sorter: sorter)
            (sorterD:sorterId)
            (randy: IRando)  
        =
        result {
            let newSwitches =
                Switch.mutateSwitches sorter.order mutRate randy sorter.switches
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
        }

    let _stageMutator
            (mutRate:mutationRate)
            (sorter: sorter)
            (sorterD:sorterId)
            (randy: IRando)
        =
        result {
            let mutantStages =
                sorter.switches
                |> Stage.fromSwitches sorter.order
                |> Seq.toArray
                |> Array.map (Stage.randomMutate randy mutRate)

            let newSwitches =
                [| for stage in mutantStages do
                        yield! stage.switches |]
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
        }

    let _stageRflMutator            
            (mutRate:mutationRate) 
            (sorter: sorter)          
            (sorterD:sorterId)  
            (randy: IRando)  
        =
        result {
            let mutantStages =
                sorter.switches
                |> Stage.fromSwitches sorter.order
                |> Seq.toArray
                |> Array.map (Stage.randomReflMutate randy mutRate)

            let newSwitches =
                [| for stage in mutantStages do
                        yield! stage.switches |]
                |> Seq.toArray

            return Sorter.fromSwitches sorterD (sorter |> Sorter.getOrder) newSwitches
        }


    let _makeMutant
            (mFunc: sorter -> sorterId -> IRando -> Result<sorter, string>)
            (switchCtPrefix: switchCount Option)
            (switchCtTarget: switchCount Option)
            (sorter: sorter)          
            (sorterD:sorterId)  
            (randy: IRando)  
        =
        result {
          let! mutantSortr = mFunc sorter sorterD randy
          let targetSwitchCt = 
            match switchCtTarget with
            | Some swct -> swct
            | None -> sorter |> Sorter.getSwitches 
                      |> Array.length |> SwitchCount.create

          let prefixSwitchCt = 
            match switchCtPrefix with
            | Some swct -> swct
            | None -> 0 |> SwitchCount.create

          return Sorter.crossOver prefixSwitchCt targetSwitchCt sorter mutantSortr sorterD randy
        }


    let create 
            (switchCtPrefix: switchCount Option)
            (switchCountFinal: switchCount Option)
            (switchGenMode:switchGenMode)  
            (mutRate: mutationRate) 
        =
        match switchGenMode with
        | Switch ->
            _create switchGenMode mutRate switchCtPrefix switchCountFinal (_makeMutant (_switchMutator mutRate) switchCtPrefix switchCountFinal)
        | Stage ->
            _create switchGenMode mutRate switchCtPrefix switchCountFinal (_makeMutant (_stageMutator mutRate) switchCtPrefix switchCountFinal)
        | StageSymmetric ->
            _create switchGenMode mutRate switchCtPrefix switchCountFinal (_makeMutant (_stageRflMutator mutRate) switchCtPrefix switchCountFinal)


type sorterMutator = 
      | Uniform of sorterUniformMutator

module SorterMutator =
    
    let getMutatorFunc 
            (sorterMutator:sorterMutator)
        =
        match sorterMutator with
        | Uniform sum -> sum |> SorterUniformMutator.getMutatorFunc


    let getMutatorId 
            (sorterMutator:sorterMutator)
        =
        match sorterMutator with
        | Uniform sum -> sum |> SorterUniformMutator.getMutatorId


    let getSwitchCountPfx
            (sorterMutator:sorterMutator)
        =
        match sorterMutator with
        | Uniform sum -> sum |> SorterUniformMutator.getMutatorFunc


    let getSwitchCountFinal
            (sorterMutator:sorterMutator)
        =
        match sorterMutator with
        | Uniform sum -> sum |> SorterUniformMutator.getSwitchCountFinal


    let makeMutants2 
            (sorterMutator:sorterMutator) 
            (randy:IRando)
            (parentIdMap:Map<sorterId, sorterParentId>)
            (sortersToMutate:sorter seq)
        =
        let sortersToMutateLookup = 
            sortersToMutate 
            |> Seq.map(fun s -> 
                (s |> Sorter.getSorterId |> SorterParentId.toSorterParentId, s))
            |> Map.ofSeq

        let _pid_mutant (childId:sorterId) =
            result {
                
                let parentSorterId = parentIdMap.Item childId
                let parentSorter = sortersToMutateLookup.Item parentSorterId
                let! mutant =
                    (sorterMutator |> getMutatorFunc)
                            parentSorter
                            childId
                            randy
                return mutant
            }

        parentIdMap 
        |> Map.toSeq
        |> Seq.map (fst >> _pid_mutant)
        |> Seq.toList
        |> Result.sequence


    let makeMutants
            (sorterMutator:sorterMutator) 
            (randy:IRando)
            (sorterCount:sorterCount)
            (parents:sorter seq)
        =
        let _pid_mutant (parent:sorter) =
            result {
                let! mutant =
                    (sorterMutator |> getMutatorFunc)
                            parent
                            (Guid.NewGuid() |> SorterId.create)
                            randy
                return
                    (  parent |> Sorter.getSorterId,
                       mutant                         )
            }

        parents |> CollectionOps.infinteLoop
                |> Seq.map (_pid_mutant)
                |> Seq.take (SorterCount.value sorterCount)
                |> Seq.toList
                |> Result.sequence
        
