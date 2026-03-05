using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record PaginationResult<TData>(int pageSize, int pageIndex, int countDataRecords, IEnumerable<TData> Data)
    {


    }
}
