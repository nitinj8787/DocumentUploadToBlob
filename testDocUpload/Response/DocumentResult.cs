using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace testDocUpload.Response
{
    public class DocumentResult
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public long? FileSize { get; set; }
    }
}
