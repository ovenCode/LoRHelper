using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace desktop.data.Models.DTOs
{
    public class MatchDTO
    {
        [Key]
        public int Id { get; set; }
        public bool IsWin { get; set; }
        public string DeckCode { get; set; } = null!;
        public string Opponent { get; set; } = null!;
        public GameType GameType { get; set; }
        public List<RegionDTO> Regions { get; set; } = new List<RegionDTO>();
        public List<RegionDTO> OpponentRegions { get; set; } = new List<RegionDTO>();
    }

    public class MatchParser
    {
        public static Func<MatchDTO, Match> ToMatch = (match) =>
            new Match
            {
                Id = match.Id,
                IsWin = match.IsWin,
                DeckCode = match.DeckCode,
                Opponent = match.Opponent,
                GameType = match.GameType,
                Regions = match
                    .Regions.Select(region => new Region
                    {
                        RegionType = (Regions)(
                            Array.IndexOf(Enum.GetNames(typeof(Regions)), region.Region)
                        )
                    })
                    .ToList(),
                OpponentRegions = match
                    .OpponentRegions.Select(region => new Region
                    {
                        RegionType = (Regions)(
                            Array.IndexOf(Enum.GetNames(typeof(Regions)), region.Region)
                        )
                    })
                    .ToList()
            };
        public static Func<Match, MatchDTO> ToMatchDTO = (match) =>
            new MatchDTO
            {
                Id = match.Id,
                IsWin = match.IsWin,
                DeckCode = match.DeckCode,
                Opponent = match.Opponent,
                GameType = match.GameType,
                Regions = match
                    .Regions.Select(region => new RegionDTO
                    {
                        Id = Array.IndexOf(Enum.GetValues(typeof(Regions)), region.RegionType),
                        Region = region.RegionType.ToString()
                    })
                    .ToList(),
                OpponentRegions = match
                    .OpponentRegions.Select(region => new RegionDTO
                    {
                        Id = Array.IndexOf(Enum.GetValues(typeof(Regions)), region.RegionType),
                        Region = region.RegionType.ToString()
                    })
                    .ToList()
            };
    }
}
