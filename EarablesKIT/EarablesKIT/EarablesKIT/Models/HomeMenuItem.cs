﻿namespace EarablesKIT.Models
{
    public enum MenuItemType
    {
        Test,
        About,
        CountMode,
        DataOverview,
        ImportExport,
        ListenAndPerform,
        MusicMode,
        Settings,
        StepMode,
        DebugMode,
        Imprint
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
