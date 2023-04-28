using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsCsvToInsert
{
    internal abstract class FileLoad : IFileLoad
    {
        public virtual DataTable LoadData(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
