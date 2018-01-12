using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsStore.ViewModels
{
    public class SettingsViewModel
    {
        public Setting ContactEmail { get; set; }
        public Setting ApplicationEmail { get; set; }
        public Setting ApplicationEmailPassword { get; set; }
    }
}