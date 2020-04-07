﻿using System;
using System.Collections.Generic;
using System.Text;
using Wcs.Common;
using Wcs.Models;

namespace Wcs.DAL
{
    public interface IStudentDAL
    {
        void Study(StudentModel model);

        [LogAfter]
        [LogBefore]
        void Run(StudentModel model);
    }
}
