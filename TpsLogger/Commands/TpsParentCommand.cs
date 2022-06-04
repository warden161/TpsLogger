// -----------------------------------------------------------------------
// <copyright file="TpsParentCommand.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Commands
{
    using System;
    using System.Text;
    using CommandSystem;
    using NorthwoodLib.Pools;

    /// <inheritdoc />
    public class TpsParentCommand : ParentCommand
    {
        /// <inheritdoc />
        public override string Command => "tps";

        /// <inheritdoc />
        public override string[] Aliases { get; } = Array.Empty<string>();

        /// <inheritdoc />
        public override string Description => "Commands related to tps monitoring";

        /// <inheritdoc />
        public override void LoadGeneratedCommands()
        {
        }

        /// <inheritdoc/>
        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();
            stringBuilder.AppendLine("Please enter a valid subcommand! Available:");
            foreach (ICommand command in AllCommands)
            {
                stringBuilder.AppendLine(command.Aliases is { Length: > 0 }
                    ? $"{command.Command} | Aliases: {string.Join(", ", command.Aliases)}"
                    : command.Command);
            }

            response = StringBuilderPool.Shared.ToStringReturn(stringBuilder).TrimEnd();
            return false;
        }
    }
}