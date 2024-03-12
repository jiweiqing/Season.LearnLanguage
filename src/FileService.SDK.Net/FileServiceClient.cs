using Learning.Domain;
using System.Security.Claims;
using System.Text.Json;

namespace FileService.SDK.Net
{
    public class FileServiceClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Uri _serverRoot;
        private readonly IJwtService _jwtService;

        public FileServiceClient(IHttpClientFactory httpClientFactory, Uri serverRoot, IJwtService jwtService)
        {
            _httpClientFactory = httpClientFactory;
            _jwtService = jwtService;
            _serverRoot = serverRoot;
        }

        public async Task<FileExistsResponse> FileExistsAsync(long fileSize, string sha256Hash, CancellationToken stoppingToken = default)
        {
            string relativeUrl = FormattableStringHelper.BuildUrl($"api/Uploader/FileExists?fileSize={fileSize}&sha256Hash={sha256Hash}");
            Uri requestUri = new Uri(_serverRoot, relativeUrl);
            var httpClient = _httpClientFactory.CreateClient();
            var result = await httpClient.GetStringAsync(requestUri, stoppingToken)!;
            return JsonSerializer.Deserialize<FileExistsResponse>(result)!;
        }

        string BuildToken()
        {
            //因为JWT的key等机密信息只有服务器端知道，因此可以这样非常简单的读到配置
            Claim claim = new Claim(ClaimTypes.Role, "Admin");
            return _jwtService.CreateToken(1, new Claim[] { claim }).AccessToken;
        }

        public async Task<Uri> UploadAsync(FileInfo file, CancellationToken stoppingToken = default)
        {
            string token = BuildToken();
            using MultipartFormDataContent content = new MultipartFormDataContent();
            using var fileContent = new StreamContent(file.OpenRead());
            content.Add(fileContent, "file", file.Name);
            var httpClient = _httpClientFactory.CreateClient();
            Uri requestUri = new Uri(_serverRoot + "/Uploader/Upload");
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var respMsg = await httpClient.PostAsync(requestUri, content, stoppingToken);
            if (!respMsg.IsSuccessStatusCode)
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                throw new HttpRequestException($"上传失败，状态码：{respMsg.StatusCode}，响应报文：{respString}");
            }
            else
            {
                string respString = await respMsg.Content.ReadAsStringAsync(stoppingToken);
                return JsonSerializer.Deserialize<Uri>(respString)!;
                //return respString.ParseJson<Uri>()!;
            }
        }
    }
}
