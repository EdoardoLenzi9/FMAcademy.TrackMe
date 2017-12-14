
using System;

namespace FactoryMind.TrackMe.Business.Exceptions
{
    public class NotFoundException : GeneralException
    {
        private string _errorDescription = "Elemento non presente in database";
        public override string ErrorDescription
        {
            get
            {
                return _errorDescription;
            }
        }

        public NotFoundException(string message) : base(message) { }

        public override string GetError()
        {
            return base.Message;
        }
    }
}