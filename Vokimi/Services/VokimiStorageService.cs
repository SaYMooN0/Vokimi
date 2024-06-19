using Amazon.S3;
using Amazon.S3.Model;
using OneOf;
using Vokimi.src;
using VokimiShared.src;
using VokimiShared.src.models.db_classes;

public class VokimiStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;


    public VokimiStorageService(IAmazonS3 s3Client, string bucketName) {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }
    public async Task<OneOf<string, Err>> SaveDraftTestCover(DraftTestId id, Stream fileStream) {
        try {
            string key = $"{ImgOperationsHelper.GeneralFolder}/{id}";
            var putRequest = new PutObjectRequest {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream
            };

            PutObjectResponse response = await _s3Client.PutObjectAsync(putRequest);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return new Err("Failed to upload the file");
            }
        } catch (Exception ex) {
            return new Err("Server error. Please try again later");
        }
    }

}
