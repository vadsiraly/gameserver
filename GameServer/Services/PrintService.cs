using GameServer.Model;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class PrintService
    {
        int tableWidth = 150;

        public void Print(Team attacker, Team defender)
        {
            Console.WriteLine("Attackers");
            PrintTeam(attacker);
            Console.WriteLine("Defenders");
            PrintTeam(defender);
        }

        public void WrapPrint(Action action, Team attacker, Team defender, string text, bool stop = true)
        {
            //action();
            //return;

            Console.WriteLine($"\n----------------------------------------\nBefore {text}\n----------------------------------------");
            Print(attacker, defender);
            if (stop)
                Console.ReadKey();

            action();

            Console.WriteLine($"\n----------------------------------------\nAfter {text}\n----------------------------------------");
            Print(attacker, defender);

            if (stop)
                Console.ReadKey();
        }

        private void PrintTeam(Team team)
        {
            PrintRow(ConsoleColor.Gray, team.Units.Select(x => x.Name + (x.IsDead ? " (dead)" : "")).ToArray());
            PrintRow(ConsoleColor.Green, team.Units.Select(x => ProgressBar(x.Health, x.MaxHealth)).ToArray());
            PrintRow(ConsoleColor.Blue, team.Units.Select(x => ProgressBar(x.Mana, x.MaxMana)).ToArray());
            PrintRow(ConsoleColor.Red, team.Units.Select(x => $"{Math.Abs(x.DamageDone)}").ToArray());
            PrintRow(ConsoleColor.Green, team.Units.Select(x => $"{x.HealingDone}").ToArray());
            PrintRow(ConsoleColor.Yellow, team.Units.Select(x =>
                x.Effects.Count(x => x.Effect.Schedule == EffectSchedule.Continuous) > 0 ?
                $"{x.Effects.Where(x => x.Effect.Schedule == EffectSchedule.Continuous).Select(x => x.SourceAbility.Name).Aggregate((c, n) => c + "," + n)}" :
                "No persistent effects"
            ).ToArray());
            PrintRow(ConsoleColor.Yellow, team.Units.Select(x =>
                x.Effects.Count(x => x.Effect.Schedule == EffectSchedule.AfterRound) > 0 ?
                $"{x.Effects.Where(x => x.Effect.Schedule == EffectSchedule.AfterRound).Select(x => x.SourceAbility.Name).Aggregate((c, n) => c + "," + n)}" :
                "No after round effects"
            ).ToArray());
            PrintRow(ConsoleColor.DarkMagenta, team.Units.Select(x =>
                x.Effects.Count(x => x.Effect.Schedule == EffectSchedule.Delayed) > 0 ?
                $"{x.Effects.Where(x => x.Effect.Schedule == EffectSchedule.Delayed).Select(x => x.SourceAbility.Name).Aggregate((c, n) => c + "," + n)}" :
                "No delayed effects"
            ).ToArray());
        }

        private string ProgressBar(double value, double max)
        {
            var progressbar = string.Empty;
            var percentage = max == 0 ? 0 : (int)(value / max * 10);
            progressbar += "[";
            foreach(var i in Enumerable.Range(0, percentage))
            {
                progressbar += "█";
            }
            foreach (var i in Enumerable.Range(0, 10 - percentage))
            {
                progressbar += " ";
            }
            progressbar += $"] ({value}/{max})";

            return progressbar;
        }

        private void PrintRow(ConsoleColor color, params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            Console.Write("|");

            foreach (string column in columns)
            {

                if (column.Contains("(dead)")) 
                    Console.BackgroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = color;

                Console.Write(AlignCentre(column, width));
                Console.ResetColor();

                Console.Write("|");
            }
            Console.WriteLine();
        }

        private string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}
