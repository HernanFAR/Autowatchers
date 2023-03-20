﻿// <auto-generated/>
#pragma warning disable
#nullable enable annotations

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace System.Diagnostics.CodeAnalysis
{
    /// <summary>
    /// Specifies that the output will be non-null if the named parameter is non-null.
    /// </summary>
    [global::System.AttributeUsage(
        global::System.AttributeTargets.Parameter |
        global::System.AttributeTargets.Property |
        global::System.AttributeTargets.ReturnValue,
        AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : global::System.Attribute
    {
        /// <summary>
        /// Initializes the attribute with the associated parameter name.
        /// </summary>
        /// <param name="parameterName">The associated parameter name. The output will be non-null if the argument to the parameter specified is non-null.</param>
        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Gets the associated parameter name.
        /// </summary>
        public string ParameterName { get; }
    }
}