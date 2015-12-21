using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuzzyLogic.Controller
{
    public enum RuleMethod
    {
        And,
        Or
    }

    public enum DeffuzificationMethod
    {
        CenterOfArea,
        CenterOfMaximum
    }

    public enum AggregationMethod
    {
        Max,
        Min,
        Sum
    }
}
