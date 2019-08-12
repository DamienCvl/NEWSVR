﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DevMode
{
    class DevStatsData
    {
        public readonly string playerName;
        public readonly string newsTitle;
        public readonly string reaction;
        public readonly uint nbCmt;
        public readonly DateTime date;

        public DevStatsData(string playerName, string newsTitle, uint reaction, uint nbCmt, DateTime date)
        {
            this.playerName = playerName;
            this.newsTitle = newsTitle;
            this.reaction = GetStringReaction(reaction);
            this.nbCmt = nbCmt;
            this.date = date;
        }

        private string GetStringReaction(uint reaction)
        {
            switch (reaction)
            {
                case 0:
                    return "None";
                case 1:
                    return "Happy";
                case 2:
                    return "Sad";
                case 3:
                    return "Angry";
                case 4:
                    return "Surprised";
                default:
                    return "Unknown";
            }
        }
    }
}