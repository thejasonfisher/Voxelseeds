﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxelSeeds.Rules
{

    /// <summary>
    /// Random flowing beetle.
    /// 
    /// Actions:
    ///     * Spawn if possible (no eat and move)
    ///     * Eat if possible (no move)
    ///     * Move if nothing to see.
    /// Eats: +Teak+, +Oak+, Beech, Redwood and there leaves.
    /// Spawns: #1 - If Resources > GetGrowingSteps.
    /// Dies: ---
    /// 
    /// Interpretions of values:
    ///     * Resources: How many ticks of successfull eating
    /// </summary>
    class HesperophanesCinnereusRule: IVoxelRule
    {
        public VoxelInfo[, ,] ApplyRule(VoxelInfo[, ,] neighbourhood)
        {
            VoxelInfo[, ,] output = new VoxelInfo[3, 3, 3];
            int res = neighbourhood[1, 1, 1].Resources;

            int t, h, b;
            VoxelType typeOfTarget = GamePlayUtils.GetEatableTarget(VoxelType.HESPEROPHANES_CINNEREUS, neighbourhood, out t, out h, out b);

            if (res >= TypeInformation.GetGrowingSteps(VoxelType.HESPEROPHANES_CINNEREUS))
            {
                // Spawn
                if (GamePlayUtils.GetEmptyTarget(neighbourhood, out t, out h, out b))
                {
                    output[t, h, b] = new VoxelInfo(VoxelType.HESPEROPHANES_CINNEREUS, true);
                    res -= TypeInformation.GetGrowingSteps(VoxelType.HESPEROPHANES_CINNEREUS);
                    output[1, 1, 1] = new VoxelInfo(VoxelType.HESPEROPHANES_CINNEREUS, true, 0, res);
                }
                else return null;
            }
            else if (!TypeInformation.IsResistent(typeOfTarget, VoxelType.HESPEROPHANES_CINNEREUS))
            {
                // Try to eat
                if (Random.Next(101) > TypeInformation.GetResistance(typeOfTarget, VoxelType.HESPEROPHANES_CINNEREUS))
                {
                    output[t, h, b] = new VoxelInfo(VoxelType.EMPTY);
                    // If food is consumed completely growth.
                    ++res;
                }
                output[1, 1, 1] = new VoxelInfo(VoxelType.HESPEROPHANES_CINNEREUS, true, 0, res);
            }
            else
            {
                // Move
                if (typeOfTarget == VoxelType.EMPTY)
                {
                    output[1, 1, 1] = new VoxelInfo(VoxelType.EMPTY);
                    output[t, h, b] = new VoxelInfo(VoxelType.HESPEROPHANES_CINNEREUS, true, 0, res);
                }
                else return null;
            }
            return output;
        }
    }
}
