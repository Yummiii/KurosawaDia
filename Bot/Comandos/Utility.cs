﻿using Bot.Extensions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;

namespace Bot.Comandos
{

    public class Utility : Weeb
    {
        private struct PossiveisMsg
        {
            public string identifier { get; private set; }
            public string msgDefault { get; private set; }

            public PossiveisMsg(string _identifier, string _msgDefault)
            {
                identifier = _identifier;
                msgDefault = _msgDefault;

            }
        }

        public void avatar(CommandContext context, object[] args)
        {
            string[] comando = (string[])args[1];
            string msg = string.Join(" ", comando, 1, (comando.Length - 1));


            if (!context.IsPrivate)
            {
                Tuple<IUser, string> getUser = new Extensions.UserExtensions().GetUser(context.Guild.GetUsersAsync().GetAwaiter().GetResult(), msg);
                if (getUser.Item1 != null || msg == "")
                {
                    IUser user;
                    if (msg != "")
                    {
                        user = getUser.Item1;
                    }
                    else
                    {
                        user = context.User;
                    }

                    string avatarUrl = user.GetAvatarUrl(0, 2048) ?? user.GetDefaultAvatarUrl();

                    PossiveisMsg[] msgs = { new PossiveisMsg("avatarMsgNice", "Nossa que avatar bonito, agora sei porque você queria ve-lo"), new PossiveisMsg("avatarMsgJoy", "Vocês são realmente criativos para avatares 😂"), new PossiveisMsg("avatarMsgIdol", "Com avatar assim seria um disperdicio não se tornar idol 😃"), new PossiveisMsg("avatarMsgFiltro", "Talvez se você posse um filtro ficaria melhor...") };
                    int rnd = new Random().Next(0, msgs.Length);

                    string msgfinal = StringCatch.GetString(msgs[rnd].identifier, msgs[rnd].msgDefault);

                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithTitle(msgfinal)
                        .WithDescription(StringCatch.GetString("avatarMsg", "\n\n{0}\n[Link Direto]({1})", user, avatarUrl))
                        .WithImageUrl(avatarUrl)
                    .Build());
                }
                else
                {
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithDescription(StringCatch.GetString("avatarErro", "**{0}** não encontrei essa pessoa", context.User.ToString()))
                            .AddField(StringCatch.GetString("usoCmd", "Uso do Comando: "), StringCatch.GetString("avatarUso", "`{0}avatar @pessoa`", (string)args[0]))
                            .AddField(StringCatch.GetString("exemploCmd", "Exemplo: "), StringCatch.GetString("exemloAvatar", "`{0}avatar @Hikari#3172`", (string)args[0]))
                            .WithColor(Color.Red)
                     .Build());
                }
            }
            else
            {
                if (msg == "")
                {
                    string avatar = context.User.GetAvatarUrl(0, 2048) ?? context.User.GetDefaultAvatarUrl();
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithAuthor($"{context.User}")
                        .WithDescription(StringCatch.GetString("avatarMsg", "[Link Direto]({0})", avatar))
                        .WithImageUrl(avatar)
                    .Build());
                }
                else
                {
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithDescription(StringCatch.GetString("avatarDm", "**{0}** desculpa mas eu não consigo pegar o avatar de outras pessoas no privado 😔", context.User.ToString()))
                            .WithColor(Color.Red)
                     .Build());
                }
            }
        }

        public void videochamada(CommandContext context, object[] args)
        {
            SocketGuildUser usr = context.User as SocketGuildUser;

            if (!context.IsPrivate && usr.VoiceChannel != null)
            {
                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithColor(Color.DarkPurple)
                        .WithDescription(StringCatch.GetString("videoChamada", "Para acessar o compartilhamento de tela basta [Clicar Aqui]({0}) 😀", $"https://discordapp.com/channels/{context.Guild.Id}/{usr.VoiceChannel.Id}"))
                .Build());
            }
            else
            {
                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithColor(Color.Red)
                        .WithDescription(StringCatch.GetString("videoChamadaDm", "você precisa estar em um canal de voz e em um servidor para usar esse comando"))
                .Build());
            }
        }

        public void emoji(CommandContext context, object[] args)
        {
            string[] comando = (string[])args[1];

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithColor(Color.DarkPurple);

            try
            {
                bool parse = Emote.TryParse(comando[1], out Emote emote);

                if (parse)
                {

                    embed.WithTitle(emote.Name);
                    embed.WithDescription(StringCatch.GetString("emoteLink", "[Link Direto]({0})", emote.Url));
                    embed.WithImageUrl(emote.Url);
                }
                else
                {
                    HttpExtensions http = new HttpExtensions();
                    string name = http.GetSiteHttp("https://ayura.com.br/links/emojis.json", comando[1]);

                    string shortName = "";
                    foreach (char character in name)
                    {
                        if (character != ':')
                        {
                            shortName += character;
                        }
                    }
                    string unicode = http.GetSite($"https://www.emojidex.com/api/v1/emoji/{shortName}", "unicode");

                    embed.WithTitle(shortName);
                    embed.WithDescription(StringCatch.GetString("emoteLinkUnicode", "[Link Direto]({0})", $"https://twemoji.maxcdn.com/2/72x72/{unicode}.png"));
                    embed.WithImageUrl($"https://twemoji.maxcdn.com/2/72x72/{unicode}.png");
                }
            }
            catch
            {
                embed.WithTitle(StringCatch.GetString("emoteInvalido", "Desculpe mas o emoji que você digitou é invalido"));
                embed.AddField(StringCatch.GetString("usoCmd", "Uso do comando: "), StringCatch.GetString("emoteUso", "`{0}emote emoji`", (string)args[0]));
                embed.AddField(StringCatch.GetString("exemploCmd", "Exemplo: "), StringCatch.GetString("emoteExeemplo", "`{0}emote :kanna:`", (string)args[0]));
                embed.WithColor(Color.Red);
            }

            context.Channel.SendMessageAsync(embed: embed.Build());
        }

        public void say(CommandContext context, object[] args)
        {
            if (!context.IsPrivate)
            {
                string[] comando = (string[])args[1];
                string msg = string.Join(" ", comando, 1, (comando.Length - 1));

                if (msg != "")
                {
                    IGuildUser user = context.User as IGuildUser;
                    if (user.GuildPermissions.ManageMessages)
                    {
                        context.Message.DeleteAsync().GetAwaiter().GetResult();
                    }

                    new EmbedControl().SendMessage(context.Channel, msg);
                }
                else
                {
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithDescription(StringCatch.GetString("sayErro", "**{0}** você precisa de me falar uma mensagem", context.User.ToString()))
                            .AddField(StringCatch.GetString("usoCmd", "Uso do comando:"), StringCatch.GetString("usoSay", "`{0}say <mensagem>`", (string)args[0]))
                            .AddField(StringCatch.GetString("exemploCmd", "Exemplo:"), StringCatch.GetString("ExemploSay", "`{0}say @Sora#5614 cade o wallpaper?`", (string)args[0]))
                            .WithColor(Color.Red)
                        .Build());
                }
            }
            else
            {
                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription(StringCatch.GetString("sayDm", "você so pode usar esse comando em servidores"))
                        .WithColor(Color.Red)
                    .Build());
            }
        }

        public void simg(CommandContext context, object[] args)
        {
            if (!context.IsPrivate)
            {
                if (context.Guild.IconUrl != null)
                {
                    string url = $"{context.Guild.IconUrl}?size=2048";
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                            .WithTitle(context.Guild.Name)
                            .WithDescription(StringCatch.GetString("simgTxt", "[Link Direto]({0})", url))
                            .WithImageUrl(url)
                            .WithColor(Color.DarkPurple)
                        .Build());
                }
                else
                {
                    context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription(StringCatch.GetString("simgIconErro", "**{0}** o servidor não tem um icone", context.User.ToString()))
                        .WithColor(Color.Red)
                    .Build()); ;
                }
            }
            else
            {
                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription(StringCatch.GetString("simgDm", "**{0}** esse comando so pode ser usado em servidores", context.User.ToString()))
                        .WithColor(Color.Red)
                    .Build());
            }
        }

        public void sugestao(CommandContext context, object[] args)
        {
            string[] comando = (string[])args[1];
            string msg = string.Join(" ", comando, 1, (comando.Length - 1));
            string servidor = "";

            if (!context.IsPrivate)
            {
                servidor = context.Guild.Name;
            }
            else
            {
                servidor = "Privado";
            }

            if (msg != "")
            {
                IMessageChannel canal = context.Client.GetChannelAsync(556598669500088320).GetAwaiter().GetResult() as IMessageChannel;

                canal.SendMessageAsync(embed: new EmbedBuilder()
                        .WithTitle($"Nova sugestão de: {context.User}")
                        .AddField("Sugestão: ", msg)
                        .AddField("Servidor: ", servidor)
                        .WithColor(Color.DarkPurple)
                    .Build());

                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription(StringCatch.GetString("sugestaoEnviada", "**{0}** eu sou muito grata por você me dar essa sugestão, vou usa-la para melhorar e te atender melhor ❤", context.User.ToString()))
                        .WithColor(Color.DarkPurple)
                    .Build());
            }
            else
            {
                context.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription(StringCatch.GetString("sugestaoFalar", "**{0}** você precisa me falara uma sugestão", context.User.ToString()))
                        .AddField(StringCatch.GetString("usoCmd", "Uso do Comando: "), StringCatch.GetString("usoSugestao", "`{0}sugestao <sugestão>`", (string)args[0]))
                        .AddField(StringCatch.GetString("exemploCmd", "Exemplo: "), StringCatch.GetString("exemploCmd", "`{0}sugestao fazer com que o bot ficasse mais tempo on`" ,(string)args[0]))
                        .WithColor(Color.Red)
                    .Build());
            }
        }

    }
}
