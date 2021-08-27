// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
namespace LiteRipper

open System.IO

// Define a function to construct a message to print


module Main =
    [<EntryPoint>]
    let main argv =
        let drives = Utils.getDrivesByType "fixed"

        Utils.printDrives drives

        use write =
            new FileStream("./test.iso", System.IO.FileMode.Create)

        let rec fileLoop (files: string list) : unit =
            match files with
            | head :: tail when Directory.Exists(head) ->
                fileLoop (
                    List.ofSeq
                    <| Directory.EnumerateFileSystemEntries(head)
                )

                fileLoop (tail)
            | head :: tail when File.Exists(head) ->
                use read =
                    new FileStream(head, FileMode.Open, FileAccess.Read)

                read.CopyTo(write)
                read.Flush()
                fileLoop (tail)
            | _ :: _ -> ()
            | [] -> ()

        fileLoop (
            List.ofSeq
            <| Directory.EnumerateFileSystemEntries("/boot/efi")
        )

        write.Flush()

        printfn "%s"
        <| Utils.generateFileSize (
            float
            <| FileSystem
                .FileSystemObject
                .Build(
                    "/boot/efi"
                )
                .FileSize
        )

        printfn "%s"
        <| Utils.generateFileSize (
            float
            <| FileSystem
                .FileSystemObject
                .Build(
                    "test.iso"
                )
                .FileSize
        )

        // let file =
        //     FileSystem.FileSystemObject.Build argv.[0]


        // printfn "%b" <| file.Copy("copy.txt")


        0 // return an integer exit code
