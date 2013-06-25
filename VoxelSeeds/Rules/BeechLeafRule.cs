﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelSeeds.Rules
{
    class BeechLeafRule : IVoxelRule
    {
        
        public VoxelInfo[, ,] ApplyRule(VoxelInfo[, ,] neighbourhood)
        {
            // Apply each n-th turn
            if (neighbourhood[1, 1, 1].Ticks < TypeInformation.GetGrowingSteps(VoxelType.BEECH_LEAF)) return null;

            VoxelInfo[, ,] output = new VoxelInfo[3, 3, 3];
            int gen = neighbourhood[1, 1, 1].Generation;
            int res = neighbourhood[1, 1, 1].Resources;
            if (gen == 0)
            {
                output[1, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF);
                if (CanPlace(2,1,1,neighbourhood))
                    output[2, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF, true, 1);
                if (CanPlace(0, 1, 1, neighbourhood))
                    output[0, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF, true, 1);
                if (CanPlace(1, 1, 2, neighbourhood))
                    output[1, 1, 2] = new VoxelInfo(VoxelType.BEECH_LEAF, true, 1);
                if (CanPlace(1, 1, 0, neighbourhood))
                    output[1, 1, 0] = new VoxelInfo(VoxelType.BEECH_LEAF, true, 1);
                if (CanPlace(1, 2, 1, neighbourhood))
                    output[1, 2, 1] = new VoxelInfo(VoxelType.BEECH_LEAF, true, 3);
            }
            else if (gen < TypeInformation.GetGrowHeight(VoxelType.BEECH_LEAF))
            {
                // Grow to the side
                output[1, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF);
                if (CanPlace(2, 1, 1, neighbourhood))
                    output[2, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF, true, gen + Random.Next(1, 3), 0, Random.Next(0, TypeInformation.GetGrowingSteps(VoxelType.BEECH_LEAF)));
                if (CanPlace(0, 1, 1, neighbourhood))
                    output[0, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF, true, gen + Random.Next(1, 3), 0, Random.Next(0, TypeInformation.GetGrowingSteps(VoxelType.BEECH_LEAF)));
                if (CanPlace(1, 1, 2, neighbourhood))
                    output[1, 1, 2] = new VoxelInfo(VoxelType.BEECH_LEAF, true, gen + Random.Next(1, 3), 0, Random.Next(0, TypeInformation.GetGrowingSteps(VoxelType.BEECH_LEAF)));
                if (CanPlace(1, 1, 0, neighbourhood))
                    output[1, 1, 0] = new VoxelInfo(VoxelType.BEECH_LEAF, true, gen + Random.Next(1, 3), 0, Random.Next(0, TypeInformation.GetGrowingSteps(VoxelType.BEECH_LEAF)));
            }
            else
            {
                // Stop growing
                output[1, 1, 1] = new VoxelInfo(VoxelType.BEECH_LEAF);
            }
            return output;
        }

        bool CanPlace(int t, int h, int b, VoxelInfo[, ,] neighbourhood)
        {
            return (neighbourhood[t, h, b].Type == VoxelType.EMPTY);
            //    || TypeInformation.IsNotWoodButBiomass(neighbourhood[t, h, b].Type);
        }
    }
}