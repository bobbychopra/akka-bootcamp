namespace GithubActors

open Akka
open Akka.FSharp
open Akka.Actor
open Octokit
open System.Drawing
open System.Windows.Forms


type AuthenticateMessage =
| Authenticate of string
| AuthenticationFailed
| AuthenticationCancelled
| AuthenticationSuccess
    
module GitHub =
    let authenticationActor (statusLabel:Label) (form:Form) (getLauncherForm:unit->Form) (mailbox:Actor<_>) =
            
        let becomeAuthenticating () =
            statusLabel.Visible <- true
            statusLabel.ForeColor <- Color.Yellow
            statusLabel.Text <- "Authenticating..."

        let becomeUnauthenticated (reason:string) = 
            statusLabel.ForeColor <- Color.Red
            statusLabel.Text <- "Authentication Failed. Please try again."


        
        let rec unauthenticated () = 
            actor{
                let! msg = mailbox.Receive ()
                match msg with
                | Authenticate(token) -> 
                    let client = GitHubClient(ProductHeaderValue("AkkaBootcamp-Unit3"))
                    client.Credentials = Credentials(token) |> ignore
//                    client.User.Current().ContinueWith
                    return! becomeAuthenticating ()
                | _ -> ()
                return! unauthenticated ()
            }
        and authenticating () =
            actor{
                let! msg = mailbox.Receive ()
                match msg with
                | AuthenticationFailed -> becomeUnauthenticated "Authentication failed."
                | AuthenticationCancelled -> becomeUnauthenticated "Authentication timed out."
                | AuthenticationSuccess -> 
                    let launcher = getLauncherForm ()
                    launcher.Show ()
                    form.Hide ()
            }
        unauthenticated ()
