using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Routing;
using MasterCore8.Data;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace MasterCore8.Helpers
{
    public static class HelperExtensions
    {
        // Url.AbsoluteContent("~url")
        public static string AbsoluteContent(this IUrlHelper url, string contentPath)
        {
            HttpRequest request = url.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value+request.PathBase), url.Content(contentPath)).ToString();
        }

        public static string NullIfWhiteSpace(this string value) {
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }
    }
}
