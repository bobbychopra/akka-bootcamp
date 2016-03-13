namespace GithubActors

open Akka
open Akka.FSharp
open Akka.Actor
open System.Drawing
open System.Windows.Forms
open GitHub

module GithubAuthForm =

    let form = new Form(Name = "GithubAuth", Visible = true, Text = "Sign in to GitHub", AutoScaleMode = AutoScaleMode.Font, ClientSize = Size(598, 194))
    let label1 = new Label(Name = "label1", Text = "GitHub Access Token", Location = Point(12, 9), Size = Size(172, 18), Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))))
    let tbOAuth = new TextBox(Name = "tbOAuth", Location = Point(190, 6), Size = Size(379, 24), Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))))
    let lblAuthStatus = new Label(Name = "lblAuthStatus", AutoSize = true, Location = Point(187, 33), Size = Size(87, 18), Text = "lblGHStatus", Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))))
    let linkGhLabel = new LinkLabel(Name = "linkGhLabel", AutoSize = true, Location = Point(148, 128), Size = Size(273, 18), Text = "How to get a GitHub Access Token", Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))))
    let btnAuthenticate = new Button(Name = "btnAuthenticate", Location = Point(214, 81), Size = Size(136,32), Text = "Authenticate", Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))))
    
    linkGhLabel.LinkClicked.Add (fun _ -> System.Diagnostics.Process.Start "https://help.github.com/articles/creating-an-access-token-for-command-line-use/" |> ignore )
    

    form.SuspendLayout ()
    form.Controls.Add label1
    form.Controls.Add tbOAuth
    form.Controls.Add lblAuthStatus
    form.Controls.Add linkGhLabel
    form.Controls.Add btnAuthenticate
    form.ResumeLayout false

    let load (myActorSystem:ActorSystem) =
        let _authActor = spawn myActorSystem "authenticator" (authenticationActor lblAuthStatus form)
        btnAuthenticate.Click.Add (fun _ -> _authActor <! OAuthToken(tbOAuth.Text))

        form

module LauncherForm =
    let form = new Form(Name = "LauncherForm", Visible = true, Text = "Who Starred This Repo?", AutoScaleMode = AutoScaleMode.Font, ClientSize = Size(562, 151), AutoScaleDimensions = SizeF(6.0f, 13.0f))
    let tbRepoUrl = new TextBox(Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))), Location = Point(95, 13), Name = "tbRepoUrl", Size = Size(455, 24))
    let lblRepo = new Label(AutoSize = true, Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))), Location = Point(3, 16), Name = "lblRepo", Size = Size(86, 18), Text = "Repo URL")
    let lblIsValid = new Label(AutoSize = true, Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0))), Location = Point(95, 44), Name = "lblIsValid", Size = Size(46, 18), Text = "label1", Visible = false)
    let btnLaunch = new Button(Location = Point(218, 90), Name = "btnLaunch", Size = Size(142, 37), Text = "GO", UseVisualStyleBackColor = true, Font = new Font("Microsoft Sans Serif", 12.0F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0))))

    btnLaunch.Click.Add (fun _ -> ())

    form.SuspendLayout ()
    form.Controls.Add tbRepoUrl
    form.Controls.Add lblRepo
    form.Controls.Add lblIsValid
    form.Controls.Add btnLaunch
    form.ResumeLayout false
    
    let load () =
        form

module RepoResultsForm =
    let form = new Form(Name = "LauncherForm", Text = "Who Starred This Repo?", AutoScaleDimensions = SizeF(6.0F, 13.0F), AutoScaleMode = AutoScaleMode.Font, ClientSize = Size(739, 322))
    let dgUsers = new DataGridView(AllowUserToOrderColumns = true, ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize, Dock = DockStyle.Fill, Location = Point(0, 0), Name = "dgUsers", Size = Size(739, 322))
    let statusStrip1 = new StatusStrip(Location = Point(0, 300), Name = "statusStrip1", Size = Size(739, 22), Text = "statusStrip1")
    let tsProgress = new ToolStripProgressBar(Name = "tsProgress", Size = Size(100, 16), Visible = false)
    let tsStatus = new ToolStripStatusLabel(Name = "tsStatus", Size = Size(73, 17), Text = "Processing...", Visible = false)
    let Owner = new DataGridViewTextBoxColumn(HeaderText = "Owner", Name = "Owner")
    let RepoName = new DataGridViewTextBoxColumn(HeaderText = "Name", Name = "RepoName")
    let URL = new DataGridViewLinkColumn(AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, HeaderText = "URL", Name = "URL")
    let Shared = new DataGridViewTextBoxColumn(HeaderText = "SharedStars", Name = "Shared")
    let Watchers = new DataGridViewTextBoxColumn(HeaderText = "Watchers", Name = "Watchers")
    let Stars = new DataGridViewTextBoxColumn(HeaderText = "Stars", Name = "Stars")
    let Forks = new DataGridViewTextBoxColumn(HeaderText = "Forks", Name = "Forks")

    form.SuspendLayout ()
    statusStrip1.SuspendLayout ()
    (dgUsers :> System.ComponentModel.ISupportInitialize).BeginInit ()

    let columns:DataGridViewColumn[] = [| Owner; RepoName; URL; Shared; Watchers; Stars; Forks |]
    dgUsers.Columns.AddRange columns
    let stripItems:ToolStripItem[] = [| tsProgress; tsStatus |]
    statusStrip1.Items.AddRange stripItems

    form.Controls.Add statusStrip1
    form.Controls.Add dgUsers

    (dgUsers :> System.ComponentModel.ISupportInitialize).EndInit ()
    statusStrip1.ResumeLayout false
    form.ResumeLayout false
