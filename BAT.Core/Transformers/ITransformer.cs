﻿using System.Collections.Generic;
using BAT.Core.Common;

namespace BAT.Core.Transformers
{
    public interface ITransformer
    {
        List<SensorReading> Transform(List<SensorReading> input);
    }
}