﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VVVV.Nodes
{
    class CustomException : Exception
    {
        public CustomException(string message) : base(message)
        {
            
        }
    }
}
