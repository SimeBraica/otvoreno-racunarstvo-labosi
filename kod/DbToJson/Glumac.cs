using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DbToJson {
    public record Glumac {
        public int Glumac_id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
    }
}
