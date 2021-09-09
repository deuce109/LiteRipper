namespace LiteRipper.FileSystem

type IFileSystemObject =
    abstract FullPath : string
    abstract FileType : string
    abstract FileSize : int64
    abstract member Exists : unit -> bool
    abstract member Copy : string -> bool
    abstract member ToString : unit -> string
