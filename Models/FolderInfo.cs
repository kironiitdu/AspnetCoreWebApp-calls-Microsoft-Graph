using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Models
{
    public class FolderInfo
    {
       
        public string DriveId { get; set; }
        public string Name { get; set; }
        public string CreatedDateTime { get; set; }
        public string UserDisplayName { get; set; }
        public string WebUrl { get; set; }
        public string LastModifiedDateTime { get; set; }
    }
}
