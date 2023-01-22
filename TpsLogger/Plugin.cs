// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger
{
    using System;
    using System.Collections.Generic;
    using Exiled.API.Features;
    using MEC;
    using RemoteAdmin;
    using TpsLogger.Commands;

    /// <inheritdoc />
    public class Plugin : Plugin<Config>
    {
        private CoroutineHandle webhookCoroutine;
        private ParentCommand parentCommand;

        /// <summary>
        /// Gets an instance of the logger class.
        /// </summary>
        public WebhookController WebhookController { get; private set; }

        /// <inheritdoc/>
        public override string Author => "Build (updated by warden161)";

        /// <inheritdoc/>
        public override string Name => "TpsLogger";

        /// <inheritdoc/>
        public override string Prefix => "TpsLogger";

        /// <inheritdoc/>
        public override Version Version { get; } = new(1, 1, 0);

        /// <inheritdoc/>
        public override Version RequiredExiledVersion { get; } = new(6, 0, 0);

        /// <inheritdoc/>
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnReloadedConfigs;
            if (Config.Webhook.IsEnabled && !string.IsNullOrEmpty(Config.Webhook.Url))
            {
                WebhookController = new WebhookController(this);
                webhookCoroutine = Timing.RunCoroutine(RunWebhook());
            }

            base.OnEnabled();
        }

        /// <inheritdoc/>
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnReloadedConfigs;
            if (webhookCoroutine.IsRunning)
                Timing.KillCoroutines(webhookCoroutine);

            WebhookController = null;
            base.OnDisabled();
        }

        /// <inheritdoc />
        public override void OnRegisteringCommands()
        {
            parentCommand = new TpsParentCommand();
            Config.Commands.RegisterTo(parentCommand);
            CommandProcessor.RemoteAdminCommandHandler.RegisterCommand(parentCommand);
            GameCore.Console.singleton.ConsoleCommandHandler.RegisterCommand(parentCommand);
        }

        /// <inheritdoc />
        public override void OnUnregisteringCommands()
        {
            CommandProcessor.RemoteAdminCommandHandler.UnregisterCommand(parentCommand);
            GameCore.Console.singleton.ConsoleCommandHandler.UnregisterCommand(parentCommand);
            parentCommand = null;
        }

        private void OnReloadedConfigs()
        {
            OnUnregisteringCommands();
            OnRegisteringCommands();
        }

        private IEnumerator<float> RunWebhook()
        {
            while (true)
            {
                if (!Config.Webhook.LogIdle)
                    yield return Timing.WaitUntilTrue(() => !IdleMode.IdleModeActive);

                yield return Timing.WaitForSeconds(Config.Webhook.Interval);
                WebhookController.SendTps();
            }
        }
    }
}