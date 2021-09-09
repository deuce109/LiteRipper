// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
namespace LiteRipper

module Main =
    [<EntryPoint>]
    let main argv =

        let drives = Utils.getDrivesByType "cdrom"

        Utils.printDrives drives

        let drive =
            (FileSystem.Drive(drives.[2]) :> FileSystem.IFileSystemObject)

        printfn "%s" <| drive.ToString()

        (drive :?> FileSystem.Drive)
            .CreateIso("rhel8.iso", "")
        |> ignore

        0 // return an integer exit code
