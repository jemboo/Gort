namespace global

open System

type Energy = private Energy of float
module Energy =
    let value (Energy v) = v
    let create v = Energy v
    let repStr v = match v with
                   |Some r -> sprintf "%.4f" (value r)
                   |None -> ""
    //let fromKey (m:Map<'a, obj>) (key:'a) =
    //    result {
    //        let! gv = ResultMap.read key m
    //        return! create "" (gv:?>float)
    //    }

    let failure = Double.MaxValue |> create

    let betterEnergy (a:Energy option) 
                     (b:Energy option) = 
        match a, b  with
        | Some av, Some bv -> if (value av) < (value bv) 
                                then a else b
        | None, Some e -> Some e
        | Some e, None -> Some e
        | _, None -> failwith "energy missing"

    let worst = 
        create Double.MaxValue

    let delta (oldE:Energy option) 
              (newE:Energy option) = 
        match oldE, newE  with
        | Some (Energy ov), Some (Energy nv) -> Some (create (nv - ov))
        | None, Some (Energy nv) ->
                        Some (create (nv - (value worst)))
        | _, _ -> None
    

    let isBetterThan (lhs:Energy) 
                     (rhs:Energy) = 
        (value lhs) < (value rhs)


type Temp = private Temp of float
module Temp =
    let value (Temp v) = v
    let create v = Temp v
    //let fromKey (m:Map<'a, obj>) (key:'a) =
    //    result {
    //        let! gv = ResultMap.read key m
    //        return! create "" (gv:?>float)
    //    }

type StepNumber = private StepNumber of int
module StepNumber =
    let value (StepNumber v) = v
    let create v = StepNumber v
    let increment gen = create ((value gen) + 1)
    //let logReporting (StepNumber totSteps) (ticsPerLog) =
    //    IntSeries.logTics ticsPerLog (totSteps * 2)
    //                |> Seq.map(fromInt)
    //                |> Seq.toArray
    //let fromKey (m:Map<'a, obj>) (key:'a) =
    //    result {
    //        let! gv = ResultMap.read key m
    //        return! create "" (gv:?>int)
    //    }


type RevNumber = private RevNumber of int
module RevNumber =
    let value (RevNumber v) = v
    let create fieldName v = RevNumber v
    let increment gen = create ((value gen) + 1)
    //let fromKey (m:Map<'a, obj>) (key:'a) =
    //    result {
    //        let! gv = ResultMap.read key m
    //        return! create "" (gv:?>int)
    //    }



type annealerSpec = 
    | Constant of Temp
    | Exp of Temp * float

module AnnealerSpec =
    let report (a:annealerSpec) =
        match a with
        | Constant t -> sprintf "c_%0.5f" (t |> Temp.value)
        | Exp (t,d) -> sprintf "e_%0.5f_%0.0f" (t |> Temp.value) d


module Annealer =

    let makeConst (temp:Temp) =
        fun curEnergy newEnergy (caster:unit->float) _ ->
            let cfv = curEnergy |> Energy.value
            let nfv = newEnergy |> Energy.value
            if (cfv >= nfv) then
                true
            else
                let tv = Temp.value temp
                let curTry = caster()
                let curThresh = Math.Exp(-(nfv - cfv) / tv) / 2.0
                curTry < curThresh

        
    let makeExp (temp:Temp) (decay:float) =
        fun curEnergy newEnergy (caster:unit->float) step ->
            let cfv = curEnergy |> Energy.value
            let nfv = newEnergy |> Energy.value
            if (cfv >= nfv) then
                true
            else
                let tv = Temp.value temp
                let stepFlt = StepNumber.value step |> float
                let curtemp = tv * Math.Exp (- stepFlt / decay)
                let curTry = caster()
                let curThresh = Math.Exp(-(nfv - cfv) / curtemp) / 2.0
                curTry < curThresh


    let make (annealerSpec:annealerSpec) =
        match annealerSpec with
        | Constant t -> makeConst t
        | Exp (t,hl) -> makeExp t hl
