namespace LMS_Elibrary.Data
{
    public class PrivateFileDTO
    {
        //public PrivateFile privateFile { get; set; }
        public int PrivateFileId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Updator { get; set; }
        public DateTime Date { get; set; }
        public int Size { get; set; }

    }
}
