﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelSeeds
{
    class Level1: Level
    {
        override public void Initialize()
        {
            _automaton = new Automaton(100, 50, 100, LevelType.PLAIN, 34857024);
            _numRemainingSeeds[(int)VoxelType.WOOD] = 3;

            _automaton.InsertSeed(50, 45, 50, VoxelType.WOOD);
        }
    }
}