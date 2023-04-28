using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XlsCsvToInsert.Approval.BaseApprovals;

namespace XlsCsvToInsert.Approval.StringDateTimeApproval
{
    public class Format_StringDateTimeApproval : StringApproval
    {
        public override bool Handle(string value)
        {
            if (value.Contains("d") || value.Contains("y") || value.Contains("m")) return true;

            return base.Handle(value);
        }
    }
}
