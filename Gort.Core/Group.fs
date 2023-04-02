namespace global

open SysExt
open Microsoft.FSharp.Core




type bitPair = private BitPair of int
module BitPair =
    let value (BitPair v) = v
    let create (value: int) = value |> BitPair


type z4 = private Z4 of int
module Z4 =
    let value (Z4 v) = v
    let create (v: int) = (v % 4) |> Z4
    let add (addn:int) (v:z4)  =
        (((v |> value ) + addn) % 4 ) |> create

type cubePos = private {config:int; dim:int}
module CubePos =

    let create (theBits:int) (dim:int) =
        {config = theBits; dim = dim}

    let getConfig (cPos:cubePos) = cPos.config

    let getDim (cPos:cubePos) = cPos.dim

    let getZ4 (offset:int) (cPos:cubePos) =
        let theBits = cPos |> getConfig
        (theBits.isset (offset + 1) |> ByteUtils.toInt) * 2 
            + (theBits.isset (offset) |> ByteUtils.toInt)
        |> Z4.create

    let setZ4 (cPos:cubePos) (offset:int) (tZ4:z4) =
        let theBits = cPos |> getConfig
        let z4v = tZ4 |> Z4.value
        let retVal =  theBits 
                        |> ByteUtils.setBit (offset + 1) (z4v.isset 1)
                        |> ByteUtils.setBit (offset) (z4v.isset 0)
        create retVal cPos.dim

    //rotates the cubePos at the offset by rot number of quarter turns
    let rotateFace (cPos:cubePos) (offset:int) (rot:int) : cubePos =
        let newZ4 = cPos |> getZ4 offset |> Z4.add rot
        setZ4 cPos offset newZ4


module Group =

    // gets the two bits at offset and (offset + 1), returned as an int
    let getQuad (theBits:int) (offset:int) : int =
            (theBits.isset (offset + 1) |> ByteUtils.toInt) * 2 
            + (theBits.isset (offset) |> ByteUtils.toInt)

    // sets the bits at offset and (offset + 1), using the lowest two bits 
    // from quadVal.
    let setQuad (targetInt:int) (offset:int) (quadVal:int) : int =
        let bitsCopy = targetInt
        let retVal =  bitsCopy |> ByteUtils.setBit (offset + 1) (quadVal.isset 1)
                               |> ByteUtils.setBit (offset) (quadVal.isset 0)
        retVal


    let changeQuad (targetInt:int) (offset:int) (delta:int) =
         let q = getQuad targetInt offset
         let newQ = (q + delta) % 4
         setQuad targetInt offset newQ


    let getQuadsFromGuide (guideInt:int) (maxBit:int) =
        let lstRet = [getQuad guideInt 0]
        let sffx = [for bp = 2 to (maxBit - 1) do
                     (guideInt.isset bp |> ByteUtils.toInt) * 2  ]
        sffx |> List.append lstRet


    //let getQuadsFromGuide (rotationGuide:int) (maxBit:int) =
    //    let lstRet = [getQuad rotationGuide 0]
    //    let sffx = [for bp = 2 to (maxBit - 1) do
    //                 (rotationGuide.isset bp |> toInt) * 2  ]
    //    sffx |> List.append lstRet


