// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
namespace LiteRipper



// Define a function to construct a message to print


module Main =
    [<EntryPoint>]
    let main argv =
        let drives = Utils.getDrivesByType "ram"
        Utils.printDrives drives
        0 // return an integer exit code
