using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XlsCsvToInsert.Approval.BaseApprovals;

namespace XlsCsvToInsert.Approval.StringNumberApproval
{
    public class Num_StringNumberApproval : CharApproval
    {
        private static char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public override bool Handle(char value)
        {
            if (numbers.Contains(value)) return true;

            return base.Handle(value);
        }
    }
}
