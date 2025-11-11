namespace MyMusicBook.Exceptions.ExceptionsBase;
public class ErrorOnValidationException : MyMusicBookException
{
    public IList<string> ErrorMessages { get; set; }
    
    public ErrorOnValidationException(IList<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }
}
