using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Utility
{
    public static class UserImageExtension
    { 
        public static string ImgeToString(IFormFile ImageFile) 
        {

            string imapth = "";
            var uniqueFileName = GetUniqueFileName(ImageFile.FileName);
            var photos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos/UserImag", uniqueFileName);
            var filePath = Path.Combine(photos);
            ImageFile.CopyTo(new FileStream(filePath, FileMode.Create));
            imapth = "/photos/Products/" + uniqueFileName;

            return imapth;

        }
        private static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                + "_"
                + Guid.NewGuid().ToString().Substring(0, 4)
                + Path.GetExtension(fileName);
        }
    }
}
