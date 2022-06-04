// -----------------------------------------------------------------------
// <copyright file="LogCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MEC;

    /// <inheritdoc />
    public class LogCommand : ICommand
    {
        private CoroutineHandle logCoroutine;

        /// <inheritdoc />
        public string Command { get; set; } = "log";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "l" };

        /// <inheritdoc />
        public string Description { get; set; } = "Toggles logging tps to the server console.";

        /// <summary>
        /// Gets or sets the interval, in seconds, to log the tps.
        /// </summary>
        [Description("The interval, in seconds, to log the tps.")]
        public float Interval { get; set; } = 1f;

        /// <summary>
        /// Gets or sets a value indicating whether logs will be sent while the server is in idle mode.
        /// </summary>
        [Description("Whether logs will be sent while the server is in idle mode.")]
        public bool LogIdle { get; set; } = false;

        /// <summary>
        /// Gets or sets the message to log.
        /// </summary>
        [Description("The message to log.")]
        public string ToLog { get; set; } = "Current TPS: {0}";

        /// <summary>
        /// Gets or sets the response to send when the tps logging is disabled.
        /// </summary>
        [Description("The response to send when the tps logging is disabled.")]
        public string LoggingDisabled { get; set; } = "TPS logging disabled.";

        /// <summary>
        /// Gets or sets the response to send when the tps logging is enabled.
        /// </summary>
        [Description("The response to send when the tps logging is enabled.")]
        public string LoggingEnabled { get; set; } = "TPS logging enabled.";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "tps.log";

        /// <summary>
        /// Gets or sets the response to send to a player when they lack permission to execute the command.
        /// </summary>
        [Description("The response to send to a player when they lack permission to execute the command.")]
        public string InsufficientPermission { get; set; } = "You do not have permission to use this command.";

        /// <inheritdoc />
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(RequiredPermission))
            {
                response = InsufficientPermission;
                return false;
            }

            if (logCoroutine.IsRunning)
            {
                Timing.KillCoroutines(logCoroutine);
                response = LoggingDisabled;
                return true;
            }

            logCoroutine = Timing.RunCoroutine(RunLog());
            response = LoggingEnabled;
            return true;
        }

        private IEnumerator<float> RunLog()
        {
            while (true)
            {
                if (!LogIdle)
                    yield return Timing.WaitUntilTrue(() => !IdleMode.IdleModeActive);

                yield return Timing.WaitForSeconds(Interval);
                Log.Debug(string.Format(ToLog, Server.Tps));
            }
        }
    }
}