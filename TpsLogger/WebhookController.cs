// -----------------------------------------------------------------------
// <copyright file="WebhookController.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger
{
    using System;
    using DSharp4Webhook.Core;
    using DSharp4Webhook.Core.Constructor;
    using Exiled.API.Features;

    /// <summary>
    /// Handles the sending of messages to a discord channel via a webhook.
    /// </summary>
    public class WebhookController : IDisposable
    {
        private static readonly EmbedBuilder EmbedBuilder = ConstructorProvider.GetEmbedBuilder();
        private static readonly EmbedFieldBuilder FieldBuilder = ConstructorProvider.GetEmbedFieldBuilder();
        private static readonly MessageBuilder MessageBuilder = ConstructorProvider.GetMessageBuilder();
        private readonly Plugin plugin;
        private readonly IWebhook webhook;
        private bool isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebhookController"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public WebhookController(Plugin plugin)
        {
            this.plugin = plugin;
            webhook = WebhookProvider.CreateStaticWebhook(plugin.Config.Webhook.Url);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            isDisposed = true;
            webhook?.Dispose();
        }

        /// <summary>
        /// Sends the tps to the webhook.
        /// </summary>
        public void SendTps()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            MessageBuilder messageBuilder = PrepareMessage();
            if (messageBuilder is null)
                return;

            webhook.SendMessage(messageBuilder.Build()).Queue((_, isSuccessful) =>
            {
                if (!isSuccessful)
                    Log.Warn("Failed to send the tps webhook.");
            });
        }

        private static string Codeline(object toFormat) => $"```{toFormat}```";

        private MessageBuilder PrepareMessage()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(WebhookController));

            EmbedBuilder.Reset();
            FieldBuilder.Reset();
            MessageBuilder.Reset();

            FieldBuilder.Inline = false;

            FieldBuilder.Name = plugin.Config.Webhook.Players ?? "Players";
            FieldBuilder.Value = Codeline($"{Server.PlayerCount}/{Server.MaxPlayerCount}");
            EmbedBuilder.AddField(FieldBuilder.Build());

            FieldBuilder.Name = plugin.Config.Webhook.Tps ?? "TPS";
            FieldBuilder.Value = Codeline(Server.Tps);
            EmbedBuilder.AddField(FieldBuilder.Build());

            if (!string.IsNullOrEmpty(plugin.Config.Webhook.Header))
                EmbedBuilder.Title = plugin.Config.Webhook.Header;

            if (!string.IsNullOrEmpty(plugin.Config.Webhook.Color))
                EmbedBuilder.Color = (uint)DSharp4Webhook.Util.ColorUtil.FromHex(plugin.Config.Webhook.Color);

            EmbedBuilder.Timestamp = DateTimeOffset.UtcNow;
            MessageBuilder.AddEmbed(EmbedBuilder.Build());

            return MessageBuilder;
        }
    }
}