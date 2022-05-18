open System
open System.Threading
open System.IO
open Suave
open Suave.Json
open Suave.Filters
open Suave.Operators
open System.Runtime.Serialization
open FSharp.Json
open YoLo
module RestUtils = 
    [<DataContract>]
    type Response = {
        [<field: DataMember(Name="word")>]
        word:string
    }
    let JSON v =
        (Json.serialize v)
        |> Successful.OK
        >=> Writers.setMimeType "application/json; charset=utf-8"
module Db = 
    let private loadFile filePath =
        File.ReadAllLines filePath    
    let getWords =
        loadFile "words.txt"    
module Application = 
    open RestUtils
    let getRandomWords getRandomInt =
        Db.getWords
        |> Seq.item (getRandomInt ())
    let private rng = Random(67898421) 
    let getRandomInt start limit =
        let rng = Random()
        fun _ -> rng.Next(start,limit)
    let randomWord () =        
        let wordPartWith () = 
            let randomWord  = getRandomWords (getRandomInt 0 Db.getWords.Length)        
            Json.serialize ({ word = randomWord})
        let wordPart () = wordPartWith ()
        wordPart () 
module WordApi =
    let getWord () =     
        warbler( fun ctx -> 
            // GET >=> path "/"
            Successful.OK (Application.randomWord())
            >=> Writers.setMimeType "Application/json; charset=utf-8")
            
let app =    
    choose
        [
            path "/app.js" >=> choose [
                GET >=> Files.file "app.js"
                GET >=> Files.browseFileHome "app.js"
                GET >=> Files.browseHome
                RequestErrors.NOT_FOUND "Found no handlers"
            ]
            path "/" >=> choose [
                GET >=> Files.browseFileHome "index.html"
                RequestErrors.NOT_FOUND "Found no handlers"
            ]
            // GET >=> Files.browseHome
            path "/getWord" >=> choose [                               
                GET >=> WordApi.getWord ()
            ]            
        ]
[<EntryPoint>]
let main argv =
    let portByEnv =
        match Env.var ("PORT") with
        | Some(x) -> x
        | None -> "8080"

    let ip : Net.IPAddress = Net.IPAddress.Parse("0.0.0.0")
    let port = uint16 portByEnv

    let socketBinding : Sockets.SocketBinding =
        { ip = ip
          port = port }

    let httpBinding : Http.HttpBinding =
        { scheme = Protocol.HTTP
          socketBinding = socketBinding }
    let cts = new CancellationTokenSource()
    let conf = { 
        defaultConfig with 
            cancellationToken = cts.Token
            homeFolder = Some (Path.GetFullPath "./public")
            bindings = [ httpBinding ]

    }    
    startWebServer conf app
    // let listening,server = startWebServerAsync conf (app ())
    // Async.Start(server, cts.Token)
    // printfn "Make requests now"    
    // Console.ReadLine() |> ignore
    // cts.Cancel()
    0
