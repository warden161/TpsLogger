// -----------------------------------------------------------------------
// <copyright file="DisplayCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using CommandSystem;
    using Exiled.API.Features;
    using Exiled.Permissions.Extensions;
    using MEC;
    using NorthwoodLib.Pools;

    /// <inheritdoc />
    public class DisplayCommand : ICommand
    {
        private readonly Dictionary<Player, CoroutineHandle> displayCoroutines = new();

        /// <inheritdoc />
        public string Command { get; set; } = "display";

        /// <inheritdoc />
        public string[] Aliases { get; set; } = { "d" };

        /// <inheritdoc />
        public string Description { get; set; } = "Toggles a tps display";

        /// <summary>
        /// Gets or sets the vertical position to display the text.
        /// </summary>
        [Description("The vertical position to display the text.")]
        public uint VerticalOffset { get; set; } = 0;

        /// <summary>
        /// Gets or sets the message to display when a player has the tps display activated.
        /// </summary>
        [Description("The message to display when a player has the tps display activated.")]
        public string TpsDisplay { get; set; } = "<align=right>TPS: {0}</align>";

        /// <summary>
        /// Gets or sets the response to send when the tps display is disabled.
        /// </summary>
        [Description("The response to send when the tps display is disabled.")]
        public string DisplayDisabled { get; set; } = "TPS display disabled.";

        /// <summary>
        /// Gets or sets the response to send when the tps display is enabled.
        /// </summary>
        [Description("The response to send when the tps display is enabled.")]
        public string DisplayEnabled { get; set; } = "TPS display enabled.";

        /// <summary>
        /// Gets or sets the permission required to use this command.
        /// </summary>
        [Description("The permission required to use this command.")]
        public string RequiredPermission { get; set; } = "tps.display";

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

            if (Player.Get(sender) is not Player player)
            {
                response = "You must be a player to use this command.";
                return false;
            }

            if (displayCoroutines.TryGetValue(player, out CoroutineHandle coroutineHandle))
            {
                Timing.KillCoroutines(coroutineHandle);
                displayCoroutines.Remove(player);
                response = DisplayDisabled;
                return true;
            }

            displayCoroutines.Add(player, Timing.RunCoroutine(RunDisplay(player)));
            response = DisplayEnabled;
            return true;
        }

        private static string NewLineFormatter(uint lineNumber)
        {
            StringBuilder lineBuilder = StringBuilderPool.Shared.Rent();
            for (int i = 32; i > lineNumber; i--)
                lineBuilder.AppendLine();

            return StringBuilderPool.Shared.ToStringReturn(lineBuilder);
        }

        private string FormatMessage()
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine(TpsDisplay);
            stringBuilder.AppendLine(NewLineFormatter(VerticalOffset));
            return StringBuilderPool.Shared.ToStringReturn(stringBuilder);
        }

        private IEnumerator<float> RunDisplay(Player player)
        {
            while (player.IsConnected)
            {
                player.ShowHint(string.Format(FormatMessage(), Server.Tps), 1.1f);
                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}