using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RecruitingTest
{
    using System.ServiceModel.Web;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "Iservice" in both code and config file together.
    [ServiceContract]
    public interface Iservice
    {
        #region Public Methods

        /// <summary>
        /// The get messages.
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "messages?token={token}", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<CompositeType> GetMessages(string token);

        /// <summary>
        ///     The get token.
        /// </summary>
        /// <returns>
        ///     The <see cref="CompositeType" />.
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "token", Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompositeType GetToken();

        /// <summary>
        /// The get data using data contract.
        /// </summary>
        /// <param name="composite">
        /// The composite.
        /// </param>
        /// <returns>
        /// The <see cref="CompositeType"/>.
        /// </returns>
        [OperationContract]
        [WebInvoke(UriTemplate = "submit", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompositeType PostMessage(CompositeType composite);

        #endregion
    }

    /// <summary>
    ///     The composite type.
    /// </summary>
    [DataContract]
    public class CompositeType
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the string value.
        /// </summary>
        [DataMember(Name = "message", IsRequired = false, EmitDefaultValue = false)]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        [DataMember(Name = "timestamp", IsRequired = false, EmitDefaultValue = false)]
        public string Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the token.
        /// </summary>
        [DataMember(Name = "token", IsRequired = false, EmitDefaultValue = false)]
        public string Token { get; set; }

        #endregion
    }
}
