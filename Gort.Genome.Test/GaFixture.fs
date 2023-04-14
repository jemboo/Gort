namespace Gort.Genome.Test

open System
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member this.Counter () =

        let ctr = Counter.create 0
        let maxCounterValue = 3
        let updateBatchLength = 5
        let mutable errorRept = ""

        let gaCounter = 
                Ga.create
                    (fun c -> c |> Counter.getId)
                    ctr
                    (Counter.update maxCounterValue)
                    (Counter.terminate updateBatchLength)
                    (Counter.archive)


        let res = Ga.update gaCounter (fun m -> errorRept <- m)

        Assert.IsTrue(true);
