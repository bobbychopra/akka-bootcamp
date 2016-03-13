namespace GithubActors

open System
open System.Windows.Forms
open Akka
open Akka.Actor
open Akka.FSharp

module Program =
    [<STAThread>]
    [<EntryPoint>]
    let main argv = 
        let system = System.create "GithubActors" <| Configuration.load ()

        Application.EnableVisualStyles ()
        Application.SetCompatibleTextRenderingDefault false
        Application.Run (GithubAuthForm.load system)
        0 // return an integer exit code
