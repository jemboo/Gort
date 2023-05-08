namespace global

open System

type sorterUniformMutatorType =
    | Switch
    | Stage
    | StageRfl

type sorterUniformMutator =
    private
        { 
          sumType: sorterUniformMutatorType
          mutationRate: mutationRate
          switchCountPfx: switchCount Option
          switchCountFinal: switchCount Option
          mFunc: sorter -> sorterId -> IRando -> Result<sorter, string> 
        }

module SorterUniformMutator =

    let private _create sorterUniformMutatorTyp mutationRat
                        switchCtPfx switchCtFinal mutationFun =
        { 
          sorterUniformMutator.sumType = sorterUniformMutatorTyp
          mutationRate = mutationRat
          mFunc = mutationFun
          switchCountPfx = switchCtPfx
          switchCountFinal = switchCtFinal
        }

    let getSorterUniformMutatorType (sum: sorterUniformMutator) = sum.sumType

    let getMutationRate (sum: sorterUniformMutator) = sum.mutationRate

    let getSorterMutator (sum: sorterUniformMutator) = sum.mFunc
    
    let getPrefixSwitchCount (sum: sorterUniformMutator) = sum.switchCountPfx
    
    let getFinalSwitchCount (sum: sorterUniformMutator) = sum.switchCountFinal

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


    let create (switchCtPrefix: switchCount Option)
               (switchCtTarget: switchCount Option)
               (sorterUniformMutatorTyp:sorterUniformMutatorType)  
               (mutRate: mutationRate) =
        match sorterUniformMutatorTyp with
        | Switch ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_switchMutator mutRate) switchCtPrefix switchCtTarget)
        | Stage ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_stageMutator mutRate) switchCtPrefix switchCtTarget)
        | StageRfl ->
            _create sorterUniformMutatorTyp mutRate switchCtPrefix switchCtTarget (_makeMutant (_stageRflMutator mutRate) switchCtPrefix switchCtTarget)


type sorterMutator = Uniform of sorterUniformMutator

module SorterMutator =
    
    let getSorterMutator 
            (sorterMutator:sorterMutator)
        =
        match sorterMutator with
        | Uniform sum -> sum |> SorterUniformMutator.getSorterMutator


    let makeMutants 
            (sorterMutator:sorterMutator) 
            (parents:sorter seq)
            (randy:IRando)
        =
        let _pid_mutant (parent:sorter) =
            result {
                let! mutant =
                    (sorterMutator |> getSorterMutator) parent (Guid.NewGuid() |> SorterId.create)  randy
                return
                    (
                        parent |> Sorter.getSorterId,
                        mutant
                    )
            }

        parents |> Seq.map (_pid_mutant)
                |> Seq.toList
                |> Result.sequence
        
