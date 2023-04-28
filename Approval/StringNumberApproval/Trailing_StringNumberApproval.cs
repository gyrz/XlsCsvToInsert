using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XlsCsvToInsert.Approval.BaseApprovals;

namespace XlsCsvToInsert.Approval.StringNumberApproval
{
    public class Trailing_StringNumberApproval : CharApproval
    {
        public override bool Handle(char value)
        {
            if (value == '.') return true;

            return base.Handle(value);
        }
    }
}
