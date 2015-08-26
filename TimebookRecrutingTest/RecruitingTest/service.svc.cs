using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RecruitingTest
{
    using System.Collections.Concurrent;
    using System.Net;
    using System.ServiceModel.Web;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "service" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select service.svc or service.svc.cs at the Solution Explorer and start debugging.
    public class service : Iservice
    {
        #region Static Fields and Constants

        /// <summary>
        ///     The tokens.
        /// </summary>
        private static readonly ConcurrentDictionary<Guid, List<Tuple<DateTime, string>>> TokenBasedMessagesCollection =
            new ConcurrentDictionary<Guid, List<Tuple<DateTime, string>>>();

        #endregion

        #region Fields

        /// <summary>
        ///     The qoutes.
        /// </summary>
        private readonly List<string> qoutes =
            new List<string>(
                new[]
                    {
                        "Ready to work.", "Zug Zug.", "Dabu.", "Lok'tar.", "What?!", "Look out!", "Missed me!", "Hee hee hee! That tickles.", 
                        "I would not do such things if I were you.", "My tummy feels funny.", "'scuse me."
                    });

        #endregion

        #region Implementation of IRecrutingTest

        /// <summary>
        /// The get data using data contract.
        /// </summary>
        /// <param name="composite">
        /// The composite.
        /// </param>
        /// <returns>
        /// The <see cref="CompositeType"/>.
        /// </returns>
        public CompositeType PostMessage(CompositeType composite)
        {
            if (composite == null)
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            Guid guid;
            if (string.IsNullOrWhiteSpace(composite.Token) || !Guid.TryParse(composite.Token, out guid))
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            List<Tuple<DateTime, string>> messages;

            if (!TokenBasedMessagesCollection.TryGetValue(guid, out messages))
            {
                throw new WebFaultException(HttpStatusCode.NotFound);
            }

            messages.Add(new Tuple<DateTime, string>(DateTime.Now, composite.Message));
            composite.Message = this.qoutes[new Random().Next(this.qoutes.Count)];

            return composite;
        }

        /// <summary>
        /// The get messages.
        /// </summary>
        /// <param name="token">
        /// The token.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/>.
        /// </returns>
        public IEnumerable<CompositeType> GetMessages(string token)
        {
            Guid guid;

            if (string.IsNullOrWhiteSpace(token) || !Guid.TryParse(token, out guid))
            {
                throw new WebFaultException(HttpStatusCode.BadRequest);
            }

            List<Tuple<DateTime, string>> messages;
            if (TokenBasedMessagesCollection.TryGetValue(guid, out messages))
            {
                return messages.Select(n => new CompositeType { Message = n.Item2, Timestamp = n.Item1.ToLongTimeString() });
            }

            throw new WebFaultException(HttpStatusCode.NotFound);
        }

        /// <summary>
        ///     The get token.
        /// </summary>
        /// <returns>
        ///     The <see cref="CompositeType" />.
        /// </returns>
        public CompositeType GetToken()
        {
            var guid = Guid.NewGuid();
            TokenBasedMessagesCollection.TryAdd(guid, new List<Tuple<DateTime, string>>());
            return new CompositeType { Token = guid.ToString("D") };
        }

        #endregion
    }
}
