namespace global

open Gort.Data.DataModel

module Run =

    let RunCause (cz: Cause) (pth: string list) (ctxt: Gort.Data.DataModel.IGortContext) =
        if cz.CauseStatus = CauseStatus.Pending then
            result {
                let! res =
                    match pth with
                    | [] -> "No path in RunCause" |> Error
                    | x :: [] ->
                        match x with
                        | "Root" ->
                            (sprintf "this child cause: %s of Root is not handled" cz.CauseType.Name)
                            |> Error
                        | _ -> "Root should be the first CauseTypeGroup" |> Error
                    | x :: xs ->
                        match x with
                        | "Root" -> ctrRoot.RunRootChildren cz xs ctxt
                        | _ -> "Bad path in RunCause" |> Error

                if (res > 0) then
                    cz.CauseStatus <- CauseStatus.Complete
                    ctxt.SaveChanges() |> ignore

                return res
            }
        else
            sprintf "Cause status was %s" (cz.CauseStatus.ToString()) |> Error
