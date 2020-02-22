using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace radio
{
    public static class Constants
    {
        public const string INPUT_TXT = @"Files\veetel.txt";
        public const string OUTPUT_TXT = @"Files\adaas.txt";
        public const string SPACE = " ";
        public const string NOT_FOUND_INFORMATION = "Nincs  információ";
        public const string NOT_FOUND_RECORDING = "Nincs ilyen feljegyzés";
        public const string SLASH = "/";
        public const string NUMBER_PATTERN = @"[0-9]+/[0-9]+ ";
    }

    public struct Records
    {
        public int Day;
        public int StationId;
        public string Data;
    }

    public class RadioStation
    {
        private List<Records> _lines;

        #region 1. Feladat
        public RadioStation()
        {
            string[] lines = System.IO.File.ReadAllLines(Constants.INPUT_TXT);
            _lines = new List<Records>();
            string[] separator = { Constants.SPACE };

            for (int i = 0; i < lines.Length; i++)
            {
                string[] details = lines[i].Split(separator, 3, StringSplitOptions.RemoveEmptyEntries);
                int day = ConvertStringToInteger(details[0]);
                int id = ConvertStringToInteger(details[1]);

                var record = new Records()
                {
                    Day = day,
                    StationId = id,
                    Data = lines[++i]
                };

                _lines.Add(record);
            }
        }
        #endregion

        #region 2. Feladat
        public string GetFirstRadioStation() => _lines[0].StationId.ToString();
        public string GetLastRadioStation() => _lines[_lines.Count - 1].StationId.ToString();
        #endregion

        #region 3. Feladat
        public List<string> GetSpecialDays(string searchString) => _lines.Where(x => x.Data.Contains(searchString)).Select(x => $"{x.Day}. nap {x.StationId}. rádióamatőr").ToList();
        #endregion

        #region 4. Feladat
        public List<string> GetStat()
        {
            var returnList = new List<string>();

            for (int i = 0; i < 11; ++i)
                returnList.Add($"{i + 1}. nap: {_lines.Count(x => x.Day == i + 1)} rádióamatőr");

            return returnList;
        }
        #endregion

        #region 5. Feladat
        public void Reconstuction()
        {
            var recovered = new List<string>();

            for (int i = 0; i < 11; i++)
            {
                List<string> DataOfDay = _lines.Where(x => x.Day == i + 1).Select(x => x.Data).ToList();
                var recoveredString = new StringBuilder();

                for (int j = 0; j < DataOfDay.Count; j++)
                {
                    for (int stringIndex = 0; stringIndex < 90; stringIndex++)
                    {
                        if (j == 0)
                            recoveredString.Append('#');

                        if (DataOfDay[j][stringIndex] != '#')
                            recoveredString[stringIndex] = DataOfDay[j][stringIndex];
                    }
                }

                recovered.Add(recoveredString.ToString());
            }

            System.IO.File.WriteAllLines(Constants.OUTPUT_TXT, recovered);
        }
        #endregion

        #region 6. Feladat
        public bool Szame(string szo)
        {
            bool valasz = true;

            for (int i = 0; i < szo.Length; i++)
                if (szo[i] < '0' || szo[i] > '9')
                    valasz = false;

            return valasz;
        }
        #endregion

        #region 7. Feladat
        public string SearchData(int stationId, int day)
        {
            if (_lines.Any(x => x.StationId == stationId && x.Day == day))
            {
                if (_lines.Where(x => x.StationId == stationId && x.Day == day).Any(x => !Szame(x.Data)))
                {
                    Match m = Regex.Match(_lines.Single(x => x.StationId == stationId && x.Day == day).Data, Constants.NUMBER_PATTERN, RegexOptions.IgnoreCase);

                    if (m.Success)
                    {
                        string[] substrings = Regex.Split(m.Value, Constants.SLASH);
                        return $"A megfigyelt egyedek száma {ConvertStringToInteger(substrings[0]) + ConvertStringToInteger(substrings[1])}";
                    }
                    else
                        return Constants.NOT_FOUND_INFORMATION;
                }
                else
                    return Constants.NOT_FOUND_INFORMATION;
            }
            else
                return Constants.NOT_FOUND_RECORDING;
        }
        #endregion

        #region Helper functions
        public int ConvertStringToInteger(string str)
        {
            int.TryParse(str, out int number);
            return number;
        }
        #endregion
    }
}

