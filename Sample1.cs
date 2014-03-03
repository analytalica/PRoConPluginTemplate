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
    public class Sample1 : PRoConPluginAPI, IPRoConPluginInterface
    {
        private bool pluginEnabled = false;
        private String someString = "string";

        private String debugLevelString = "1";
        private int debugLevel = 1;

        public Sample1()
        {

        }

        public string GetPluginName()
        {
            return "Sample1.cs : Plugin Template";
        }

        public string GetPluginVersion()
        {
            return "0.0.0";
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
            return @"A basic plugin template. Does nothing. This version has no comments in the code.";
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
            this.RegisterEvents(this.GetType().Name, "OnPluginLoaded");
        }

        public void OnPluginEnable()
        {
            this.pluginEnabled = true;
            this.toConsole(1, "Template Plugin Enabled!");
        }
		
        public void OnPluginDisable()
        {
            this.pluginEnabled = false;
            this.toConsole(1, "Template Plugin Disabled!");
        }

        public List<CPluginVariable> GetDisplayPluginVariables()
        {
            List<CPluginVariable> lstReturn = new List<CPluginVariable>();
            lstReturn.Add(new CPluginVariable("Settings|Some String", typeof(string), someString));
            lstReturn.Add(new CPluginVariable("Settings|Debug Level", typeof(string), debugLevelString));
            return lstReturn;
        }

        public List<CPluginVariable> GetPluginVariables()
        {
            return GetDisplayPluginVariables();
        }

        public void SetPluginVariable(String strVariable, String strValue)
        {
            if (Regex.Match(strVariable, @"Some String").Success)
            {
                someString = strValue;
            }
            else if (Regex.Match(strVariable, @"Debug Level").Success)
            {
                debugLevelString = strValue;
                try
                {
                    debugLevel = Int32.Parse(debugLevelString);
                }
                catch (Exception z)
                {
                    toConsole(1, "Invalid debug level! Choose 0, 1, or 2 only.");
                    debugLevel = 1;
                    debugLevelString = "1";
                }
            }
        }
    }
}