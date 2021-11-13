﻿using System;
using System.Globalization;
using System.Text.Json.Serialization;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public class Grade : IGradable
    {
        public Grade(string name, float value, float weighting, DateTime date, bool counts)
        {
            this.Name = name;
            this.GradeValue = value;
            this.Weighting = weighting;
            this.Date = date;
            this.Counts = counts;
        }
        
        [JsonInclude]
        public string Name { get; private set; }
        
        [JsonInclude]
        public float GradeValue { get; private set; }
        
        [JsonInclude]
        public float Weighting { get; private set; }
        
        [JsonInclude]
        public DateTime Date { get; private set; }
        
        [JsonInclude]
        public bool Counts { get; private set; }

        internal float RoundedGrade => (float)Math.Round(GradeValue, 2);
        internal string DateString => Date.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture);
    }
}