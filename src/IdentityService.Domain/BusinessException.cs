namespace IdentityService.Domain
{
    public class BusinessException: Exception
    {
        public BusinessException()
        {
        }

        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; set; } = string.Empty;
    }
}
