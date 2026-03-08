namespace Shared.Error_Models
{
    public class ValidationError
    {
        public string Feild { get; set; }
        public IEnumerable<string>Erroes { get; set; }
    }
}