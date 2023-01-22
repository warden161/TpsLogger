// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger
{
    using Exiled.API.Interfaces;
    using TpsLogger.Configs;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <inheritdoc/>
        public bool Debug { get; set; }

        /// <summary>
        /// Gets or sets the command configs.
        /// </summary>
        public CommandsConfig Commands { get; set; } = new();

        /// <summary>
        /// Gets or sets the webhook configs.
        /// </summary>
        public WebhookConfig Webhook { get; set; } = new();
    }
}