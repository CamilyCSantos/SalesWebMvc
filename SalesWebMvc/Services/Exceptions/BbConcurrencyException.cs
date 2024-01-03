namespace SalesWebMvc.Services.Exceptions
{
    public class BbConcurrencyException : ApplicationException
    {
        public BbConcurrencyException(string message ) : base( message ) 
        {
        }
    }
}
