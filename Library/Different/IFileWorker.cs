using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Library
{
    public interface IFileWorker
    {
        Task<bool> ExistsAsync(string filename); // проверка существования файла
        Task<Stream> CreateAsync(string filename); // возвращаем поток записи файла
        Task<Stream> OpenReadAsync(string filename);
        Task DeleteAsync(string filename);  // удаление файла
    }
}
