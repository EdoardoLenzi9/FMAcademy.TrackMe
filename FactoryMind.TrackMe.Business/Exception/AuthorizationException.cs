
using System;

namespace FactoryMind.TrackMe.Business.Exceptions
{
    public class AuthorizationException : GeneralException
    {
        private string _errorDescription = "utente non possiede i permessi per eseguire questa operazione";
        public override string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
        }

        public AuthorizationException(string message) : base(message) { }

        public override string GetError()
        {
            return base.Message;
        }
    }
}