using GrhDz.Domains.Models.Avances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrhDz.Apps.Avances
{
    public interface IPdfService
    {
        Task<byte[]> GenerateAvancePdfAsync(List<Avance> avances, DateTime date);
    }

}
