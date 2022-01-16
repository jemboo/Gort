namespace global
open System


    module BatchRuns =
    
        let textExt = FileExt.create ".txt"
    

        let Proc (yab: unit->unit) = 

            Console.WriteLine(sprintf "Started: %s "
                                    (System.DateTime.Now.ToLongTimeString()))
            yab() |> ignore
    
            Console.WriteLine(sprintf "Finished: %s "
                                    (System.DateTime.Now.ToLongTimeString()))
 
            Console.Read() |> ignore


        let SayHi () =
            Console.WriteLine("Hi")