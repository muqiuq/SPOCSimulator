using System;
using System.Collections.Generic;
using System.Text;

namespace SPOCSimulator.HIO.Utils
{
    public interface IHIOOutput<T>
    {
        public T ToHIO();
    }
}
