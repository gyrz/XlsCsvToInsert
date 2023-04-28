using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsCsvToInsert
{
    internal interface IFileLoad
    {
        DataTable LoadData( string fileName );
    }
}
