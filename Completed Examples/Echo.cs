using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Players;

namespace PRoConEvents
{
    public class Echo : PRoConPluginAPI, IPRoConPluginInterface
    {
        private bool pluginEnabled = false;

        private int debugLevel = 1;

        public Echo()
        {

        }

        public string GetPluginName()
        {
            return "Echo Plugin";
        }

        public string GetPluginVersion()
        {
            return "1.0.2";
        }

        public string GetPluginAuthor()
        {
            return "Analytalica";
        }

        public string GetPluginWebsite()
        {
            return "purebattlefield.org";
        }

        public string GetPluginDescription()
        {
            return @"Echos chat messages back to players.";
        }

        public void toChat(String message)
        {
            if (!message.Contains("\n") && !String.IsNullOrEmpty(message))
            {
                this.ExecuteCommand("procon.protected.send", "admin.say", message, "all");
            }
            else if(message != "\n")
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
					if(!String.IsNullOrEmpty(message))
                    toChat(send);
                }
            }
        }

        public void toChat(String message, String playerName)
        {
            if (!message.Contains("\n") && !String.IsNullOrEmpty(message))
            {
                this.ExecuteCommand("procon.protected.send", "admin.say", message, "player", playerName);
            }
            else if(message != "\n")
            {
                string[] multiMsg = message.Split(new string[] { "\n" }, StringSplitOptions.None);
                foreach (string send in multiMsg)
                {
					if(!String.IsNullOrEmpty(message))
                    toChat(send, playerName);
                }
            }
        }

        public void toConsole(int msgLevel, String message)
        {
            if (debugLevel >= msgLevel)
            {
                this.ExecuteCommand("procon.protected.pluginconsole.write", message);
            }
        }

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion)
        {
            this.RegisterEvents(this.GetType().Name, "OnPluginLoaded", "OnGlobalChat", "OnPlayerChat", "OnSquadChat");
        }

        public void OnPluginEnable()
        {
            this.pluginEnabled = true;
            this.toConsole(1, "Echo Plugin Enabled!");
        }
		
        public void OnPluginDisable()
        {
            this.pluginEnabled = false;
            this.toConsole(1, "Echo Disabled!");
        }
		
		public override void OnGlobalChat(string speaker, string message) { 
			echoToPlayer(message, speaker); 
		}
		public override void OnTeamChat(string speaker, string message, int teamId) { 
			echoToPlayer(message, speaker); 
		}
		public override void OnSquadChat(string speaker, string message, int teamId, int squadId) { 
			echoToPlayer(message, speaker); 
		}
		public void echoToPlayer(string message, string playerName){
			if(pluginEnabled && playerName != "Server")
				this.ExecuteCommand("procon.protected.send", "admin.say", message, "player", playerName);
		}

        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Debug Level", typeof(string), debugLevel.ToString()));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, @"Debug Level").Success)
            {
                try
                {
                    debugLevel = Int32.Parse(strValue);
                }
                catch (Exception z)
                {
                    toConsole(1, "Invalid debug level! Choose 0, 1, or 2 only.");
                    debugLevel = 1;
                }
            }
        }
    }
}