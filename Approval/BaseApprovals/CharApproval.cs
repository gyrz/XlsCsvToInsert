using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XlsCsvToInsert.Approval.BaseApprovals
{
    public class CharApproval
    {
        private CharApproval _approval;

        public CharApproval SetNext(CharApproval approval)
        {
            _approval = approval;
            return _approval;
        }

        public virtual bool Handle(char value)
        {
            return _approval.Handle(value);
        }
    }
}
