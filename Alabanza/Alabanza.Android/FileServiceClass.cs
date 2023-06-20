using Alabanza.Droid;
using Alabanza.utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FileServiceClass))]
namespace Alabanza.Droid
{
    public class FileServiceClass: FileService
    {
        public string GetRootPath() { 
            return Application.Context.GetExternalFilesDir(null).ToString();
        }

        public void createFile(string[] paths)
        {
            var filename = "exportedFile.txt";
            var destination = Path.Combine(GetRootPath(),filename);
            string content = "";

            foreach (string path in paths)
            {
                string song = File.ReadAllText(path);
                content = content + song + "\n" + "ENDSONG" + "\n";
            }
            File.WriteAllText(destination, content);
        }
    }
}