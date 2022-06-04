// -----------------------------------------------------------------------
// <copyright file="CurrentCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Commands
{
    using System;
    using System.ComponentModel;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;

    /// <inheritdoc />
    public class CurrentCommand : ICommand
    {
        /// <inheritdoc />
        public string Command { get; set; } = "current";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "c" };

        /// <inheritdoc />
        public string Description { get; set; } = "Gets the current tps";

        /// <summary>
        /// Gets or sets the response to send to the player.
        /// </summary>
        [Description("The response to send to the player.")]
        public string Response { get; set; } = "Current TPS: {0}";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "tps.current";

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

            response = string.Format(Response, Server.Tps);
            return true;
        }
    }
}