namespace global
open Gort.DataStore.DataModel

module Run =

    let RunCause (cz:CauseR) 
                 (pth:string list) 
                 (ctxt:IGortContext2) =
        if cz.CauseStatus = CauseStatus.Pending then
            result {
                let! res = 
                    match cz.Genus with
                    | _ -> "No path in RunCause" |> Error

                if (res > 0) then
                    cz.CauseStatus <- CauseStatus.Complete
                    ctxt.SaveChanges() |> ignore
                return 1
            }
        else sprintf "Cause status was %s" (cz.CauseStatus.ToString()) |> Error