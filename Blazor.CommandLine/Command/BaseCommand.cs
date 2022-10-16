using Blazor.CommandLine.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.NamingConventionBinder;

namespace Blazor.CommandLine.Command
{
    public abstract class BaseCommand
    {
        readonly System.CommandLine.Command _command;
        readonly IRunningCommand _loadingService;
        internal System.CommandLine.Command Command => _command;
        
        public BaseCommand(string name, string description, bool longRunning = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException($"Command name can not have white space.", nameof(name));
            }

            _command = new System.CommandLine.Command(name, description);

            if (longRunning)
            {
                _loadingService = new RunningCommand();
            }

            Handle();

        }

        private void Handle()
        {
            _command.Handler = CommandHandler.Create<CancellationToken, List<string>, IConsole, string, string, string, string>(async (token, args, console, o1, o2, o3, o4) =>
            {
                if (_loadingService != null)
                {
                    await _loadingService.StartCommandAsync(async (task) =>
                    {
                        await ExecuteAsync(console.Out as DefaultStreamWriter,o1, o2, o3, o4, args);
                    }, "Execution is started...");
                }
                else
                {
                    Execute(console.Out as DefaultStreamWriter, o1, o2, o3, o4, args);
                }
            });
        }


        public virtual async Task<bool> ExecuteAsync(DefaultStreamWriter console, string option1, string option2, string option3, string option4, List<string> arguments)
        {
            await Task.Delay(200);
            console.WriteLine("ExecuteAsync() is not implemented.");

            return false;
        }
        public virtual bool Execute(DefaultStreamWriter console, string option1, string option2, string option3, string option4, List<string> arguments)
        {
            console.WriteLine("Execute() is not implemented.");
            return false;
        }

        public void AddOption(string name, string description)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new System.ArgumentNullException(nameof(name));
            }
            int optionCount = _command.Options.Count();
            optionCount = optionCount == 0 ? 1 : optionCount + 1;
            var optionName = $"-o{optionCount.ToString()}";
            string[] aliases = new string[] { name, optionName };
                
            _command.AddOption(new Option<string>(aliases)
            {
                Description = description,
                Name = name
            });

            Handle();
            /*int optionCount = _command.Options.Count();
            if (optionCount < 4)
            {
                optionCount = optionCount == 0 ? 1 : optionCount + 1;
                var optionName = $"-o{optionCount.ToString()}";
                string[] aliases = new string[] { name, optionName };
                
                _command.AddOption(new Option<string>(aliases)
                {
                    Description = description,
                    Name = name
                });

                Handle();
            }
            else
            {
                return;
            }*/

        }

        public void UseArguments(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new System.ArgumentNullException(nameof(description));

            var argument = new Argument<List<string>>();
            argument.Name = "args";
            argument.Description = description;
            argument.Arity = ArgumentArity.ZeroOrMore;
            _command.AddArgument(argument);

            Handle();
        }


    }
}