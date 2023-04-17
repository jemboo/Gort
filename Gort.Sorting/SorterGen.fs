namespace global

open System

type switchMode =
    | Switch
    | Stage
    | StageSymmetric


type rndSorterGen =
    private
        { 
          order: order
          rngGen:rngGen
          switchMode:switchMode
          switchPfx: switch[]
         }

module SorterGen =

    let getSorterId (sortr: sorter) = sortr.sortrId

