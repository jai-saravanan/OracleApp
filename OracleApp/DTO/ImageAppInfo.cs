using System.Runtime.Serialization;

namespace OracleApp.DTO
{
    public class ImageAppInfo
    {
        /// <summary>
        /// Name of the Image App
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Input Agent Prefix for metadata files for the image app
        /// </summary>
        public String InputAgentPrefix { get; set; }

        /// <summary>
        /// Fields required by Input Agent for the image app
        /// </summary>
        public String InputAgentFields { get; set; }

        /// <summary>
        /// List of Security objects for the image app
        /// </summary>
        public List<ImageSecurityInfo> SecurityList { get; set; }

        /// <summary>
        /// List of document types for the image app
        /// </summary>
        public List<String> DocumentTypes { get; set; }
    }

    public class ImageSecurityInfo
    {
        public String AdGroupName { get; set; }

        public Boolean CanViewImage { get; set; }

        public Boolean CanWriteImage { get; set; }

        public Boolean CanDeleteImage { get; set; }
    }
}
