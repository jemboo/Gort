namespace Gort.DataConvert.Test

open Microsoft.VisualStudio.TestTools.UnitTesting
open DomainTables

[<TestClass>]
type rolloutDtoFixture() =

    [<TestMethod>]
    member this.rolloutDto() =
        let arOfIntAr =
            [| [| 1111; 11112; 11113; 5 |]
               [| 2211; 12; 13; 4 |]
               [| 2221; 22; 23; 3 |]
               [| 5555531; 32; 33; 2 |] |]


        let llForm = arOfIntAr
                        |> Array.map(Array.toList)
                        |> Array.toList

        let arrayLen = 4 |> ArrayLength.createNr
        let btsPerSymbol = 28 |> BitsPerSymbol.createNr
        let fmt = rolloutFormat.RfI32
        let rollout = Rollout.fromIntArrays fmt arrayLen btsPerSymbol arOfIntAr
                      |> Result.ExtractOrThrow

        let dto = rollout |> RolloutDto.toJson
        let rolloutBack = dto |> RolloutDto.fromJson
                          |> Result.ExtractOrThrow

        let rbData = rolloutBack 
                        |> Rollout.toIntArrays
                        |> Seq.map(Array.toList)
                        |> Seq.toList

        Assert.IsTrue ( CollectionProps.areEqual llForm rbData )



