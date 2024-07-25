using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;

namespace ImmersiveGames.Utils
{
    public enum ComparisonOperator { LessOrEqual, GreaterOrEqual }

    public class PercentageTrack
    {
        public int Threshold { get; set; }
        public Action Action { get; set; }
        public ComparisonOperator Operator { get; set; }
    }

    public class PercentageTracker
    {
        private readonly List<PercentageTrack> _tracks = new();
        private readonly HashSet<int> _executedTracks = new();

        public void AddTrack(int percentageThreshold, Action action, ComparisonOperator op = ComparisonOperator.LessOrEqual)
        {
            _tracks.Add(new PercentageTrack { Threshold = percentageThreshold, Action = action, Operator = op });
        }

        public void VerifyPercentage(double actualPercentage)
        {
            var roundedPercentage = (int)Math.Floor(actualPercentage);

            DebugManager.Log<PercentageTrack>($"Verificando porcentagem atual: {roundedPercentage}%");

            // Verifica LessOrEqual em ordem decrescente
            foreach (var track in _tracks.Where(x => x.Operator == ComparisonOperator.LessOrEqual).OrderByDescending(x => x.Threshold))
            {
                if (_executedTracks.Contains(track.Threshold)) continue;
                if (roundedPercentage > track.Threshold) continue;
                DebugManager.Log<PercentageTrack>($"Atingiu a faixa: {track.Threshold}%");
                track.Action();
                _executedTracks.Add(track.Threshold);
            }

            // Verifica GreaterOrEqual em ordem crescente
            foreach (var track in _tracks.Where(x => x.Operator == ComparisonOperator.GreaterOrEqual).OrderBy(x => x.Threshold))
            {
                if (_executedTracks.Contains(track.Threshold)) continue;
                if (roundedPercentage < track.Threshold) continue;
                DebugManager.Log<PercentageTrack>($"Atingiu a faixa: {track.Threshold}%");
                track.Action();
                _executedTracks.Add(track.Threshold);
            }
        }
    }
}
