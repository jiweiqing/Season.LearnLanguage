namespace Learning.AspNetCore
{
    public class ValidationErrorInfo
    {
        /// <summary>
        /// Relate invalid member (field/property).
        /// </summary>
        public string Member { get; set; } = string.Empty;

        /// <summary>
        /// Validation error messages.
        /// </summary>
        public string[]? Messages { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        public ValidationErrorInfo()
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="member">Validation error message</param>
        public ValidationErrorInfo(string member)
        {
            Member = member;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="member">Related invalid member</param>
        /// <param name="messages">Validation error messages</param>
        public ValidationErrorInfo(string member, string[] messages)
            : this(member)
        {
            Messages = messages;
        }

        /// <summary>
        /// Creates a new instance of <see cref="ValidationErrorInfo"/>.
        /// </summary>
        /// <param name="member">Related invalid member</param>
        /// <param name="message">Validation error messages</param>
        public ValidationErrorInfo(string member, string message)
            : this(member, new[] { message })
        {
        }
    }
}
