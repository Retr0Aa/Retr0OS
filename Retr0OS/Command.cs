using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retr0OS
{
    public abstract class Command
    {
        public string keyword;
        public string description;

        protected Command(string keyword, string description)
        {
            this.keyword = keyword;
            this.description = description;
        }

        public abstract void Execute(string[] args);
    }
}
