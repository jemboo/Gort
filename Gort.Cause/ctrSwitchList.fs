namespace global
open System
open Gort.Data.DataModel

module ctrSwitchList =


    let RunSwitchList (cz:Cause) (ctxt:Gort.Data.DataModel.IGortContext) =
        match cz.CauseType.Name with
        | "SwitchListImport" -> 5|>Ok
        | n -> (sprintf "%s not handled in RunSwitchList" n) |> Error


    let RunSwitchListChildren (cz:Cause) (pth:string list) (ctxt:Gort.Data.DataModel.IGortContext) =
        let g::gs = pth
        match g with
        | n -> (sprintf "%s not handled in RunSwitchListChildren" n) |> Error
