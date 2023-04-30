namespace global

open System
open Microsoft.FSharp.Core

type sparseArray<'a> =
    private
        {
            length:int
            indexes:int[]
            values:'a[]
            emptyVal:'a
        }


module SparseArray =
    let create<'a>
            (length:int)
            (indexes:int[])
            (values:'a[])
            (emptyVal:'a)
        =
        {
            length=length
            indexes=indexes
            values=values
            emptyVal=emptyVal
        }

    let getLength sa = sa.length
    let getIndexes sa  = sa.indexes
    let getValues sa  = sa.values
    let getEmptyVal sa  = sa.emptyVal
    
    let fromArray 
            (emptyVal:'a)
            (src:'a[])
        =
        let indexes = new ResizeArray<int>()
        let values = new ResizeArray<'a>()
        let mutable dex = 0
        while dex < src.Length do
            if (src.[dex] <> emptyVal) then
                indexes.Add dex
                values.Add src.[dex]
            dex <- dex + 1
        {
            length = src.Length
            indexes = indexes.ToArray()
            values = values.ToArray()
            emptyVal = emptyVal
        }


    let toArray 
            (sa:sparseArray<'a>)
        =
        let arRet = Array.create<'a> sa.length sa.emptyVal
        let mutable dex = 0
        while dex < sa.indexes.Length do
            arRet.[sa.indexes.[dex]] <- sa.values.[dex]
            dex <- dex + 1
        arRet