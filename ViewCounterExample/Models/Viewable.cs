using System.Collections.Generic;

namespace ViewCounterExample.Models
{
    public abstract class Viewable : BaseEntity
    {
        public Viewable()
        {
            Views = new List<View>();
        }

        public int ViewsCount { get; set; }
        public List<View> Views { get; }
    }
}
