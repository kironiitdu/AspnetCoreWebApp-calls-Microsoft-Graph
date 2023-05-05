using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Models
{
    public class DriveInfo
    {
        public int SerialNo { get; set; }
        public string DriveId { get; set; }
        public string ParentId { get; set; }
        public string FolderId { get; set; }
        public string FolderName { get; set; }
        public string FileName { get; set; }
        public string CreatedDateTime { get; set; }
        public string UserDisplayName { get; set; }
        public string WebUrl { get; set; }
        public string LastModifiedDateTime { get; set; }


    }
}
