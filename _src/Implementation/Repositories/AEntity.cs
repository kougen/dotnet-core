using System;
using Implementation.Converters;
using Infrastructure.Repositories;
using Newtonsoft.Json;

namespace Implementation.Repositories
{
    public abstract class AEntity : IEntity
    {
        [JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        protected AEntity()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }
    }
}
