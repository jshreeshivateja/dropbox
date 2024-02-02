namespace Dropbox
{
    public class DropboxFile
    {
        public string FileName { get; set; }

        public byte[] Data { get; set; }

        public DropboxFileMetadata Metadata { get; set; }
    }

    public class DropboxFileMetadata
    {
        public string FileName { get; set; }

        public DateTime CreatedAt { get; set; }

        public long SizeInBytes { get; set; }

        public string FileType { get; set; }
    }
}
