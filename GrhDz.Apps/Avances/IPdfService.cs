using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrhDz.Domains.Models.Avances;

namespace GrhDz.Apps.Avances
{
    public interface IPdfService
    {
        Task<byte[]> GenerateAvancePdfAsync(List<AvanceModel> avances, DateTime date);
    }
}
