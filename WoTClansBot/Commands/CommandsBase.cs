﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;

namespace Negri.Wot.Bot
{
    /// <summary>
    /// Base class for Commands
    /// </summary>
    public abstract class CommandsBase
    {
        /// <summary>
        ///     Regex for clan tags
        /// </summary>
        protected static readonly Regex ClanTagRegex = new Regex("^[A-Z0-9\\-_]{2,5}$", RegexOptions.Compiled);

        protected async Task<bool> CanExecute(CommandContext ctx, string feature)
        {
            var cfg = GuildConfiguration.FromGuild(ctx.Guild);
            if (!cfg.CanCallerExecute(feature, ctx.Member, ctx.Channel, out var reason))
            {
                if (!cfg.SilentDeny)
                {
                    var emoji = DiscordEmoji.FromName(ctx.Client, ":no_entry:");
                    var embed = new DiscordEmbedBuilder
                    {
                        Title = "Access denied",
                        Description = $"{emoji} {reason}",
                        Color = DiscordColor.Red
                    };

                    await ctx.RespondAsync("", embed: embed);
                }
                
                return await Task.Run(() => false);
            }

            return await Task.Run(() => true);
        }

        protected Plataform GetPlataform(string s, Plataform defaultPlataform, out string clean)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                clean = string.Empty;
                return defaultPlataform;
            }
            
            s = s.Trim('\"', ' ', '\t', '\r', '\n', '\'', '`', '´');
            s = s.RemoveDiacritics().ToLowerInvariant();

            if (s.StartsWith("ps."))
            {
                clean = s.Substring(3);
                return Plataform.PS;                
            }
            else if (s.StartsWith("ps4."))
            {
                clean = s.Substring(4);
                return Plataform.PS;                
            }
            else if (s.StartsWith("xbox."))
            {
                clean = s.Substring(5);
                return Plataform.XBOX;                
            }
            else if (s.StartsWith("x."))
            {
                clean = s.Substring(2);
                return Plataform.XBOX;                
            }
            else if (s.StartsWith("p."))
            {
                clean = s.Substring(2);
                return Plataform.PS;                
            }

            clean = s;
            return defaultPlataform;
        }
    }
}