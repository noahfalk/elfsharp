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
        public long Size;
        Func<EndianBinaryReader> readerSource;
        uint n_namesz;
        uint n_descsz;
        public uint Type;
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
                Type = reader.ReadUInt32();
                //TODO: we should sanity check this size
                byte[] nameBytes = reader.ReadBytes(Align4((int)n_namesz));
                if(n_namesz != 0)
                {
                    Name = Encoding.UTF8.GetString(nameBytes, 0, (int)n_namesz - 1);
                }
                else
                {
                    Name = "";
                }
                contentsOffset = reader.BaseStream.Position;
                Size = contentsOffset + Align4((int)n_descsz) - headerOffset;
            }
        }

        public T GetParsedContents<T>() where T : class
        {
            if(typeof(T) == typeof(FileTable))
            {
                return new FileTable(contentsOffset, readerSource) as T;
            }
            return null;
             
        }

        public byte[] GetContents()
        {
            using (var reader = ObtainReader(contentsOffset))
            {
                return reader.ReadBytes(Align4((int)n_descsz));
            }
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
        NT_PRPSINFO = 3
    }
}
