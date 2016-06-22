using ELFSharp.ELF;
using ELFSharp.ELF.Sections;
using ELFSharp.ELF.Segments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DumpReader
{
    class Program
    {
        static void Main(string[] args)
        {
            IELF elf = ELFReader.Load("D:\\playground\\core");
            foreach(ISegment s in elf.Segments)
            {
                Console.WriteLine(s.Type);
                if(s.Type == SegmentType.Note)
                {
                    NoteSegment note = s.GetParsedContents<NoteSegment>();
                    Console.WriteLine("    " + note.Name);
                    if(note.Type == ELFNoteType.NT_FILE)
                    {
                        FileTable fileTable = note.GetParsedContents<FileTable>();
                    }
                }
            }
        }
    }
}
