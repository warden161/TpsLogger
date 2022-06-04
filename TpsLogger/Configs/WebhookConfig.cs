// -----------------------------------------------------------------------
// <copyright file="WebhookConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Configs
{
    using System.ComponentModel;

    /// <summary>
    /// Handles configs related to the webhook.
    /// </summary>
    public class WebhookConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether the webhook will be used.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the webhook url.
        /// </summary>
        [Description("The webhook url.")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the interval, in seconds, to log the tps.
        /// </summary>
        [Description("The interval, in seconds, to log the tps.")]
        public float Interval { get; set; } = 60f;

        /// <summary>
        /// Gets or sets a value indicating whether webhooks will be sent while the server is in idle mode.
        /// </summary>
        [Description("Whether webhooks will be sent while the server is in idle mode.")]
        public bool LogIdle { get; set; } = false;

        /// <summary>
        /// Gets or sets the embed header.
        /// </summary>
        [Description("The embed header.")]
        public string Header { get; set; } = "TPS Logger";

        /// <summary>
        /// Gets or sets the color of the embed.
        /// </summary>
        [Description("The color of the embed.")]
        public string Color { get; set; } = "#808080";

        /// <summary>
        /// Gets or sets the literal translation for 'players'.
        /// </summary>
        [Description("The literal translation for 'players'.")]
        public string Players { get; set; } = "Players";

        /// <summary>
        /// Gets or sets the literal translation for 'TPS'.
        /// </summary>
        [Description("The literal translation for 'TPS'.")]
        public string Tps { get; set; } = "TPS";
    }
}