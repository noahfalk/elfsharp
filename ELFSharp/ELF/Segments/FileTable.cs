using MiscUtil.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ELFSharp.ELF.Segments
{
    public class FileEntry
    {
        public ulong vmStart;
        public ulong vmStop;
        public ulong pageOffset;
        public string FileName;
    }

    public class FileTable
    {
        long tableOffset;
        Func<EndianBinaryReader> readerSource;
        public List<FileEntry> Entries;

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
                Entries = new List<FileEntry>();

                ulong entryCount = reader.ReadUInt64();
                ulong pageSize = reader.ReadUInt64();

                //TODO: sanity check the entryCount
                for(uint i = 0; i < entryCount; i++)
                {
                    ulong vmStart = reader.ReadUInt64();
                    ulong vmStop = reader.ReadUInt64();
                    ulong pageOffset = reader.ReadUInt64();
                    Entries.Add(new FileEntry() { vmStart = vmStart, vmStop = vmStop, pageOffset = pageOffset });
                }
                for (int i = 0; i < (int)entryCount; i++)
                {
                    List<byte> nameBytes = new List<byte>();
                    byte b;
                    while(0 != (b = reader.ReadByte()))
                    {
                        nameBytes.Add(b);
                    }
                    string fileName = Encoding.UTF8.GetString(nameBytes.ToArray());
                    Entries[i].FileName = fileName;
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
