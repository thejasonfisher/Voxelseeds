﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelSeeds
{
    class Level3 : Level
    {
        public Level3(VoxelRenderer voxelRenderer) : base(voxelRenderer)
        {
        }

        override public void Initialize()
        {
            _automaton = new Automaton(200, 60, 200, LevelType.BUBBLE, 3485704, 1.3f);

            _resources = 8000;
            _finalParasiteMass = 10000;
            _targetBiomass = 5000;

            int x = GetMap().SizeX / 2 + 5;
            int z = GetMap().SizeZ / 2;
            int y = GetMap().GetHeighest(x, z);
            InsertSeed(x, Math.Max(y, 0) + 1, z, VoxelType.NOBLEROT_FUNGUS);
        }
    }
}
