
using System;

namespace FactoryMind.TrackMe.Business.Exceptions
{
    public class ParameterException : GeneralException
    {
        private string _errorDescription = "Parametri della funzione errati";
        public override string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
        }

        public ParameterException(string message) : base(message) { }

        public override string GetError()
        {
            return base.Message;
        }
    }
}