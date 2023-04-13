open System
open System.IO


// For more information see https://aka.ms/fsharp-console-apps

let useParalll = true |> UseParallel.create
let order = Order.createNr 24


let rolloutFormatFull = rolloutFormat.RfBs64
let sorterEvalMod = sorterEvalMode.DontCheckSuccess

let projectFolder = "C:\\GortFiles\\SubsetTest24"

//////// Permutation sortable set ///////////

let fullBin16SortableSetId = "5a9384b0-9777-4caf-8abc-f56f5ff5dd99"
                            |> Guid.Parse
                            |> SortableSetId.create

let permSortableSetId_A = "dfe5e12c-f80a-4ce7-9f20-1a9fca6084db"
                            |> Guid.Parse
                            |> SortableSetId.create
let permSortableSetId_B = "4cf9aeeb-644f-404f-b164-1d1d1fb39199"
                            |> Guid.Parse
                            |> SortableSetId.create
let permSortableSetId_C = "9ba695f0-1649-4f14-ad54-6e2dcc747454"
                            |> Guid.Parse
                            |> SortableSetId.create

//let permA = Permutation.create [|8; 15; 2; 5; 14; 10; 12; 7; 11; 3; 1; 0; 9; 6; 4; 13|]
//            |> Result.ExtractOrThrow
//let permB = Permutation.create [|7; 5; 4; 0; 8; 2; 9; 3; 6; 12; 14; 11; 13; 10; 15; 1|]
//            |> Result.ExtractOrThrow
//let permC = Permutation.create [|9; 6; 11; 13; 14; 5; 0; 15; 10; 7; 12; 4; 8; 2; 1; 3|]
//            |> Result.ExtractOrThrow
let permA = Permutation.create [|16; 17; 18; 19; 20; 21; 22; 23; 8; 15; 2; 5; 14; 10; 12; 7; 11; 3; 1; 0; 9; 6; 4; 13|]
            |> Result.ExtractOrThrow
let permB = Permutation.create [|16; 17; 18; 19; 20; 21; 22; 23; 7; 5; 4; 0; 8; 2; 9; 3; 6; 12; 14; 11; 13; 10; 15; 1|]
            |> Result.ExtractOrThrow
let permC = Permutation.create [|16; 17; 18; 19; 20; 21; 22; 23; 9; 6; 11; 13; 14; 5; 0; 15; 10; 7; 12; 4; 8; 2; 1; 3|]
            |> Result.ExtractOrThrow

let randomSorterSetEvalsFileA = "randomSorterSetEval_A.txt"
let randomSorterSetEvalsFileB = "randomSorterSetEval_B.txt"
let randomSorterSetEvalsFileC = "randomSorterSetEval_C.txt"


let permSortableSetA = SortableSet.makeOrbits rolloutFormat.RfU8 None permA permSortableSetId_A
                        |> Result.ExtractOrThrow
let permSortableSetB = SortableSet.makeOrbits rolloutFormat.RfU8 None permB permSortableSetId_B
                        |> Result.ExtractOrThrow
let permSortableSetC = SortableSet.makeOrbits rolloutFormat.RfU8 None permC permSortableSetId_C
                        |> Result.ExtractOrThrow



//////////////   Random SorterSet  //////////////////
let randomSorterSetId = "ba4653e7-223f-4038-afac-3f3367598a71" 
                            |> Guid.Parse
                            |> SorterSetId.create
let randomSorterSetFullEvalsFile = "randomSorterSetFullEval.txt"
let wPfx = Seq.empty<switch>
let switchCt = SwitchCount.orderTo999SwitchCount order
let randomSorterSetSize = SorterCount.create 100
let rngGenRandomSorterSet = RngGen.create rngType.Lcg (11288 |> RandomSeed.create)
let randomSorterSetFile = "randomSorterSet.txt"



