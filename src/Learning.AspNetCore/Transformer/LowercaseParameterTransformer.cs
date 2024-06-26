﻿namespace Learning.AspNetCore
{
    public class LowercaseParameterTransformer: IOutboundParameterTransformer
    {
        public string? TransformOutbound(object? value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString()?.ToLowerInvariant();
        }
    }
}
