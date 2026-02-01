namespace WT.B2C.API.BuildingBlocks.Types;

public class FileOutputDto
{
    public byte[] FileBytes { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }
}