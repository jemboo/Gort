namespace global

open Microsoft.FSharp.Core
open System
open System.Security.Cryptography

module StringUtil =

    let nullOption (sV:string option) =
        match sV with
        |Some sv -> sv
        |None -> null


    let stringOption 
            (emptyVal:string) 
            (strVal: string option) 
        =
        match strVal with
        | Some sv -> sv
        | None -> emptyVal