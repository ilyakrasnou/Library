using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Library;
using System.Threading.Tasks;
using System.IO;

[assembly: Dependency(typeof(Library.Droid.FileWorker))]
namespace Library.Droid
{ 
    class FileWorker: IFileWorker
    {
        public Task DeleteAsync(string filename)
        {
            // удаляем файл
            File.Delete(filename);
            return Task.FromResult(true);
        }

        public Task<bool> ExistsAsync(string filename)
        {
            bool exists = File.Exists(filename);
            return Task<bool>.FromResult(exists);
        }

        public Task<Stream> CreateAsync(string filename)
        {
            return Task<Stream>.FromResult((Stream)File.Create(filename));
        }

        public Task<Stream> OpenReadAsync(string filename)
        {
            return Task<Stream>.FromResult((Stream) File.OpenRead(filename));
        }
    }
}
