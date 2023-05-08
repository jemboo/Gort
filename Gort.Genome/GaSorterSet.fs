namespace global
open System



















          
type gaSorterSetOld = 
    private 
        { 
            id:gaId; 
            generation:generation; 
            value:sorterSetEval
        }


module GaSorterSetOld =

    let create 
            (value:sorterSetEval) 
        =
        {
          id = Guid.NewGuid() |> GaId.create;
          generation = 0 |> Generation.create
          value = value
        }

    let getValue 
            (gaSorterSet:gaSorterSetOld) 
        =
        gaSorterSet.value

    let getId 
            (gaSorterSet:gaSorterSetOld) 
        =
        gaSorterSet.id

    let update
            (gaSorterSet:gaSorterSetOld) 
         =
         try
            { gaSorterSet with
                generation 
                    = gaSorterSet.generation |> Generation.next


            } |> Ok

         with ex ->
            ("error in update: " + ex.Message) |> Result.Error

    let terminate 
            (maxGen:generation)
            (gaSorterSet:gaSorterSetOld) 
        =
        false


    let archive
            (fileAppender: string -> seq<string> -> Result<bool, string>)
            (fileName:string)
            (gaSorterSet:gaSorterSetOld)  
        =
        result {
            if System.IO.File.Exists fileName then
                fileAppender fileName (seq {"Hi"} ) |> ignore
            else
                fileAppender fileName (seq {"Hi"} ) |> ignore

            return gaSorterSet.id
        }