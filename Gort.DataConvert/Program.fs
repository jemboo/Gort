open System
open System.IO


// For more information see https://aka.ms/fsharp-console-apps

let useParalll = true |> UseParallel.create
let order = Order.createNr 24
let wPfx = Seq.empty<switch>
let switchCt = SwitchCount.orderTo999SwitchCount order
let sorterCt = SorterCount.create 10
let rngGnSorterSeeds = RngGen.create rngType.Lcg (11288 |> RandomSeed.create)

let rolloutFormt = rolloutFormat.RfBs64
let sorterEvalMod = sorterEvalMode.DontCheckSuccess

let projectFolder = "C:\\GortFiles\\SubsetTest"
let randomSorterSetFile = "randomSorterSet.txt"
let randomSorterSetEvalsFile = "randomSorterSetEval.txt"

let makeRandomSorterSet() =
   let sorterSetId = Guid.NewGuid() |> SorterSetId.create
   let fileP = Path.Combine(projectFolder, randomSorterSetFile)

   let sorterSt = SorterSet.createRandomSwitches 
                    sorterSetId sorterCt order wPfx switchCt rngGnSorterSeeds
   let cereal = sorterSt |> SorterSetDto.toJson
   use streamW = new StreamWriter(fileP)
   streamW.WriteLine(cereal)


let getRandomSorterSet() =
    let fileP = Path.Combine(projectFolder, randomSorterSetFile)
    use streamW = new StreamReader(fileP)
    let cereal = streamW.ReadToEnd()
    let sorterSetBckR = cereal |> SorterSetDto.fromJson
    sorterSetBckR |> Result.ExtractOrThrow


let makeRandomSorterSetEvals(sorterSt:sorterSet) =
    let fileP = Path.Combine(projectFolder, randomSorterSetFile)
    let sortableStId = SortableSetId.create (Guid.NewGuid())

    let sortableSt =
       SortableSet.makeAllBits 
            sortableStId 
            rolloutFormt
            order 
            |> Result.ExtractOrThrow


    let sorterEvls =
        SorterSetEval.evalSorters 
            sorterEvalMod 
            sortableSt 
            (sorterSt |> SorterSet.getSorters)
            useParalll


    use streamW = new StreamWriter(fileP)
    let cereal = String.Empty
    streamW.WriteLine(cereal)
    


makeRandomSorterSet()
let sorterSet = getRandomSorterSet()
makeRandomSorterSetEvals(sorterSet)


printfn "Hello from F#"