let makeRandomSorterSet() =
   let fileP = Path.Combine(projectFolder, randomSorterSetFile)

   let sorterSt = SorterSet.createRandomSwitches 
                    randomSorterSetId 
                    randomSorterSetSize 
                    order 
                    wPfx 
                    switchCt 
                    rngGenRandomSorterSet

   let cereal = sorterSt |> SorterSetDto.toJson
   use streamW = new StreamWriter(fileP)
   streamW.WriteLine(cereal)

let getRandomSorterSet() =
    let fileP = Path.Combine(projectFolder, randomSorterSetFile)
    use streamW = new StreamReader(fileP)
    let cereal = streamW.ReadToEnd()
    let sorterSetBckR = cereal |> SorterSetDto.fromJson
    sorterSetBckR |> Result.ExtractOrThrow

//////////////////////////////////////////


let makeFullSortableSet 
        (id:sortableSetId)
        =
       SortableSet.makeAllBits 
            id 
            rolloutFormatFull
            order 
            |> Result.ExtractOrThrow



///////////   SorterSet Eval  //////////////////

let makeSorterSetEvals
        (sorterSt:sorterSet) 
        (sortableSt:sortableSet)
        (fileN:string)
        =
    let fileP = Path.Combine(projectFolder, fileN)
    let sorterEvls =
        SorterSetEval.evalSorters 
            sorterEvalMod
            sortableSt 
            (sorterSt |> SorterSet.getSorters)
            useParalll
    use streamW = new StreamWriter(fileP)
    sorterEvls |> Array.map(fun sev -> sev |> SorterEvalDto.toJson |> streamW.WriteLine)
    

let getSorterSetEvals
    (fileN:string)
    =
    let fileP = Path.Combine(projectFolder, fileN)
    use streamW = new StreamReader(fileP)
    seq {
        let mutable keepG = true
        while keepG do 
            yield (streamW.ReadLine() |> SorterEvalDto.fromJson)
            keepG <- not streamW.EndOfStream
    } |> Seq.toList |> Result.sequence |> Result.ExtractOrThrow


let writeSorterEvalsSkinnyTable
        (sevs:sorterEval seq) 
        (fileN:string) 
        =
   let fileP = Path.Combine(projectFolder, fileN)
   use streamW = new StreamWriter(fileP)
   streamW.WriteLine "sorterId\tsorterPhenotypeId\tsortableSetID\tstages\tswitches"
   sevs |> Seq.iter(fun sev -> 
        let fl = sprintf "%s\t%s\t%s\t%d\t%d" 
                    (sev |> SorterEval.getSorterId |> SorterId.value |> string)
                    (sev |> SorterEval.getSortrPhenotypeId |> Option.map(SorterPhenotypeId.value) |> string)
                    (sev |> SorterEval.getSortableSetId |> SortableSetId.value |> string)
                    (sev |> SorterEval.getSorterSpeed |> SorterSpeed.getStageCount0)
                    (sev |> SorterEval.getSorterSpeed |> SorterSpeed.getSwitchCount0)
        streamW.WriteLine(fl))


///////////////////   Main ////////////////////////////

//makeRandomSorterSet()
//let sorterSet = getRandomSorterSet()

//let fullSortableSt = makeFullSortableSet fullBin16SortableSetId
//makeSorterSetEvals sorterSet fullSortableSt randomSorterSetFullEvalsFile |> ignore

//makeRandomSorterSet()
let sorterSet = getRandomSorterSet()

//makeSorterSetEvals sorterSet permSortableSetB randomSorterSetEvalsFileA |> ignore

//let fullSortableSt = makeFullSortableSet fullBin16SortableSetId
//makeSorterSetEvals sorterSet fullSortableSt randomSorterSetFullEvalsFile |> ignore
let evals = getSorterSetEvals(randomSorterSetFullEvalsFile)
writeSorterEvalsSkinnyTable evals "stF.txt"

printfn "Hello from F#"

