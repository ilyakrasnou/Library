using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Windows.Storage;
using System.IO;

[assembly: Dependency(typeof(Library.UWP.FileWorker))]
namespace Library.UWP
{
    class FileWorker:IFileWorker
    {
        public async Task DeleteAsync(string filename)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filename);
            await file.DeleteAsync();
            /*StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            if (await ExistsAsync(filename))
            {
                StorageFile storageFile = await localFolder.GetFileAsync(filename);
                await storageFile.DeleteAsync();
            }*/
        }

        public async Task<bool> ExistsAsync(string filename)
        {
            /*var file = await StorageFile.GetFileFromPathAsync(filename);
            if (file == null) return false;
            else return true;*/
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            try
            {
                await localFolder.GetFileAsync(filename);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<Stream> CreateAsync(string filename)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filename);
            if (file == null)
            {
                throw new NotImplementedException();
            }
            else
            {
                return (await file.OpenTransactedWriteAsync()).Stream.AsStreamForWrite();
            }
        }

        public async Task<Stream> OpenReadAsync(string filename)
        {
            StorageFile file = await StorageFile.GetFileFromPathAsync(filename);
            return await file.OpenStreamForReadAsync();
        }
    }
}
