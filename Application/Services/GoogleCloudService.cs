using Google.Cloud.Storage.V1;

namespace Application.Services;

/// <summary>
/// Сервис облака гугл
/// </summary>
public class GoogleCloudService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleCloudService()
    {
        var credentialsPath = "C:\\Users\\vantu\\Desktop\\Fanfic\\fanficsitecloudstorage-795b5b0f6e48.json";
        _bucketName = "fanfic-site-bucket";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", credentialsPath);
        _storageClient = StorageClient.Create();
    }
    
    /// <summary>
    /// Загрузка файла на облако
    /// </summary>
    /// <param name="fileStream">Стрим изображения.</param>
    /// <param name="fileName">Название изображения.</param>
    /// <param name="contentType">Тип файла.</param>
    /// <returns>Ссылка на файл в облаке.</returns>
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var objectName = fileName;
        await _storageClient.UploadObjectAsync(
            _bucketName,
            objectName,
            contentType,
            fileStream);
        
        return $"https://storage.googleapis.com/{_bucketName}/{objectName}";
    }
    
    /// <summary>
    /// Удаление файла в облаке
    /// </summary>
    /// <param name="fileName">Название файла.</param>
    public async Task DeleteFileAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, fileName);
        Console.WriteLine($"File {fileName} deleted from {_bucketName}");
    }
}