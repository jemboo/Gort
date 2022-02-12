namespace global
open Microsoft.FSharp.Core
open System
open System.Security.Cryptography

module GuidUtils = 

    let makeGuid (g1:uint64) (g2:uint64) (g3:uint64) (g4:uint64) =
        let pc0 = System.BitConverter.GetBytes(g1)
        let pc1 = System.BitConverter.GetBytes(g2)
        let pc2 = System.BitConverter.GetBytes(g3)
        let pc3 = System.BitConverter.GetBytes(g4)

        let woof = seq {pc0.[0]; pc0.[1]; pc0.[2]; pc0.[3]; 
                        pc1.[0]; pc1.[1]; pc1.[2]; pc1.[3];
                        pc2.[0]; pc2.[1]; pc2.[2]; pc2.[3];
                        pc3.[0]; pc3.[1]; pc3.[2]; pc3.[3]; } |> Seq.toArray
        new System.Guid(woof)


    let addGuids (g1:Guid) (g2:Guid) =
        let pcs1 = g1.ToByteArray()
        let pcs2 = g2.ToByteArray()
        let pcsS = Array.init 16 (fun i-> pcs1.[i] + pcs2.[i])
        new System.Guid(pcsS)


    let addGuidsO (g1:Guid option) (g2:Guid option) =
        match g1,g2 with
        | Some v1, Some v2 -> addGuids v1 v2
        | None, Some v2 -> v2
        | Some v1, None -> v1
        | None, None -> Guid.Empty


    let guidFromBytes (objs:seq<byte>) =
        let md5 = MD5.Create()
        System.Guid(md5.ComputeHash(objs |> Seq.toArray))

    let guidFromObjs (objs:seq<obj>) =
        System.Guid(ByteUtils.hashObjs objs)


    let guidFromStringR (gstr:string) =
        let mutable gv = Guid.NewGuid()
        match Guid.TryParse(gstr, &gv) with
        | true -> gv |> Ok
        | false -> "not a guid: " + gstr |> Result.Error
        

    let guidFromStringO (gstr:string) =
        let mutable gv = Guid.NewGuid()
        match Guid.TryParse(gstr, &gv) with
        | true -> gv |> Some
        | false -> None
