﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasMe.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, Exception? Inner = null) : base(message, Inner)
        {

        }
        public NotFoundException(string message) : base(message)
        {

        }
    }
}