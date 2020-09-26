using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Srever
{
    using System;
    using System.Collections.Generic;

    public partial class ANSWER
    {
        public int Id { get; set; }
        public Nullable<int> QuestionId { get; set; }
        public string Answer1 { get; set; }
        public Nullable<bool> Result { get; set; }

        public virtual QUESTION QUESTION { get; set; }
    }
}
