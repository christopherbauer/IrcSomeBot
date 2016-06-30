# IrcSomeBot
Short and sweet irc bot that loads "responders" that drive its posting behavior.

##ToDo

Right now the bot may private message a user then fail to respond to requests for the same ticker in the channels it supports.

The bot currently cannot operate in multiple channels simultaneously.

The only way for the bot to maintain state over multiple messages is to hack the source up (might need to rewrite the responders for this).