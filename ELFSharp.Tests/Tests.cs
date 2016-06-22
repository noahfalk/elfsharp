using ELFSharp.ELF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ELFSharp.Tests
{
    public class Tests
    {

        [Fact]
        public void ParseDump()
        {
            IELF elf = ELFSharp.ELF.ELFReader.Load("D:\\playground\\core");
        }
    }
}
