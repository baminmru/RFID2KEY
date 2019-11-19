using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace CR2KService
{
    [RunInstaller(true)]
    public partial class CR2KInstaller : Installer
    {
        public CR2KInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}