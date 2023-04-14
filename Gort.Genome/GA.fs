namespace global
open System


type gaId = private GaId of Guid

module GaId =
    let value (GaId v) = v
    let create dex = GaId dex

type reporter<'R> = 'R -> unit

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

    let update (maxV:int) (ctr:counter) =
        if ctr.value < maxV then
            {ctr with value = ctr.value + 1} |> Ok
        else
            "too big" |> Error

    let terminate (limit:int) (ctr:counter) =
        ctr.value >= limit |> Ok

    let archive (lastId:gaId) (ctr:counter) =
        if (ctr.value % 2 = 0) then
            ctr.id |> Ok
        else
            lastId |> Ok


module GaTest = 
    let u = None