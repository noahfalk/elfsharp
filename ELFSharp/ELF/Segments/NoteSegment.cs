using MiscUtil.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELFSharp.ELF.Segments
{
    public class NoteSegment
    {
        long headerOffset;
        long contentsOffset;
        Func<EndianBinaryReader> readerSource;
        uint n_namesz;
        uint n_descsz;
        public ELFNoteType Type;
        public string Name;
        

        public NoteSegment(long headerOffset, Func<EndianBinaryReader> readerSource)
        {
            this.readerSource = readerSource;
            this.headerOffset = headerOffset;
            ReadHeader();
        }

        void ReadHeader()
        {
            using (var reader = ObtainReader(headerOffset))
            {
                n_namesz = reader.ReadUInt32();
                n_descsz = reader.ReadUInt32();
                Type = (ELFNoteType) reader.ReadUInt32();
                //TODO: we should sanity check this size
                byte[] nameBytes = reader.ReadBytes(Align4((int)n_namesz));
                Name = Encoding.UTF8.GetString(nameBytes, 0, (int)n_namesz-1);
                contentsOffset = reader.BaseStream.Position;
            }
        }

        public T GetParsedContents<T>() where T : class
        {
            if(typeof(T) == typeof(FileTable) && 
               Type == ELFNoteType.NT_FILE)
            {
                return new FileTable(contentsOffset, readerSource) as T;
            }

            return null;
        }

        int Align4(int value)
        {
            return (value + 3) & ~3;
        }

        private EndianBinaryReader ObtainReader(long givenOffset)
        {
            var reader = readerSource();
            reader.BaseStream.Seek(givenOffset, SeekOrigin.Begin);
            return reader;
        }
    }

    public enum ELFNoteType : uint
    {
        //TODO: what other types are possible?
        NT_FILE = 1,
        NT_PRPSINFO = 3
    }
}
