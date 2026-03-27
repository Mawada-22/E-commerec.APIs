using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contarcts
{
    public interface IDbInitializer
    {
        public Task IntialaizerAsync();
        public Task IdenetityIntialaizerAsync();
    }
}
