using System;

namespace FactoryMind.TrackMe.Business.Exceptions
{
    public class GeneralException : Exception
    {
        private string _errorDescription = "Errore Generale";
        public virtual string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
        }

        public GeneralException(string message) : base(message) { }

        public virtual string GetError()
        {
            return base.Message;
        }
    }
}