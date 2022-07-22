using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.DataStore.DataModel
{
    public enum ParamDataType
    {
        Int32,
        IntArray,
        Double,
        DoubleArray,
        String,
        StringArray,
        Guid,
        GuidArray,
        ByteArray
    }
    public enum CauseStatus
    {
        Pending = 0,
        InProgress = 1,
        Complete = 2,
        Error = 3
    }
}
