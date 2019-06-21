using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.IO
{
    public static class TextReaderExtensions
    {

        public static char[] ReadBuffer(this TextReader textReader)
        {
            List<char> returnArray = new List<char>();

            char[] buffer = new char[1024];
            var index = 0;
            int count = 0;
            do
            {
                count = textReader.ReadBlock(buffer, index, 1024);
                index += count;
                returnArray.AddRange(buffer.Take(count));
            } while (count == 1024);

            return returnArray.ToArray();
        }
    }
}
