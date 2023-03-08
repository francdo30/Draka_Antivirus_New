using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScanAutomatique
{
    internal class Scancp
    {
        public int filesCount { get; set; }
        public int file { get; set; }
        public int virus { get; set; }
        public string item { get; set; }
        public string statut { get; set; }
        public bool isInit { get; set; }
    }
}
