﻿using System;
using System.Collections.Generic;

namespace EarablesKIT.Models.DatabaseService
{
    public class DBEntry
    {
        private const string StepAmountIdentifier = "Steps";
        private const string PushUpAmountIdentifier = "PushUps";
        private const string SitUpAmountIdentifier = "SitUps";

        private DateTime _date;

        public DateTime Date => _date;

        private readonly Dictionary<string, int> _trainingsdata;

        public Dictionary<string, int> TrainingsData => _trainingsdata;

        public DBEntry(DateTime date, int stepAmount, int pushUpAmount, int sitUpAmount)
        {
            _trainingsdata = new Dictionary<string, int>();
            _trainingsdata.Add(StepAmountIdentifier, stepAmount);
            _trainingsdata.Add(PushUpAmountIdentifier, pushUpAmount);
            _trainingsdata.Add(SitUpAmountIdentifier, sitUpAmount);
            _date = date;
        }

        public override string ToString()
        {
            string result = _date.ToString("d")+ ",";
            foreach (var keyValuePair in _trainingsdata)
            {
                result += keyValuePair.Key + "=" + keyValuePair.Value + ",";
            }

            result = result.Substring(0, result.Length - 1);
            return result;
        }

        public static DBEntry ParseDbEntry(string entry)
        {
            if (entry == null)
                return null;

            var parts = entry.Split(',');

            if (parts.Length != 4)
                return null;

            if (!DateTime.TryParse(parts[0], out DateTime date))
                return null;

            if (!parts[1].StartsWith(StepAmountIdentifier + "=") || !parts[2].StartsWith(PushUpAmountIdentifier + "=") || !parts[3].StartsWith(SitUpAmountIdentifier + "="))
                return null;

            if (!int.TryParse(parts[1].Substring(parts[1].IndexOf("=") + 1), out var stepAmount))
                return null;

            if (!int.TryParse(parts[2].Substring(parts[2].IndexOf("=") + 1), out var pushUpAmount))
                return null;
            
            if (!int.TryParse(parts[3].Substring(parts[3].IndexOf("=") + 1), out var sitUpAmount))
                return null;

            return new DBEntry(date, stepAmount, pushUpAmount, sitUpAmount);
        }
    }
}