// Note: This file was originally generated by Genie

//===============================================================================
//
// IErrorLogData
//
// PURPOSE: 
// Data abstraction interface for ErrorLog.
//
// NOTES: 
// 
//
//===============================================================================
//
// Copyright (C) 2008 
// All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED 'AS IS' WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Agile.Framework;
// Required for DateTime

namespace Agile.Mobile.Web.Logging
{
    /// <summary>
    /// Data abstraction interface for ErrorLog.
    /// </summary>
    public interface IErrorLogData : IModelInterface
    {

        /// <summary>
        /// Gets the ErrorLogId.
        /// </summary>
        long ErrorLogId { get; set; }

        /// <summary>
        /// Gets the ErrorType.
        /// </summary>
        string ErrorType { get; set; }

        /// <summary>
        /// Gets the Message.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Gets the PersonId.
        /// </summary>
        int? PersonId { get; set; }

        /// <summary>
        /// Gets the Username.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Gets the AppVersion.
        /// </summary>
        string AppVersion { get; set; }

        /// <summary>
        /// Gets the OS.
        /// </summary>
        string OS { get; set; }

        /// <summary>
        /// Gets the OSVersion.
        /// </summary>
        string OSVersion { get; set; }

        /// <summary>
        /// Gets the Device.
        /// </summary>
        string Device { get; set; }

        /// <summary>
        /// Gets the Country.
        /// </summary>
        string Country { get; set; }

        /// <summary>
        /// Gets the Language.
        /// </summary>
        string Language { get; set; }

        /// <summary>
        /// Gets the CreatedUtc.
        /// </summary>
        DateTimeOffset CreatedUtc { get; set; }

        /// <summary>
        /// Gets the UpdatedUtc.
        /// </summary>
        DateTimeOffset? UpdatedUtc { get; set; }

    }


}
