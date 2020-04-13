using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Common;
using Wcs.Models;

namespace Wcs.BLL
{
    public interface IStudentBLL
    {
        void Study(StudentModel model);

        [LogAfter]
        [LogBefore]
        void Run(StudentModel model);
    }
}
