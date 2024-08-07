using System.Runtime.Serialization;

namespace OracleApp.DTO
{
    public class ApiResponse
    {
        /// <summary>
        /// List of messages for the response
        /// </summary>
        public List<String> Messages { get; set; }

        /// <summary>
        /// Service result
        /// </summary>
        public ApiResult Result { get; set; }
    }

    public enum ApiResult
    {
        Unknown = 0,
        Success = 1,
        Failed = 2,
        Warnings = 3
    }
}
