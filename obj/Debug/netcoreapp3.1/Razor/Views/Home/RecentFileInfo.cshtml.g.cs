#pragma checksum "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "89bc31db916823b7a93e63c9376fcf12a657005d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_RecentFileInfo), @"mvc.1.0.view", @"/Views/Home/RecentFileInfo.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\_ViewImports.cshtml"
using _2_1_Call_MSGraph;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
using Newtonsoft.Json.Linq;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
using _2_1_Call_MSGraph.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"89bc31db916823b7a93e63c9376fcf12a657005d", @"/Views/Home/RecentFileInfo.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c8f405b3760fadc064a50fd999fd9e2ef9558e1", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_RecentFileInfo : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
  
    ViewData["Title"] = "RecentFileInfo";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n<br />\r\n\r\n<table class=\"table-bordered \">\r\n    <tbody>\r\n");
#nullable restore
#line 12 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
          
            //as Microsoft.Graph.Drive
            var fileInfo = (List<RecentFiles>)ViewData["filesInfo"];
            foreach (var item in fileInfo)
            {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n\r\n                    <td><strong>Serial No</strong></td>\r\n                    <td><strong> ");
#nullable restore
#line 21 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
                            Write(item.SerialNo);

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong></td>\r\n                </tr>\r\n                <tr>\r\n\r\n                    <td>File Name</td>\r\n                    <td> ");
#nullable restore
#line 26 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
                    Write(item.FileName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n                <tr>\r\n                    <td>Web URL</td>\r\n                    <td><a");
            BeginWriteAttribute("href", " href=\"", 741, "\"", 760, 1);
#nullable restore
#line 30 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
WriteAttributeValue("", 748, item.WebUrl, 748, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" target=\"_blank\">");
#nullable restore
#line 30 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
                                                          Write(item.WebUrl);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></td>\r\n                </tr>\r\n                <tr>\r\n                    <td>Created Time</td>\r\n                    <td>");
#nullable restore
#line 34 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"
                   Write(item.CreatedDateTime);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n                </tr>\r\n");
            WriteLiteral("                <tr>\r\n                    <td><hr /></td>\r\n                    <td><hr /></td>\r\n                </tr>\r\n");
#nullable restore
#line 54 "D:\OneDrive - Microsoft\Desktop\2-1-Call-MSGraph\Views\Home\RecentFileInfo.cshtml"

            }

        

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
