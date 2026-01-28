using GRC.BuildingBlocks.Domain.SeedWork;
public class PolicyDocument : Entity
{
    public string FileName { get; private set; }
    public string FileUrl { get; private set; }
    public string DocumentType { get; private set; }
    public long FileSize { get; private set; }
    public Guid PolicyId { get; private set; }
    public DateTime CreatedAt { get; set; }

    private PolicyDocument() { }

    public PolicyDocument(string fileName, string fileUrl, string documentType, long fileSize = 0)
    {
        Id = Guid.NewGuid();
        FileName = fileName;
        FileUrl = fileUrl;
        DocumentType = documentType;
        FileSize = fileSize;
        CreatedAt = DateTime.UtcNow;
    }
}