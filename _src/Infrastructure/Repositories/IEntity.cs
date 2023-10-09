using System;
using System.Text.Json.Serialization;

namespace Infrastructure.Repositories
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}
