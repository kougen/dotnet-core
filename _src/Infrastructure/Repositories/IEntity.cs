using System;

namespace Infrastructure.Repositories
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
