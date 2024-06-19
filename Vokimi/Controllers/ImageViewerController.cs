using Amazon.S3.Model;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace Vokimi.Controllers
{
    [ApiController]
    [Route("vokimiimgs/[action]")]
    public class ImageViewerController : Controller
    {

        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        public ImageViewerController(IAmazonS3 s3Client, IConfiguration configuration) {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:BucketName"];
        }
       

        [HttpGet("{*fileKey}")]
        public async Task<IActionResult> GetImage(string fileKey) {
            try {
                var request = new GetObjectRequest {
                    BucketName = _bucketName,
                    Key = fileKey
                };

                using (var response = await _s3Client.GetObjectAsync(request)) {
                    using (var responseStream = response.ResponseStream) {
                        var memoryStream = new MemoryStream();
                        await responseStream.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        return File(memoryStream, response.Headers.ContentType, response.Key);
                    }
                }
            } catch (AmazonS3Exception ex) {
                return NotFound($"Error fetching file: {ex.Message}");
            } catch (Exception ex) {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
