namespace CReview.Models
{

    public class FunctionSignature
    {
        public string FunctionName { get; set; }
        public string ReturnType { get; set; }
        public FunctionArgument[] Arguments { get; set; }
    }
}
