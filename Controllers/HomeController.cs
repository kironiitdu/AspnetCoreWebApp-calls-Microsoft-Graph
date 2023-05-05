using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using _2_1_Call_MSGraph.Models;
using System.IO;
using System.Collections.Generic;
using DriveInfo = _2_1_Call_MSGraph.Models.DriveInfo;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Linq;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;
using UserAssertion = Microsoft.IdentityModel.Clients.ActiveDirectory.UserAssertion;
using Newtonsoft.Json;
using Microsoft.Identity.Web.Resource;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Xml;
using System.Security.Claims;

namespace _2_1_Call_MSGraph.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly GraphServiceClient _graphServiceClient;
        //  static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        //  static readonly string[] scopesToAccessDownstreamApi = new string[] { "https://graph.microsoft.com/.default" };

        // private readonly ITokenAcquisition _tokenAcquisition;


        public HomeController(ILogger<HomeController> logger,
                          GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;
        }
        //public async Task<IActionResult> IndexAsync()
        //{
        //    HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);

        //    string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopesToAccessDownstreamApi);
        //    // return await callTodoListService(accessToken);
        //    return Ok(accessToken);
        //}
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost] //attribute to get posted values from HTML Form
        public async Task<IActionResult> GetTokenAsync()
        {
            AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/" + "3deed68b-4a47-1ab17ef7cfbe");
            ClientCredential clientCredential = new ClientCredential("b603c7be-a866-7-e6921e61f925", "Vxf1SluKbgu4PF2XDSeZ8wL/Yp8ns4sc=");
            var authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com/.default", clientCredential);
            return Ok(authResult);
        }
       // [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Index()
        {
            //var user = await _graphServiceClient.Me.Request().GetAsync();
            //ViewData["ApiResult"] = user.DisplayName;

            return View();
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> Profile()
        {


            var username = HttpContext.User.Identity.Name;

            var givenName = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value;
            var email = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var country = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value;
            var me = await _graphServiceClient.Me.Request().GetAsync();
            ViewData["Me"] = me;

            try
            {
                // Get user photo
                using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
                {
                    byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                    ViewData["Photo"] = Convert.ToBase64String(photoByte);
                }
            }
            catch (System.Exception)
            {
                ViewData["Photo"] = null;
            }

            return View();
        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> GetUsers()
        {

            var users = await _graphServiceClient
                       .Users
                       .Request()
                       .Top(999)  // <- Custom page of 999 records (if you want to set it)
                       .GetAsync()
                       .ConfigureAwait(false);

            while (true)
            {
                //TODO: relevant code here (process users)

                // If the page is the last one
                if (users.NextPageRequest is null)
                    break;

                // Read the next page
                users = await users
                  .NextPageRequest
                  .GetAsync()
                  .ConfigureAwait(false);
            }

            return View();
        }


        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> GetPlannerInfo()
        {
            try
            {

                var tasks = await _graphServiceClient.Me.Planner.Tasks
                    .Request()
                    .GetAsync();
                ViewData["PlanerInfo"] = tasks.CurrentPage;
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> GetDriveFolders()
        {
            try
            {
                var folders = await _graphServiceClient.Me.Drive.Root.Children.Request().GetAsync();
                ViewData["folderInfo"] = folders.CurrentPage;
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<IActionResult> UploadFileOnFolder(IFormFile uploadedFile, string folderName)
        {
            try
            {

                if (uploadedFile.Length > 0)
                {
                    dynamic response;

                    var fileName = uploadedFile.FileName;
                    using var stream = uploadedFile.OpenReadStream();
                    response = await _graphServiceClient.Me.Drive.Root.ItemWithPath(folderName + "/" + fileName).Content.Request().PutAsync<DriveItem>(stream);
                    var uploadedFolderName = response.ParentReference.Path;
                    //Bind to model
                    Models.RecentFiles _objFileInfo = new Models.RecentFiles();

                    _objFileInfo.SerialNo = 1;
                    _objFileInfo.FileName = response.Name;
                    _objFileInfo.ParentFolderId = uploadedFolderName.Substring(13);
                    _objFileInfo.WebUrl = response.WebUrl;
                    _objFileInfo.CreatedDateTime = response.CreatedDateTime.ToString();


                    ViewData["filesInfo"] = _objFileInfo;
                }
                else
                {
                    ViewData["message"] = "Sorry Upload failed! Please check your file!";
                }

                //Return to view
                ViewBag.message = "File Uploaded Successfully!";
                // ViewData["filesInfo"] = _listOfFiles;

            }
            catch (Exception ex)
            {

                ViewData["message"] = ex.Message;
            }
            return View();
            //return RedirectToAction("RecentFileInfo");
        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<IActionResult> SubscribeOnDrive()
        {
            try
            {
                var sub = new Microsoft.Graph.Subscription();
                var newSubscription = await _graphServiceClient.Subscriptions.Request().AddAsync(sub);
            }
            catch (System.Exception)
            {
                ViewData["driveInfo"] = null;
            }
            return RedirectToAction("RecentDirveInfo");

        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<IActionResult> CreateFolderOnDrive(string folderName)
        {
            try
            {
                var driveItem = new DriveItem
                {
                    Name = folderName,
                    Folder = new Folder
                    {
                    }
                };

                var item = await _graphServiceClient.Me.Drive.Root.Children
                      .Request()
                      .AddAsync(driveItem);

                //Return to view
                ViewData["folderInfo"] = item;

            }
            catch (System.Exception)
            {
                ViewData["driveInfo"] = null;
            }
            return RedirectToAction("RecentDirveInfo");

        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<IActionResult> DeleteFolderOnDrive(string fileId, string fileName)
        {
            try
            {
                await _graphServiceClient.Me.Drive.Root.ItemWithPath(fileId + "/" + fileName).Request().DeleteAsync();
                //await _graphServiceClient.Me.Drive.Items[fileId].Request().DeleteAsync();


                //Return to view
                ViewBag.message = "Deleted Successfully!";

            }
            catch (System.Exception ex)
            {
                ViewBag.message = "Deleted Failed! Please check your folder name!";
            }
            return RedirectToAction("RecentFileInfo");

        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> RecentFileInfo()
        {
            try
            {
                //Get Recent File Info
                var recentFileInfo = await _graphServiceClient.Me.Drive.Recent().Request().GetAsync();

                //Bind Drive Info to model
                List<RecentFiles> _listOfFiles = new List<RecentFiles>();
                int serial = 1;
                foreach (var item in recentFileInfo)
                {
                    Models.RecentFiles _objFileInfo = new Models.RecentFiles();
                    _objFileInfo.SerialNo = serial++;
                    _objFileInfo.FileId = item.Id;
                    _objFileInfo.FileName = item.Name;
                    _objFileInfo.WebUrl = item.WebUrl;
                    _objFileInfo.CreatedDateTime = item.CreatedDateTime.ToString();
                    _listOfFiles.Add(_objFileInfo);
                }

                //Return to view
                ViewBag.message = "File Uploaded Successfully!";
                ViewData["filesInfo"] = _listOfFiles;

            }
            catch (System.Exception)
            {
                ViewData["filesInfo"] = null;
            }

            return View();
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> SharedFileInfo()
        {
            try
            {
                var sharedWithMe = await _graphServiceClient.Me.Drive
               .SharedWithMe()
               .Request()
               .GetAsync();
                //Bind Drive Info to model
                List<RecentFiles> _listOfSharedFiles = new List<RecentFiles>();
                int serial = 1;
                if (sharedWithMe.Count != 0)
                {
                    foreach (var item in sharedWithMe)
                    {
                        Models.RecentFiles _objFileInfo = new Models.RecentFiles();
                        _objFileInfo.SerialNo = serial++;
                        _objFileInfo.FileId = item.Id;
                        _objFileInfo.ParentFolderId = item.RemoteItem.ParentReference.Id;
                        _objFileInfo.FileName = item.Name;
                        _objFileInfo.WebUrl = item.WebUrl;
                        _objFileInfo.CreatedDateTime = item.CreatedDateTime.ToString();
                        _listOfSharedFiles.Add(_objFileInfo);
                    }

                }
                else
                {
                    Models.RecentFiles _objFileInfo = new Models.RecentFiles();
                    _objFileInfo.SerialNo = serial++;
                    _objFileInfo.FileId = "No Item Found";
                    _objFileInfo.FileName = "No Item Found";
                    _objFileInfo.ParentFolderId = "-";
                    _objFileInfo.WebUrl = "-";
                    _objFileInfo.CreatedDateTime = "-";
                    _listOfSharedFiles.Add(_objFileInfo);

                }


                //Return to view
                ViewData["filesShareInfo"] = _listOfSharedFiles;

            }
            catch (System.Exception ex)
            {
                ViewData["filesShareInfo"] = null;
            }

            return View();
        }

        public async Task<IActionResult> SendEmailFromGraphAPI()
        {


            var message = new Message
            {
                Subject = "Email From Microsoft Graph API?",
                Body = new ItemBody
                {
                    ContentType = BodyType.Text,
                    Content = "The new cafeteria is open."
                },
                ToRecipients = new List<Recipient>()
                        {
                            new Recipient
                            {
                                EmailAddress = new EmailAddress
                                {
                                    Address = "fannyd@contoso.onmicrosoft.com"
                                }
                            }
                        },
                CcRecipients = new List<Recipient>()
                            {
                                new Recipient
                                {
                                    EmailAddress = new EmailAddress
                                    {
                                        Address = "danas@contoso.onmicrosoft.com"
                                    }
                                }
                            }
            };

            var saveToSentItems = false;

            await _graphServiceClient.Me
                .SendMail(message, saveToSentItems)
                .Request()
                .PostAsync();
            return Ok();
        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        public async Task<IActionResult> RecentDirveInfo()
        {
            try
            {


                InvitedUserMessageInfo info = new InvitedUserMessageInfo();
                object obj = new object();
                obj = "Testing Another Additional Data";

                var dict = new Dictionary<string, object>();
                dict.Add("Data1", obj);
                dict.Add("Data2", obj);
                info.AdditionalData = dict;

                var AdditionalData = new Dictionary<string, object>
                    {
                        {"OtherEmail", "Test"},
                        {"OtherRole" , "Test"}
                    };

                info.AdditionalData = AdditionalData;

                var invitation = new Invitation
                {
                    InvitedUserEmailAddress = "kironiitdu@outlook.com",
                    InviteRedirectUrl = "https://yourapplicationurl.com",
                    InvitedUserMessageInfo = new InvitedUserMessageInfo { AdditionalData = AdditionalData },
                };

                var data = await _graphServiceClient.Invitations
                      .Request()
                      .AddAsync(invitation);
                //Get Folder Info
                var driveInfo = await _graphServiceClient.Me.Drive.Root.Children.Request().GetAsync();













                //Bind Drive Info to model
                List<DriveInfo> list = new List<DriveInfo>();
                int serial = 1;
                foreach (var item in driveInfo)
                {
                    Models.DriveInfo _objDriveInfo = new Models.DriveInfo();
                    _objDriveInfo.SerialNo = serial++;
                    _objDriveInfo.DriveId = item.ParentReference.DriveId;
                    _objDriveInfo.ParentId = item.ParentReference.Id;
                    _objDriveInfo.FolderId = item.Id;
                    _objDriveInfo.FolderName = item.Name;
                    _objDriveInfo.UserDisplayName = item.CreatedBy.User.DisplayName;
                    _objDriveInfo.CreatedDateTime = item.CreatedDateTime.ToString();
                    _objDriveInfo.LastModifiedDateTime = item.LastModifiedDateTime.ToString();
                    _objDriveInfo.WebUrl = item.WebUrl;
                    list.Add(_objDriveInfo);
                }


                ViewData["driveInfo"] = list;

            }
            catch (System.Exception ex)
            {
                ViewData["driveInfo"] = null;
            }

            return View();
        }
        public class ClassData
        {
            [JsonProperty("Id")]
            public string InsertId { get; set; }


            [JsonProperty("ClassID")]
            public string ClassID { get; set; }


            [JsonProperty("InternalID")]
            public string InternalId { get; set; }


            [JsonProperty("ID")]
            public string ID { get; set; }

            [JsonProperty("Name")]
            public string Name { get; set; }
        }

        public ActionResult ImportJsonClassData(ClassData classData)
        {
            try
            {
                ClassData _data = new ClassData();
                _data.ClassID = classData.ClassID ?? "If null then setting and empty data into database";
                _data.ID = classData.ID ?? "If null then setting and empty data into database";
                _data.InsertId = classData.InsertId ?? "If null then setting and empty data into database";

                //action to insert into table


                if (classData.ClassID == null)
                {
                    _data.ClassID = "";
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
                //  ex.Message  
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public static List<string> room1Inventory = new List<string> { "key", "dog" };
        public static List<string> room2Inventory = new List<string> { "gun", "banana", "clown" };
        static void DisplayListItems(List<string> whichList)
        {
            Console.WriteLine("{0}", string.Join(", ", whichList));
            Console.ReadKey();
        }


        public async Task<object> FindByUserType()
        {

            try
            {

                //Initialize on behalf of user token aquisition service
                var _tokenAcquisition = this.HttpContext.RequestServices
               .GetRequiredService<ITokenAcquisition>() as ITokenAcquisition;
                //define the scope
                string[] scopes = new string[] { "https://graph.microsoft.com/.default" };
            
                //Getting token from Azure Active Directory
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
                //Request Grap API end point
                HttpClient _client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("https://graph.microsoft.com/v1.0/me"));
                //Passing Token For this Request
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _client.SendAsync(request);
                //Get User into from grpah API
                dynamic userInfo = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());


                return userInfo;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public async Task<object> PostXMLRequestAspNet5API()
        {

            try
            {
                //Console.WriteLine("Exemplo ticketBai");
                var handler = new HttpClientHandler();
                //handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                //handler.SslProtocols = SslProtocols.Tls12;
                //handler.ClientCertificates.Add(new X509Certificate2("Certificado", "Password"));
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                var client = new HttpClient(handler);

                var httpContent = new StringContent("@FaridKironTestAsp.net5API", Encoding.UTF8, "text/xml");
                client.DefaultRequestHeaders.Accept.Clear();

                var VResultado = await client.PostAsync("http://localhost:4400/home/PostAsync", httpContent);
                if (VResultado.IsSuccessStatusCode == false)
                {
                    return null;
                }
                else
                {
                    //Aceptado no necesaria verificacion
                    var data = await VResultado.Content.ReadAsStringAsync();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(data);
                    XmlNodeList nodeList = xmldoc.GetElementsByTagName("string");
                    string response = string.Empty;
                    foreach (XmlNode node in nodeList)
                    {
                        response = node.InnerText;
                    }
                    Console.WriteLine(response);
                    return response;
                    //}
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<object> GetUserInfoFromGraphAPI(Customer model)
        {

            try
            {
                var postXmlRequest = await PostXMLRequestAspNet5API();
                var callgraphApi = await TokenControl();

                //Initialize on behalf of user token aquisition service

                var _tokenAcquisition = this.HttpContext.RequestServices.GetRequiredService<ITokenAcquisition>() as ITokenAcquisition;

                //define the scope
                string[] scopes = new string[] { "https://graph.microsoft.com/.default" };

                //Getting token from Azure Active Directory
                string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

                //Request Grap API end point
                HttpClient _client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, string.Format("https://graph.microsoft.com/v1.0/me"));

                //Passing Token For this Request
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = await _client.SendAsync(request);

                //Get User into from grpah API
                dynamic userInfo = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());


                return userInfo;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost] //attribute to get posted values from HTML Form
        public async Task<IActionResult> AddCustomer(Customer model)
        {
            //var data = await FindByUserType();
            // var data = await GetUserInfoFromGraphAPI();
            //AuthenticationContext authContext = new AuthenticationContext("https://login.microsoftonline.com/" + "3deed68b-4a9f-4ce5-aa47-1ab17ef7cfbe");
            //ClientCredential clientCredential = new ClientCredential("b603c7be-a866-4aea-ad87-e6921e61f925", "Vxf1SluKbgu4PF0Nf3wE5oGl/2XDSeZ8wL/Yp8ns4sc=");
            //var authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com", clientCredential);

            var clientID = "b603c7be-a866-4ae6921e61f925";
            var clientSecret = "Vxf1SluKbDSeZ8wL/Yp8ns4sc=";
            var tenant = "3deed68b--1ab17ef7cfbe";

            var appCred = new ClientCredential(clientID, clientSecret);

            var authContext = new AuthenticationContext(
                "https://login.microsoftonline.com/" + tenant);

            var authResult = await authContext.AcquireTokenAsync("https://graph.microsoft.com", appCred);


            return Ok();
        }
        [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
        [HttpPost]
        public async Task<IActionResult> CreateInvitation()
        {
            InvitedUserMessageInfo info = new InvitedUserMessageInfo();
            info.AdditionalData.Add("Data1", "Testing Additional Data");
            info.AdditionalData.Add("Data2", "Testing Another Additional Data");
            var invitation = new Invitation
            {
                InvitedUserEmailAddress = "test@microsoft.com",
                InviteRedirectUrl = "https://myapp.contoso.com",
                InvitedUserMessageInfo = info
            };

            await _graphServiceClient.Invitations
                .Request()
                .AddAsync(invitation);
            return Ok();
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> TokenControl()
        {
            try
            {
                var clientID = "00ab01dc-0787e26-875da8fbbf8e";//"b603c7be-a866d87-e6921e61f925";
                var clientSecret = "m.Q7Q~.FEkMjyVO2gvGw0j";//"Vxf1SluKbgu4PF0Nf2XDSeZ8wL/Yp8ns4sc=";
                var tenant = "e4c9ab4e-bd27-40ba2a757fb";//"3deed68b-4a9f-4ce5-a17ef7cfbe";
                IConfidentialClientApplication app;
                app = ConfidentialClientApplicationBuilder.Create(clientID)
                                                          .WithClientSecret(clientSecret)
                                                          .WithAuthority(new Uri("https://login.microsoftonline.com/" + tenant))
                                                          .Build();

                var result = await app.AcquireTokenForClient(new List<string>() { "https://graph.microsoft.com/.default" })
                                  .ExecuteAsync();

                HttpClient sender = new HttpClient();
                sender.DefaultRequestHeaders.Add(
                          "Authorization",
                           String.Format("Bearer " + result.AccessToken)
                           );
                HttpResponseMessage meResult = await sender.GetAsync("https://graph.microsoft.com/v1.0/users/Kiron@hanxia.onmicrosoft.com/photo/$value");
                string context = await meResult.Content.ReadAsStringAsync();
                //   byte[] photoByte = ((MemoryStream)context.)
                // ViewData["photo"] = Convert.ToBase64String(context);
                Console.WriteLine("WAAA");
                return Ok();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
