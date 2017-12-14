
using System;

namespace FactoryMind.TrackMe.Business.Exceptions
{
    public class RepositoryException : GeneralException
    {
        private string _errorDescription = "Errore interrogazione Repository";
        public override string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
        }

        public RepositoryException(string message) : base(message) { }

        public override string GetError()
        {
            return base.Message;
        }
    }
}