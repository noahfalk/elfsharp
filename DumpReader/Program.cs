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
                    NoteList notes = s.GetParsedContents<NoteList>();
                    foreach(NoteSegment note in notes.Notes)
                    {
                        Console.WriteLine("    " + note.Name + " : 0x" + note.Type.ToString("x"));
                        Console.WriteLine("         byte[0x" + note.GetContents().Length.ToString("x") + "]");
                        if(note.Type == 0x46494c45)
                        {
                            FileTable fileTable = note.GetParsedContents<FileTable>();
                            foreach(FileEntry entry in fileTable.Entries)
                            {
                                
                                ulong fileAddress = 0;
                                elf.TryVMToFile(entry.vmStart, out fileAddress);
                                Console.WriteLine("        " + fileAddress + " : " + entry.FileName);
                            }
                        }
                    }
                    
                }
            }
        }
    }
}
