using System;
using System.Collections.Generic;
using Implementation.Converters;
using Implementation.Repositories;
using Infrastructure.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ImplementationTest.RepositoryTests.Model
{
    public class User : AEntity, IUser
    {
        // [JsonConverter(typeof(GuidConverter))]
        // public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public User() : base()
        {
            
        }
        
    }
}
