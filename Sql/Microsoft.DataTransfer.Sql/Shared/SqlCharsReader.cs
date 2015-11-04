using System.Data.SqlTypes;
using System.IO;

namespace Microsoft.DataTransfer.Sql.Shared
{
    sealed class SqlCharsReader : TextReader
    {
        private long position;
        private SqlChars data;

        public SqlCharsReader(SqlChars data)
        {
            position = 0;
            this.data = data;
        }

        public override int Peek()
        {
            return data[position];
        }

        public override int Read()
        {
            if (position >= data.Length)
                return -1;

            return data[position++];
        }
    }
}
