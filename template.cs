//Import various C# things.
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

//Import Procon things.
using PRoCon.Core;
using PRoCon.Core.Plugin;
using PRoCon.Core.Players;

namespace PRoConEvents {
    public class template : PRoConPluginAPI, IPRoConPluginInterface {

        //--------------------------------------
        //Class level variables.
        //--------------------------------------

        private bool pluginEnabled = false;

        //--------------------------------------
        //Plugin constructor. Can be left blank.
        //--------------------------------------

        public template() {

        }

        //--------------------------------------
        //Description settings for your plugin.
        //--------------------------------------

        public string GetPluginName() {
            return "Plugin Template";
        }

        public string GetPluginVersion() {
            return "0.0.0";
        }

        public string GetPluginAuthor() {
            return "Analytalica";
        }

        public string GetPluginWebsite() {
            return "purebattlefield.org";
        }

        public string GetPluginDescription() {
            return @"A basic plugin template. Does nothing.";
        }

        //--------------------------------------
        //These methods run when Procon does what's on the label.
        //--------------------------------------

        //Runs when the plugin is compiled.

        public void OnPluginLoaded(string strHostName, string strPort, string strPRoConVersion) {
			// Depending on your plugin you will need different types of events. See other plugins for examples.
			this.RegisterEvents(this.GetType().Name, "OnPluginLoaded");
        }

        //Runs when the plugin is enabled (checked in Procon)
        //Note that this also runs right after a server restart if it was enabled before.

        public void OnPluginEnable() {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = true;

            //Say something in the console.
            this.ExecuteCommand("procon.protected.pluginconsole.write", "Template Plugin Enabled" );
        }

        //Runs when the pluginh is disabled (unchecked in Procon)

        public void OnPluginDisable() {
            //Use a variable like this one to turn on and off your plugin.
            this.pluginEnabled = false;

            //Say something in the console.
            this.ExecuteCommand("procon.protected.pluginconsole.write", "Template Plugin Disabled");
        }

    }
}