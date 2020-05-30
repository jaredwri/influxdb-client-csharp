using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Core.Proposal
{
    public interface IWriteApi
    {
        Task WriteAsync<T>(T measurement) where T : class;
    }
}
