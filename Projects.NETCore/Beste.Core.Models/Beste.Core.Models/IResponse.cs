using System;

namespace Beste.Core.Models
{
    public interface IResponse<T>
    {
        T Result { get; }
    }

}
