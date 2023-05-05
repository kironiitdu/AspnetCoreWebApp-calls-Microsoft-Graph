using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace _2_1_Call_MSGraph.Controllers
{
    public class DropboxController : Controller
    {
       
        
        public static async Task<string> UploadFile(string folder, string fileName, string fileUri)
        {
           var dropBoxClient = new DropboxClient("Token","AppKey");
            using (var ms = new FileStream(fileUri, FileMode.Open, FileAccess.Read))
            {
                FileMetadata updated = await dropBoxClient.Files.UploadAsync(
                folder + "/" + fileName,
                WriteMode.Overwrite.Instance,
                body: ms);

                var shareLinkInfo = new Dropbox.Api.Sharing.CreateSharedLinkWithSettingsArg(folder + "/" + fileName);
                var reponseShare = await dropBoxClient.Sharing.CreateSharedLinkWithSettingsAsync(shareLinkInfo);
                return reponseShare.Url;
            }
        }
    }
}
