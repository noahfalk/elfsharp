using MiscUtil.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELFSharp.ELF.Segments
{
    public class FileTable
    {
        long tableOffset;
        Func<EndianBinaryReader> readerSource;


        public FileTable(long tableOffset, Func<EndianBinaryReader> readerSource)
        {
            this.readerSource = readerSource;
            this.tableOffset = tableOffset;
            ReadTable();
        }

        void ReadTable()
        {
            using (var reader = ObtainReader(tableOffset))
            {
                ulong entryCount = reader.ReadUInt64();
                ulong pageSize = reader.ReadUInt64();
                //TODO: sanity check the entryCount
                for(uint i = 0; i < entryCount; i++)
                {
                    ulong vmStart = reader.ReadUInt64();
                    ulong vmStop = reader.ReadUInt64();
                    ulong pageOffset = reader.ReadUInt64();
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
