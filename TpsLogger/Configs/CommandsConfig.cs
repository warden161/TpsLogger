// -----------------------------------------------------------------------
// <copyright file="CommandsConfig.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TpsLogger.Configs
{
    using TpsLogger.Commands;

    /// <summary>
    /// Handles the configs for the commands.
    /// </summary>
    public class CommandsConfig
    {
        /// <summary>
        /// Gets or sets an instance of the <see cref="Commands.CurrentCommand"/> class.
        /// </summary>
        public CurrentCommand Current { get; set; } = new();

        /// <summary>
        /// Gets or sets an instance of the <see cref="Commands.DisplayCommand"/> class.
        /// </summary>
        public DisplayCommand Display { get; set; } = new();

        /// <summary>
        /// Gets or sets an instance of the <see cref="Commands.LogCommand"/> class.
        /// </summary>
        public LogCommand Log { get; set; } = new();

        /// <summary>
        /// Registers all of the commands to the specified parent command.
        /// </summary>
        /// <param name="parentCommand">The parent command to register the commands to.</param>
        public void RegisterTo(ParentCommand parentCommand)
        {
            parentCommand.RegisterCommand(Current ?? new CurrentCommand());
            parentCommand.RegisterCommand(Display ?? new DisplayCommand());
            parentCommand.RegisterCommand(Log ?? new LogCommand());
        }
    }
}