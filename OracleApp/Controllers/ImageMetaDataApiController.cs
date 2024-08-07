using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OracleApp.DTO;

namespace OracleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageMetaDataApiController : ControllerBase
    {
        private readonly ILogger<ImageMetaDataApiController> _logger;
        private const String MetaDataFileExt = ".txt";
        private const String PdfExt = ".pdf";
        private const Char InputAgentFiledSeparator = '|';
        public ImageMetaDataApiController(ILogger<ImageMetaDataApiController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "Create")]
        public async Task<ApiResponse> Create([FromBody] ImageAppInfo appInfo, String customPrefix, String dateStringFormat,
                                      String filePath, String caseNumber, String masNumber, String docType,
                                      DateTime scanDate, String status, String documentId, String controlNumber,
                                      String scanOperator, String externalId, String internalId, String source,
                                      String fileType, String fileSize, DateTime receivedDate, String message,
                                      String otherData, String url, String docCategory, String macId, String batchName)
        {
            _logger.LogInformation($"{nameof(Create)} for file {filePath} begins.");
            var result = CreateLocalMethod(appInfo, customPrefix, dateStringFormat, filePath, caseNumber, masNumber,
                                        docType, scanDate, status, documentId, controlNumber, scanOperator, externalId,
                                        internalId, source, fileType, fileSize, receivedDate, message, otherData, url,
                                        docCategory, macId, batchName);
            _logger.LogInformation($"{nameof(Create)} for file {filePath} ends. Result was {result.Result}");
            return result;
        }

        private ApiResponse CreateLocalMethod(ImageAppInfo appInfo, String customPrefix, String dateStringFormat,
                                      String filePath, String caseNumber, String masNumber, String docType,
                                      DateTime scanDate, String status, String documentId, String controlNumber,
                                      String scanOperator, String externalId, String internalId, String source,
                                      String fileType, String fileSize, DateTime receivedDate, String message,
                                      String otherData, String url, String docCategory, String macId, String batchName)
        {
            int retryCount = 0;
            int maxRetry = 5;
            ApiResponse apiResponse = null;
            try
            {
                apiResponse = CreateMetadata(appInfo, customPrefix, dateStringFormat, filePath, caseNumber, masNumber,
                                        docType, scanDate, status, documentId, controlNumber, scanOperator, externalId,
                                        internalId, source, fileType, fileSize, receivedDate, message, otherData, url,
                                        docCategory, macId, batchName);
            }
            catch (Exception)
            {
                retryCount = retryCount + 1;
                if (retryCount <= maxRetry)
                {
                    apiResponse = CreateMetadata(appInfo, customPrefix, dateStringFormat, filePath, caseNumber, masNumber,
                                                            docType, scanDate, status, documentId, controlNumber, scanOperator, externalId,
                                                            internalId, source, fileType, fileSize, receivedDate, message, otherData, url,
                                                            docCategory, macId, batchName);
                }

            }

            return apiResponse;
        }

        /// <summary>
        /// Creates a oracle input agent metadata file with parameters provided
        /// </summary>
        /// <param name="appInfo">Image App information</param>
        /// <param name="customPrefix">App specific prefix to tied metadata file to a specific app</param>
        /// <param name="dateStringFormat">Format to save dates as string</param>
        /// <param name="filePath">Full name of file to be uploaded by Oracle Input Agent</param>
        /// <param name="caseNumber">Case number associated with the file</param>
        /// <param name="masNumber">MAS number associated with the file</param>
        /// <param name="docType">Doc type associated with the file</param>
        /// <param name="scanDate">Date file was scanned (or created)</param>
        /// <param name="status">Imaging status file will be uploaded under</param>
        /// <param name="documentId">Document Id associated with the file, if there is one</param>
        /// <param name="controlNumber">Control number associated with the file, if there is one</param>
        /// <param name="scanOperator">ID of user requesting the upload</param>
        /// <param name="externalId">External Id associated with the file, if there is one</param>
        /// <param name="internalId">Internal Id associated with the file, if there is one</param>
        /// <param name="source">CDR source to upload the file under</param>
        /// <param name="fileType">Type of file, optional</param>
        /// <param name="fileSize">Size of the file</param>
        /// <param name="receivedDate">Date file was received</param>
        /// <param name="message">Optional message to associate to document when uploading it</param>
        /// <param name="otherData">Optional data to associate to document when uploading it</param>
        /// <param name="url">Url associated with the file, if there is one</param>
        /// <param name="docCategory">CDR Doc Category to upload the file under</param>
        /// <param name="macId">MAC Id associated with the file, if there is one</param>
        /// <param name="batchName">CDR Batch Name to upload the file under</param>
        /// <param name="receivedTime">Time file was received</param>
        /// <returns>Service response indicating if service call was successful and with response messages, if any</returns>
        private ApiResponse CreateMetadata(
            ImageAppInfo appInfo,
            String customPrefix,
            String dateStringFormat,
            String filePath,
            String caseNumber,
            String masNumber,
            String docType,
            DateTime scanDate,
            String status,
            String documentId,
            String controlNumber,
            String scanOperator,
            String externalId,
            String internalId,
            String source,
            String fileType,
            String fileSize,
            DateTime receivedDate,
            String message,
            String otherData,
            String url,
            String docCategory,
            String macId,
            String batchName,
            TimeSpan? receivedTime = null)
        {
            var fileInfo = new FileInfo(filePath);

            var newFileName = fileInfo.Name;

            var line = appInfo.InputAgentFields;
            foreach (var fieldName in appInfo.InputAgentFields.Split(InputAgentFiledSeparator))
            {
                String fieldValue;
                switch (fieldName)
                {
                    case nameof(filePath):
                        //if (sendToAbbyyMode == SendToABBYYMode.Send)
                        //{
                        //    var pos = newFileName.LastIndexOf(fileInfo.Extension, StringComparison.OrdinalIgnoreCase);
                        //    fieldValue = newFileName.Remove(pos, fileInfo.Extension.Length).Insert(pos, PdfExt);
                        //}
                        //else
                        //{
                        //    fieldValue = newFileName;
                        //}
                        fieldValue = newFileName;
                        break;
                    case nameof(caseNumber):
                        fieldValue = caseNumber;
                        break;
                    case nameof(masNumber):
                        fieldValue = masNumber;
                        break;
                    case nameof(docType):
                        fieldValue = docType;
                        break;
                    case nameof(scanDate):
                        fieldValue = scanDate.ToString(dateStringFormat);
                        break;
                    case nameof(status):
                        fieldValue = status;
                        break;
                    case nameof(documentId):
                        fieldValue = documentId;
                        break;
                    case nameof(controlNumber):
                        fieldValue = controlNumber;
                        break;
                    case nameof(scanOperator):
                        fieldValue = scanOperator;
                        break;
                    case nameof(externalId):
                        fieldValue = externalId;
                        break;
                    case nameof(internalId):
                        fieldValue = internalId;
                        break;
                    case nameof(source):
                        fieldValue = source;
                        break;
                    case nameof(fileType):
                        fieldValue = fileType;
                        break;
                    case nameof(fileSize):
                        fieldValue = fileSize;
                        break;
                    case nameof(receivedDate):
                        fieldValue = receivedDate.ToString(dateStringFormat);
                        break;
                    case nameof(message):
                        fieldValue = message;
                        break;
                    case nameof(otherData):
                        fieldValue = otherData;
                        break;
                    case nameof(url):
                        fieldValue = url;
                        break;
                    case nameof(docCategory):
                        fieldValue = docCategory;
                        break;
                    case nameof(macId):
                        fieldValue = macId;
                        break;
                    case nameof(batchName):
                        fieldValue = batchName;
                        break;
                    case nameof(receivedTime):
                        fieldValue = receivedTime.HasValue ? receivedTime.Value.ToString("c") : string.Empty;
                        break;
                    default:
                        fieldValue = string.Empty;
                        break;
                }

                line = line.Replace(fieldName, fieldValue);

                string metadataFolder = "D:\\Freelance\\Harshitha\\MetaData";
                var metadataFileName = Path.Combine(metadataFolder, $"{appInfo.InputAgentPrefix}_{customPrefix}_{fileInfo.Name}{MetaDataFileExt}");
                if (System.IO.File.Exists(metadataFileName))
                    System.IO.File.Delete(metadataFileName);

                using (var sw = System.IO.File.CreateText(metadataFileName))
                {
                    sw.WriteLine(line);
                }

            }
            var result = new ApiResponse
            {
                Result = ApiResult.Success,
            };
            return result;
        }
    }
}
