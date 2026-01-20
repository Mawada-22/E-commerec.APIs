using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record PaginationResult<TData>(int pageSize,int pageIndex,int countDataRecords,IEnumerable<TData>Data)
    {


    }
}
