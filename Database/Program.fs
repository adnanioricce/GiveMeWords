open StringDB
open StringDB.Databases
open StringDB.Fluency
open StringDB.IO
open StringDB.Transformers
open StringDB.LazyLoaders
open System.Collections.Generic
let selectMany (ab:'a -> 'b seq) (abc:'a -> 'b -> 'c) input =
    input |> Seq.collect (fun a -> ab a |> Seq.map (fun b -> abc a b))
let createDb () =
  DatabaseBuilder()
  |> (fun builder -> builder.UseIODatabase(StringDBVersion.Latest, "database.db"))
  |> (fun builder -> builder.WithBuffer(1000))
  |> (fun builder -> builder.WithTransform(StringTransformer.Default, StringTransformer.Default))
let db = createDb ()
[0..1000]
|> Seq.iter (fun i -> [0..1000] |> Seq.iter (fun j -> db.Insert(System.Guid.NewGuid().ToString(),(i + j).ToString())))

for k in db do
  printfn "Key=%s -> %s" k.Key (k.Value.Load())