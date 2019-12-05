using System;
using System.Collections.Generic;
using System.Text;

namespace NetPatterns
{
    public interface ICommand
    {
        void Execute();
        //void Undo();
    }

    public class Invoker 
    {
        SortedList<string, ICommand> commands;
        
        //Stack<ICommand> executed;

        public Invoker()
        {
            commands = new SortedList<string, ICommand>();
        }

        public void Add(string code, ICommand cmd)
        {
            commands.Add(code, cmd);
        }

        public void Remove(string code)
        {
            if (commands.Keys.Contains(code))
               commands.Remove(code);
        }

        public void Execute(string code) 
        {
            if (commands.Keys.Contains(code))
            {
                commands[code].Execute();
                //executed.Push(commands[code]);
            }
        }

        public IList<string> getCommandNames()
        {
            return commands.Keys;
        }
    }
}
