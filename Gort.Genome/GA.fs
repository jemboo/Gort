namespace global
open System

type energy = private Energy of float
module Energy =
    let value (Energy v) = v
    let create v = Energy v
    let repStr v = 
        match v with
        |Some r -> sprintf "%.4f" (value r)
        |None -> ""

    let failure = Double.MaxValue |> create

    let betterEnergy (a:energy option) 
                     (b:energy option) = 
        match a, b  with
        | Some av, Some bv -> if (value av) < (value bv) 
                                then a else b
        | None, Some e -> Some e
        | Some e, None -> Some e
        | _, None -> failwith "energy missing"

    let worst = 
        create Double.MaxValue

    // a negative delta is an improvement
    let delta (oldE:energy option) 
              (newE:energy option) = 
        match oldE, newE  with
        | Some (Energy ov), Some (Energy nv) -> 
            Some (create (nv - ov))
        | None, Some (Energy nv) ->
            Some (create Double.MinValue)
        | _, _ -> None
    
    // lower energies are better
    let isBetterThan (lhs:energy) 
                     (rhs:energy) = 
        (value lhs) < (value rhs)



type gaId = private GaId of Guid

module GaId =
    let value (GaId v) = v
    let create dex = GaId dex


type generation = private Generation of int

module Generation =
    let value (Generation v) = v
    let create dex = Generation dex
    let next (Generation v) = Generation (v + 1)


//type reporter<'R> = 'R -> unit

type ga<'T> = 
    private
        {
            lastSavedAncestorId:gaId
            current: 'T;
            updater: 'T -> Result<'T, string>
            terminator: 'T -> Result<bool, string>
            archiver: gaId -> 'T  -> Result<gaId, string>
        }


module Ga =
    let create<'T>
            (ider:'T->gaId)
            (current: 'T)
            (updater:'T -> Result<'T, string>)
            (terminator: 'T -> Result<bool, string>)
            (archiver: gaId -> 'T -> Result<gaId, string>)
            =
        {
            lastSavedAncestorId = ider current;
            current = current;
            updater = updater
            terminator = terminator
            archiver = archiver
        }

    let getLastSavedAncestorId (ga:ga<'T> ) =
        ga.lastSavedAncestorId

    let getCurrent (ga:ga<'T> ) =
        ga.current

    let getUpdater (ga:ga<'T> ) =
        ga.updater

    let getTerminator (ga:ga<'T> ) =
        ga.terminator

    let update
        (ga:ga<'T> )
        (errorRep: string -> unit)
        =
        let mutable keepGoing = true
        let mutable gaCur = ga
        while keepGoing do
            let gaNext =
                result {
                    let! z = gaCur.updater gaCur.current
                    let! lastSavedAncestorId = 
                        gaCur.archiver gaCur.lastSavedAncestorId z
                    let! kg = gaCur.terminator z
                    keepGoing <- not kg
                    return 
                        {
                            lastSavedAncestorId = lastSavedAncestorId;
                            current = z;
                            updater = gaCur.updater
                            archiver = gaCur.archiver
                            terminator = gaCur.terminator
                        }
                }
            gaCur <-
                match gaNext with
                | Ok v -> v
                | Error msg -> 
                    keepGoing <- false
                    errorRep msg
                    gaCur
        gaCur


type counter = private {id:gaId; value:int}

module Counter =

    let create (value:int) =
        {
          id = Guid.NewGuid() |> GaId.create;  
          counter.value = value
        }

    let getValue (ctr:counter) =
        ctr.value

    let getId (ctr:counter) =
        ctr.id

    let update 
            (maxV:int) 
            (ctr:counter) 
         =
        if ctr.value < maxV then
            {ctr with value = ctr.value + 1} |> Ok
        else
            "too big" |> Error

    let terminate 
            (limit:int)
            (ctr:counter)
        =
        ctr.value >= limit |> Ok


    let archive 
            (lastId:gaId) 
            (ctr:counter) 
        =
        if (ctr.value % 2 = 0) then
            ctr.id |> Ok
        else
            lastId |> Ok

