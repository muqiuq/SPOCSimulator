using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace SPOCSimulator.ContentManager
{
    public abstract class BaseContentManager
    {
        protected JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

    }
}
