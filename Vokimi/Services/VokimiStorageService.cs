using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OneOf;
using OneOf.Types;
using System.Collections;
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
            string key = $"{ImgOperationsHelper.DraftTestCoversFolder}/{id}";
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
    public async Task<OneOf<string, Err>> SaveDraftTestAnswerImage(Stream fileStream, DraftTestQuestionId questionId) {
        try {
            string key = $"{ImgOperationsHelper.DraftTestAnswersFolder}/{questionId}/{Guid.NewGuid()}";
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
    public async Task ClearUnusedAnswerImagesForQuestion(DraftTestQuestionId questionId, IEnumerable<string> reservedKeys) {
        try {
            var listRequest = new ListObjectsV2Request {
                BucketName = _bucketName,
                Prefix = $"{ImgOperationsHelper.DraftTestAnswersFolder}/{questionId}/"
            };

            ListObjectsV2Response listResponse;
            do {
                listResponse = await _s3Client.ListObjectsV2Async(listRequest);
                var keysToDelete = listResponse.S3Objects
                    .Select(o => o.Key)
                    .Where(key => !reservedKeys.Contains(key))
                    .ToList();

                if (keysToDelete.Any()) {
                    var deleteRequest = new DeleteObjectsRequest {
                        BucketName = _bucketName,
                        Objects = keysToDelete.Select(key => new KeyVersion { Key = key }).ToList()
                    };

                    await _s3Client.DeleteObjectsAsync(deleteRequest);
                }

                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated);
        } catch (Exception ex) { }
    }


}
