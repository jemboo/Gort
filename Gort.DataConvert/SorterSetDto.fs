namespace global
open System
open Microsoft.FSharp.Core

type sorterSetDto = { 
        id: Guid; order:int; sorterIds: Guid[]; 
        offsets: int[]; byteLens:int[] }
