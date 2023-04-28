using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsCsvToInsert.Approval.BaseApprovals
{
    public class StringApproval
    {
        private StringApproval _approval;

        public StringApproval SetNext(StringApproval approval)
        {
            _approval = approval;
            return _approval;
        }

        public virtual bool Handle(string value)
        {
            return _approval.Handle(value);
        }
    }
}
