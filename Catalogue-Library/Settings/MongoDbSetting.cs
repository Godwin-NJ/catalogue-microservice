using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue_Library.Settings
{
    public class MongoDbSetting
    {
        public string Host {  get; init; }
        public int Port { get; init; }
        public string ConnectionString => $"mongodb://{Host}/{Port}";
    }
}
