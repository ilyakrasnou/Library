using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Library.iOS.FileWorker))]
namespace Library.iOS
{
    class FileWorker:IFileWorker
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
            return Task<Stream>.FromResult((Stream) File.Create(filename));
        }

        public Task<Stream> OpenReadAsync(string filename)
        {
            return Task<Stream>.FromResult((Stream) File.OpenRead(filename));
        }
    }
}