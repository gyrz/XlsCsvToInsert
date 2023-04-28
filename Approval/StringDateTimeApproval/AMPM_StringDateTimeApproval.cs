using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XlsCsvToInsert.Approval.BaseApprovals;

namespace XlsCsvToInsert.Approval.StringDateTimeApproval
{
    public class AMPM_StringDateTimeApproval : StringApproval
    {
        public override bool Handle(string value)
        {
            if (value.Contains(" AM") || value.Contains(" PM")) return true;

            return base.Handle(value);
        }
    }
}
