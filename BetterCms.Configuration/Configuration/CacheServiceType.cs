﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CacheServiceType.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace BetterCms.Configuration
{
    /// <summary>
    /// BetterCMS cache service types.
    /// </summary>
    public enum CacheServiceType
    {
        /// <summary>
        /// Default cache service based on the server HttpRuntime cache. This is default.
        /// </summary>
        HttpRuntime = 0,

        /// <summary>
        /// Cache service is automatically picked by scanning installed modules for the caching service.
        /// </summary>
        Auto = 1,

        /// <summary>
        /// Custom storage provider
        /// </summary>
        Custom = 2
    }
}