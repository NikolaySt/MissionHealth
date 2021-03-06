using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialNet.Backend.Helpers
{
    public class MinifyResponseHelper
    {
        public static Stream Process(Stream outputStream)
        {
            return new WhiteSpaceFilter(outputStream, s =>
            {
                s = Regex.Replace(s, @"\s+", " ");
                s = Regex.Replace(s, @"\s*\n\s*", "\n");
                s = Regex.Replace(s, @"\s*\>\s*\<\s*", "><");
                s = Regex.Replace(s, @"<!--(.*?)-->", "");   //Remove comments

				// single-line doctype must be preserved 
				var firstEndBracketPosition = s.IndexOf(">");				
                if (firstEndBracketPosition >= 0)
                {
                    s = s.Remove(firstEndBracketPosition, 1);
                    s = s.Insert(firstEndBracketPosition, ">");
                }

				//s = Regex.Replace(s, @"[^\xFFFD]", "");		 
                return s;                
            });
        }
    }

    public class WhiteSpaceFilter : Stream
    {

        private Stream _shrink;
        private Func<string, string> _filter;

        public WhiteSpaceFilter(Stream shrink, Func<string, string> filter)
        {
            _shrink = shrink;
            _filter = filter;
        }

        public override bool CanRead { get { return true; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return true; } }
        public override void Flush() { _shrink.Flush(); }
        public override long Length { get { return 0; } }
        public override long Position { get; set; }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _shrink.Read(buffer, offset, count);
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _shrink.Seek(offset, origin);
        }
        public override void SetLength(long value)
        {
            _shrink.SetLength(value);
        }
        public override void Close()
        {
            _shrink.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // capture the data and convert to string 
            Debug.WriteLine("===============================================================");
            Debug.WriteLine("WhiteSpaceFilter - Buffer: " + count);
            Debug.WriteLine("===============================================================");
            byte[] data = new byte[count];
            Buffer.BlockCopy(buffer, offset, data, 0, count);
            string s = Encoding.UTF8.GetString(data);

            // filter the string
            s = _filter(s);

            // write the data to stream 
            byte[] outdata = Encoding.UTF8.GetBytes(s);
            _shrink.Write(outdata, 0, outdata.GetLength(0));
        }
    }
}