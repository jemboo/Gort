namespace global

type activated<'y,'r> = private {payload:'y; activationParam:'r}

module Activated =

    let make y r =  { payload = y; activationParam = r }

    let getParam av = av.activationParam

    let filter (avs : activated<'y,'r> seq) actFun =
        avs |> Seq.filter(fun av -> actFun (av |> getParam))

    let mutatePayload av mutator rate (randy:IRando) =
        if ( randy.NextFloat < (rate |> MutationRate.value)) then
           { payload = mutator av.payload; activationParam = av.activationParam }
           else
           { payload = av.payload; activationParam = av.activationParam }


    let mutateParameter av mutator rate (randy:IRando) =
        if ( randy.NextFloat < (rate |> MutationRate.value)) then
           { payload = av.payload; activationParam = mutator av.activationParam }
           else
           { payload = av.payload; activationParam = av.activationParam }

    let flipo v =
        not v