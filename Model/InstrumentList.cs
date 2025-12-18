using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class InstrumentList : List<Instrument>
    {
        public InstrumentList() { }
        public InstrumentList(IEnumerable<Instrument> list) : base(list) { }
        public InstrumentList(IEnumerable<BaseEntity> list) : base(list.Cast<Instrument>().ToList()) { }
    }
}
