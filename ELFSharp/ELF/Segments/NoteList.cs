using MiscUtil.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELFSharp.ELF.Segments
{
    public class NoteList
    {
        long startOffset;
        long size;
        Func<EndianBinaryReader> readerSource;
        public List<NoteSegment> Notes;
        public ELFNoteType Type;
        public string Name;


        public NoteList(long startOffset, long size, Func<EndianBinaryReader> readerSource)
        {
            this.readerSource = readerSource;
            this.startOffset = startOffset;
            this.size = size;
            ReadNotes();
        }

        void ReadNotes()
        {
            using (var reader = ObtainReader(startOffset))
            {
                Notes = new List<NoteSegment>();
                long currentOffset = startOffset;
                while(currentOffset <= startOffset + size)
                {
                    NoteSegment note = new NoteSegment(currentOffset, readerSource);
                    Notes.Add(note);
                    currentOffset += note.Size;
                }
            }
        }

        private EndianBinaryReader ObtainReader(long givenOffset)
        {
            var reader = readerSource();
            reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
            return reader;
        }
    }
}
