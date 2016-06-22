namespace ELFSharp.ELF.Segments
{
    public interface ISegment
    {
        SegmentType Type { get; }
        SegmentFlags Flags { get; }
        byte[] GetContents();
        T GetParsedContents<T>() where T : class;
        byte[] GetRawHeader();
    }
}

