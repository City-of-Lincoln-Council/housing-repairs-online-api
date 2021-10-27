using System.Collections.Generic;

namespace HousingRepairsOnlineApi.Helpers
{
    public class SoREngine
    {
        private Dictionary<string, Dictionary<string, Dictionary<string, string>>> sorMapping =
            new()
            {
                {
                    "kitchen",
                    new Dictionary<string, Dictionary<string, string>>
                    {
                        {
                            "cupboards", new Dictionary<string, string>
                            {
                                { "doorHangingOff", "N373049" },
                                { "doorMissing", "N373049" },
                            }
                        },
                        {
                            "worktop", new Dictionary<string, string>
                            {
                                { "damaged", "N372005" },
                            }
                        },
                        {
                            "dampMould", new Dictionary<string, string>
                            {
                                {"dampMould", "N114001"},
                            }
                        },
                        {
                            "electrical", new Dictionary<string, string>
                            {
                                {"extractorFan", "N841007"},
                                {"sockets", "N861511"},
                                {"lightFitting", "N858111"},
                            }
                        },
                        {
                            "sink", new Dictionary<string, string>
                            {
                                {"taps", "N630147"},
                                {"pipeworkLeak", "N630147"},
                            }
                        },
                        {
                            "wallsFloorsCeiling", new Dictionary<string, string>
                            {
                                {"wallTiles", "N431041"},
                                {"floorTiles", "N432005"},
                                {"lightFitting", "N858111"},
                                {"skirtingBoardArchitrave", "N381001"},
                                {"plasteringCeiling", "N413305"},
                                {"plasteringWalls", "N431023"},
                            }
                        },
                        {
                            "window", new Dictionary<string, string>
                            {
                                {"stuckShut", "N318125"},
                                {"condensated", "N318151"},
                            }
                        },
                        {
                            "door", new Dictionary<string, string>
                            {
                                {"backDoorWooden", "N324123"},
                                {"backDoorUPVC", "N325117"},
                                {"backFrenchDoors", "N325117"},
                                {"internal", "N330007"},
                                {"sliding", "N330007"},
                            }
                        }
                    }
                },
                {
                    "bathroom",
                    new Dictionary<string, Dictionary<string, string>>
                    {
                        {
                            "bath", new Dictionary<string, string>
                            {
                                {"bathTaps", "N631301"},
                                {"sealAroundBath", "N630945"},
                                {"bathPanel", "N630945"},
                                {"bathBlockage", "N630945"},
                            }
                        },
                        {
                            "shower", new Dictionary<string, string>
                            {
                                {"electricShower", "N631131"},
                                {"showerTap", "N631337"},
                                {"showerHose", "N631111"},
                                {"showerHead", "N631121"},
                            }
                        },
                        {
                            "sink", new Dictionary<string, string>
                            {
                                {"sinkTaps", "N631301"},
                                {"damagedSink", "N630714"},
                                {"leakBlockedSink", "N620507"},
                            }
                        },
                        {
                            "toilet", new Dictionary<string, string>
                            {
                                {"notFlushing", "N630573"},
                                {"overflowing", "N630573"},
                                {"looseFromFloor", "N630516"},
                            }
                        }
                    }
                }
            };

        public string MapSorCode(string location, string problem, string issue)
        {
            var result = sorMapping[location][problem][issue];

            return result;
        }
    }
}
