using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsStore.Infrastucture
{
    public static class UrlHelpers
    {
        public static string ProductIconPath(this UrlHelper helper, string productFilename)
        {
            var productPictureFolder = AppConfig.PhotosFolderRelative; 
            var path = Path.Combine(productPictureFolder, productFilename);
            var absolutePath = helper.Content(path);
            return absolutePath; 
        }
    }
}