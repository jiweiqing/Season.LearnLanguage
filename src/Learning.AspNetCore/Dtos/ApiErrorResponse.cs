﻿namespace Learning.AspNetCore
{
    public class ApiErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ValidationErrorInfo[]? ValidationErrors { get; set; }
    }
}
