using System;

namespace datumation_products.Shared.Infrastructure.Logging
{
    public interface ILogFactory
    {
        void WriteMessage(string msg);

        void WriteMessage(string msg, Exception ex);
        void WriteMessageAsync(string msg);
    }
}