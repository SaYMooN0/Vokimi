using Amazon.S3;
using Amazon.S3.Model;
using OneOf;
using VokimiShared.src;
using VokimiShared.src.models.db_classes;
using VokimiShared.src.models.db_entities_ids;

public class VokimiStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;


    public VokimiStorageService(IAmazonS3 s3Client, string bucketName) {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }
    private readonly Err
       fileUploadingErr = new Err("Failed to upload the file"),
       serverErr = new Err("Server error. Please try again later")
       ;
    private async Task<PutObjectResponse> PutObjectIntoStorage(string objKey, Stream fileStream) {
        PutObjectRequest putRequest = new() {
            BucketName = _bucketName,
            Key = objKey,
            InputStream = fileStream
        };

        return await _s3Client.PutObjectAsync(putRequest);
    }

    public async Task<OneOf<string, Err>> SaveDraftTestCover(DraftTestId id, Stream fileStream) {
        try {
            string key = $"{ImgOperationsHelper.DraftTestCoversFolder}/{id}";
            PutObjectResponse response = await PutObjectIntoStorage(key, fileStream);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return fileUploadingErr;
            }
        } catch (Exception ex) { return serverErr; }
    }
    public async Task<OneOf<string, Err>> SaveDraftTestQuestionImage(Stream fileStream, DraftTestQuestionId questionId, string sessionKey) {
        try {
            string key = $"{ImgOperationsHelper.DraftTestQuestionsFolder}/{questionId}/{sessionKey}";
            PutObjectResponse response = await PutObjectIntoStorage(key, fileStream);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return fileUploadingErr;
            }
        } catch (Exception ex) {
            return serverErr;
        }
    }
    public async Task<OneOf<string, Err>> SaveDraftTestAnswerImage(Stream fileStream, DraftTestQuestionId questionId) {
        try {
            string key = $"{ImgOperationsHelper.TestAnswersFolder}/{questionId}/{Guid.NewGuid()}";
            PutObjectResponse response = await PutObjectIntoStorage(key, fileStream);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return fileUploadingErr;
            }
        } catch (Exception ex) {
            return serverErr;
        }
    }
    public async Task<OneOf<string, Err>> SaveTestConclusionImage(Stream fileStream, DraftTestId testId, string imgSessionKey) {
        try {
            string key = $"{ImgOperationsHelper.TestConclusionsFolder}/{testId}/{imgSessionKey}";
            PutObjectResponse response = await PutObjectIntoStorage(key, fileStream);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return fileUploadingErr;
            }
        } catch { return serverErr; }
    }
    public async Task<OneOf<string, Err>> SaveDraftTestResultImage(Stream fileStream, DraftTestId testId, string inputKey) {
        try {
            string key = $"{ImgOperationsHelper.DraftTestResultsFolder}/{testId}/{inputKey}";
            PutObjectResponse response = await PutObjectIntoStorage(key, fileStream);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK) {
                return key;
            }
            else {
                return fileUploadingErr;
            }
        } catch { return serverErr; }
    }
    private async Task<Err> ClearUnusedImages(string prefix, IEnumerable<string>? reservedKeys = null) {
        try {
            var listRequest = new ListObjectsV2Request {
                BucketName = _bucketName,
                Prefix = prefix
            };

            ListObjectsV2Response listResponse;
            do {
                listResponse = await _s3Client.ListObjectsV2Async(listRequest);

                List<KeyVersion> objectsToDelete;

                if (reservedKeys is null || reservedKeys.Count() == 0) {
                    objectsToDelete = listResponse.S3Objects
                        .Select(o => new KeyVersion { Key = o.Key })
                        .ToList();
                }
                else {
                    objectsToDelete = listResponse.S3Objects
                        .Where(o => (reservedKeys == null || !reservedKeys.Contains(o.Key)))
                        .Select(o => new KeyVersion { Key = o.Key })
                        .ToList();
                }

                if (objectsToDelete.Any()) {
                    var deleteRequest = new DeleteObjectsRequest {
                        BucketName = _bucketName,
                        Objects = objectsToDelete
                    };

                    var deleteResponse = await _s3Client.DeleteObjectsAsync(deleteRequest);

                    if (deleteResponse.HttpStatusCode != System.Net.HttpStatusCode.OK) {
                        return new Err("Failed to delete some unused images");
                    }
                }

                listRequest.ContinuationToken = listResponse.NextContinuationToken;
            } while (listResponse.IsTruncated);
        } catch {
            return serverErr;
        }
        return Err.None;
    }
    public async Task<Err> ClearDraftTestConclusionUnusedImages(DraftTestId testId, string? usedKey = null) {
        string prefix = $"{ImgOperationsHelper.TestConclusionsFolder}/{testId}/";
        IEnumerable<string>? reservedKeys = usedKey is null ? null : [usedKey.ToString()];
        return await ClearUnusedImages(prefix, reservedKeys);
    }
    public async Task ClearUnusedDraftTestAnswerImagesForQuestion(DraftTestQuestionId questionId, IEnumerable<string> reservedKeys) {
        string prefix = $"{ImgOperationsHelper.TestAnswersFolder}/{questionId}/";
        await ClearUnusedImages(prefix, reservedKeys);
    }
    public async Task ClearUnusedDraftTestResultsImages(DraftTestId testId, IEnumerable<string> reservedKeys) {
        string prefix = $"{ImgOperationsHelper.DraftTestResultsFolder}/{testId}/";
        await ClearUnusedImages(prefix, reservedKeys);
    }
    public async Task ClearUnusedDraftTestQuestionImages(DraftTestQuestionId questionId, string? reservedKey) {
        string prefix = $"{ImgOperationsHelper.DraftTestQuestionsFolder}/{questionId}/";
        IEnumerable<string>? reservedKeys = reservedKey is null ? null : [reservedKey.ToString()];
        await ClearUnusedImages(prefix, reservedKeys);
    }
    public async Task<OneOf<string, Err>> MoveConclusionImageToPublished(string imageKey, DraftTestId draftTestId, TestId testId) {
        try {
            string newKey = $"{ImgOperationsHelper.TestsFolder}/{testId}/{ImgOperationsHelper.TestCoverFileName}";

            var copyRequest = new CopyObjectRequest {
                SourceBucket = _bucketName,
                SourceKey = imageKey,
                DestinationBucket = _bucketName,
                DestinationKey = newKey
            };

            CopyObjectResponse copyResponse = await _s3Client.CopyObjectAsync(copyRequest);

            if (copyResponse.HttpStatusCode == System.Net.HttpStatusCode.OK) {
            
                await ClearDraftTestConclusionUnusedImages(draftTestId);
                return newKey;
            }
            else {
                return fileUploadingErr;
            }
        } catch {
            return serverErr;
        }
    }

}
