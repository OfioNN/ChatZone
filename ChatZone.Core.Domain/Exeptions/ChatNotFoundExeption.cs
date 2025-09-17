using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatZone.Core.Domain.Exeptions
{
    public class ChatNotFoundExeption : Exception
    {
        public ChatNotFoundExeption(string chatName)
            : base($"Chat with name '{chatName}' was not found.") {}
    }
}
