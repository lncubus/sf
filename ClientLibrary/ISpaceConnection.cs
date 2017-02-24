using System;
using System.ServiceModel;

namespace ClientLibrary
{
    /// <summary>
    /// Server contract.
    /// </summary>
    [ServiceContract(
        SessionMode=SessionMode.Required,
        ProtectionLevel=System.Net.Security.ProtectionLevel.None
    )]
    public interface ISpaceConnection
    {
        /// <summary>
        /// Connect as non-autheticated user.
        /// Returns session specific nounce to the client.
        /// </summary>
        [OperationContract(IsInitiating = true, IsTerminating = false)]
        string Connect();

        /// <summary>
        /// Login with specified response.
        /// </summary>
        /// <param name="response">Response.</param>
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        bool Login(string response);

        /// <summary>
        /// Terminates the session.
        /// </summary>
        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = true)]
        void Logout();

        /// <summary>
        /// Dummy procedure.
        /// </summary>
        /// <returns>Integer counter.</returns>
        [OperationContract(IsInitiating = false, IsTerminating = false)]
        int Ping();
    }
}

