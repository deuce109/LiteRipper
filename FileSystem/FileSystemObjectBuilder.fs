namespace LiteRipper.FileSystem

open System

module FileSystemObject =
    let getFileObjectType (filePath: string) =
        if not <| IO.Path.IsPathFullyQualified(filePath) then
            let message = sprintf "${(nameof filePath)}"
            let argExcept = ArgumentException message
            raise argExcept

        let attr = IO.File.GetAttributes(filePath)

        if attr.HasFlag(IO.FileAttributes.Directory) then
            typedefof<Folder>
        else
            typedefof<File>


    let Build (filePath: string) : IFileSystemObject =
        let fullPath = IO.Path.GetFullPath(filePath)
        let fileType: Type = getFileObjectType (fullPath)
        Activator.CreateInstance(fileType, fullPath) :?> IFileSystemObject
